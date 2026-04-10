using AutoMapper;
using MyFullstackApp.DataAccess.Context;
using MyFullstackApp.Domains.Entities.Capsule;
using MyFullstackApp.Domains.Models.Base;
using MyFullstackApp.Domains.Models.Capsule;

namespace MyFullstackApp.BusinessLogic.Core.Capsules;

public class CapsuleLocationAction
{
    protected readonly IMapper Mapper;

    protected CapsuleLocationAction(IMapper mapper)
    {
        Mapper = mapper;
    }

    protected List<CapsuleLocationDto> ExecuteGetAllCapsuleLocationsAction()
    {
        using var db = new AppDbContext();
        return Mapper.Map<List<CapsuleLocationDto>>(db.CapsuleLocations.ToList());
    }

    protected CapsuleLocationDto? GetCapsuleLocationDataByIdAction(int id)
    {
        using var db = new AppDbContext();
        var l = db.CapsuleLocations.FirstOrDefault(x => x.Id == id);
        return l == null ? null : Mapper.Map<CapsuleLocationDto>(l);
    }

    protected CapsuleLocationDto? GetCapsuleLocationDataByCapsuleIdAction(int capsuleId)
    {
        using var db = new AppDbContext();
        var l = db.CapsuleLocations.FirstOrDefault(x => x.CapsuleId == capsuleId);
        return l == null ? null : Mapper.Map<CapsuleLocationDto>(l);
    }

    protected ResponceMsg ExecuteCapsuleLocationCreateAction(CapsuleLocationDto location)
    {
        using var db = new AppDbContext();
        if (!db.TimeCapsules.Any(c => c.Id == location.CapsuleId))
        {
            return new ResponceMsg { IsSuccess = false, Message = "Capsule not found." };
        }

        if (db.CapsuleLocations.Any(l => l.CapsuleId == location.CapsuleId))
        {
            return new ResponceMsg { IsSuccess = false, Message = "Location for this capsule already exists. Use update." };
        }

        var entity = Mapper.Map<CapsuleLocationData>(location);
        entity.Id = 0;
        entity.Capsule = null!;

        db.CapsuleLocations.Add(entity);
        db.SaveChanges();

        return new ResponceMsg { IsSuccess = true, Message = "Location was successfully added." };
    }

    protected ResponceMsg ExecuteCapsuleLocationUpdateAction(CapsuleLocationDto location)
    {
        using var db = new AppDbContext();
        var data = db.CapsuleLocations.FirstOrDefault(x => x.Id == location.Id);
        if (data == null)
        {
            return new ResponceMsg { IsSuccess = false, Message = "Location not found." };
        }

        if (!db.TimeCapsules.Any(c => c.Id == location.CapsuleId))
        {
            return new ResponceMsg { IsSuccess = false, Message = "Capsule not found." };
        }

        data.CapsuleId = location.CapsuleId;
        data.Latitude = location.Latitude;
        data.Longitude = location.Longitude;
        data.PlaceLabel = location.PlaceLabel;

        db.SaveChanges();

        return new ResponceMsg { IsSuccess = true, Message = "Location updated successfully." };
    }

    protected ResponceMsg ExecuteCapsuleLocationDeleteAction(int id)
    {
        using var db = new AppDbContext();
        var data = db.CapsuleLocations.FirstOrDefault(x => x.Id == id);
        if (data == null)
        {
            return new ResponceMsg { IsSuccess = false, Message = "Location not found." };
        }

        db.CapsuleLocations.Remove(data);
        db.SaveChanges();

        return new ResponceMsg { IsSuccess = true, Message = "Location deleted successfully." };
    }
}
