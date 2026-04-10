using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyFullstackApp.DataAccess.Context;
using MyFullstackApp.Domains.Entities.Capsule;
using MyFullstackApp.Domains.Entities.Moderation;
using MyFullstackApp.Domains.Enums;
using MyFullstackApp.Domains.Models.Base;
using MyFullstackApp.Domains.Models.Capsule;

namespace MyFullstackApp.BusinessLogic.Core.Capsules;

public class TimeCapsuleAction
{
    protected readonly IMapper Mapper;

    protected TimeCapsuleAction(IMapper mapper)
    {
        Mapper = mapper;
    }

    protected static ResponceMsg? ValidatePayload(TimeCapsuleDto c)
    {
        return c.ContentType switch
        {
            CapsuleContentType.Text when string.IsNullOrWhiteSpace(c.TextContent) =>
                new ResponceMsg { IsSuccess = false, Message = "Text content is required for text capsules." },
            CapsuleContentType.Link when string.IsNullOrWhiteSpace(c.LinkUrl) =>
                new ResponceMsg { IsSuccess = false, Message = "Link URL is required for link capsules." },
            CapsuleContentType.File when string.IsNullOrWhiteSpace(c.FileStoragePath) =>
                new ResponceMsg { IsSuccess = false, Message = "File path is required for file capsules." },
            _ => null
        };
    }

    protected List<TimeCapsuleDto> ExecuteGetAllTimeCapsulesAction()
    {
        using var db = new AppDbContext();
        var list = db.TimeCapsules.Include(c => c.Owner).ToList();
        return list.Select(MapCapsule).ToList();
    }

    protected TimeCapsuleDto? GetTimeCapsuleDataByIdAction(int id)
    {
        using var db = new AppDbContext();
        var c = db.TimeCapsules.Include(x => x.Owner).FirstOrDefault(x => x.Id == id);
        return c == null ? null : MapCapsule(c);
    }

    protected List<TimeCapsuleDto> ExecuteGetTimeCapsulesByOwnerAction(int ownerUserId)
    {
        using var db = new AppDbContext();
        return db.TimeCapsules
            .Include(c => c.Owner)
            .Where(c => c.OwnerUserId == ownerUserId)
            .OrderByDescending(c => c.CreatedAtUtc)
            .AsEnumerable()
            .Select(MapCapsule)
            .ToList();
    }

    protected List<TimeCapsuleDto> ExecuteGetPublicFeedAction()
    {
        var now = DateTime.UtcNow;
        using var db = new AppDbContext();
        return db.TimeCapsules
            .Include(c => c.Owner)
            .Where(c => c.IsPublic && c.OpenAtUtc <= now)
            .OrderByDescending(c => c.OpenAtUtc)
            .AsEnumerable()
            .Select(MapCapsule)
            .ToList();
    }

    private TimeCapsuleDto MapCapsule(TimeCapsuleData c)
    {
        var dto = Mapper.Map<TimeCapsuleDto>(c);
        dto.OwnerDisplayName = c.Owner?.DisplayName;
        return dto;
    }

    protected ResponceMsg ExecuteTimeCapsuleCreateAction(TimeCapsuleDto capsule)
    {
        var validation = ValidatePayload(capsule);
        if (validation != null)
        {
            return validation;
        }

        using var db = new AppDbContext();
        if (!db.UserAccounts.Any(u => u.Id == capsule.OwnerUserId))
        {
            return new ResponceMsg { IsSuccess = false, Message = "Owner user not found." };
        }

        var entity = Mapper.Map<TimeCapsuleData>(capsule);
        entity.Id = 0;
        entity.CreatedAtUtc = DateTime.UtcNow;
        entity.Owner = null!;
        entity.Location = null;
        entity.Reports = new List<ModerationReportData>();

        db.TimeCapsules.Add(entity);
        db.SaveChanges();

        return new ResponceMsg { IsSuccess = true, Message = "Capsule was successfully created." };
    }

    protected ResponceMsg ExecuteTimeCapsuleUpdateAction(TimeCapsuleDto capsule)
    {
        var validation = ValidatePayload(capsule);
        if (validation != null)
        {
            return validation;
        }

        using var db = new AppDbContext();
        var data = db.TimeCapsules.FirstOrDefault(x => x.Id == capsule.Id);
        if (data == null)
        {
            return new ResponceMsg { IsSuccess = false, Message = "Capsule not found." };
        }

        if (!db.UserAccounts.Any(u => u.Id == capsule.OwnerUserId))
        {
            return new ResponceMsg { IsSuccess = false, Message = "Owner user not found." };
        }

        data.OwnerUserId = capsule.OwnerUserId;
        data.ContentType = capsule.ContentType;
        data.Title = capsule.Title;
        data.TextContent = capsule.TextContent;
        data.LinkUrl = capsule.LinkUrl;
        data.FileStoragePath = capsule.FileStoragePath;
        data.OpenAtUtc = capsule.OpenAtUtc;
        data.RecipientEmail = capsule.RecipientEmail;
        data.IsPublic = capsule.IsPublic;

        db.SaveChanges();

        return new ResponceMsg { IsSuccess = true, Message = "Capsule updated successfully." };
    }

    protected ResponceMsg ExecuteTimeCapsuleDeleteAction(int id)
    {
        using var db = new AppDbContext();
        var data = db.TimeCapsules.FirstOrDefault(x => x.Id == id);
        if (data == null)
        {
            return new ResponceMsg { IsSuccess = false, Message = "Capsule not found." };
        }

        db.TimeCapsules.Remove(data);
        db.SaveChanges();

        return new ResponceMsg { IsSuccess = true, Message = "Capsule deleted successfully." };
    }
}
