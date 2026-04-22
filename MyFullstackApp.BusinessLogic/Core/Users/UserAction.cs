using AutoMapper;
using MyFullstackApp.DataAccess.Context;
using MyFullstackApp.Domains.Entities.Capsule;
using MyFullstackApp.Domains.Entities.User;
using MyFullstackApp.Domains.Models.Base;
using MyFullstackApp.Domains.Models.User;

namespace MyFullstackApp.BusinessLogic.Core.Users;

public class UserAction
{
    protected readonly IMapper Mapper;

    protected UserAction(IMapper mapper)
    {
        Mapper = mapper;
    }

    protected List<UserAccountDto> ExecuteGetAllUserAccountsAction()
    {
        using var db = new AppDbContext();
        return Mapper.Map<List<UserAccountDto>>(db.UserAccounts.ToList())
            .Select(u =>
            {
                u.Password = string.Empty;
                return u;
            })
            .ToList();
    }

    protected UserAccountDto? GetUserAccountDataByIdAction(int id)
    {
        using var db = new AppDbContext();
        var u = db.UserAccounts.FirstOrDefault(x => x.Id == id);
        if (u == null)
        {
            return null;
        }

        var dto = Mapper.Map<UserAccountDto>(u);
        dto.Password = string.Empty;
        return dto;
    }

    protected UserLoginResultDto ExecuteLoginUserAction(UserLoginRequestDto request)
    {
        using var db = new AppDbContext();

        var email = request.Email.Trim().ToLowerInvariant();
        var role = request.Role.Trim().ToLowerInvariant();

        var user = db.UserAccounts.FirstOrDefault(x =>
            x.Email.ToLower() == email &&
            x.Password == request.Password &&
            x.Role.ToLower() == role);

        if (user == null)
        {
            return new UserLoginResultDto
            {
                IsSuccess = false,
                Message = "Пользователь с такими email, паролем и ролью не найден.",
                Role = "guest"
            };
        }

        return new UserLoginResultDto
        {
            IsSuccess = true,
            Message = "Вход выполнен успешно.",
            UserId = user.Id,
            Role = user.Role,
            DisplayName = user.DisplayName
        };
    }

    protected ResponceMsg ExecuteUserAccountCreateAction(UserAccountDto user)
    {
        using var db = new AppDbContext();
        if (string.IsNullOrWhiteSpace(user.Password))
        {
            return new ResponceMsg { IsSuccess = false, Message = "Password is required." };
        }

        if (db.UserAccounts.Any(x => x.Email.Equals(user.Email)))
        {
            return new ResponceMsg { IsSuccess = false, Message = "User with this email already exists." };
        }

        var entity = Mapper.Map<UserAccountData>(user);
        entity.Id = 0;
        entity.Role = string.IsNullOrWhiteSpace(user.Role) ? "user" : user.Role.Trim().ToLowerInvariant();
        entity.CreatedAtUtc = DateTime.UtcNow;
        entity.Capsules = new List<TimeCapsuleData>();

        db.UserAccounts.Add(entity);
        db.SaveChanges();

        return new ResponceMsg { IsSuccess = true, Message = "User was successfully created." };
    }

    protected ResponceMsg ExecuteUserAccountUpdateAction(UserAccountDto user)
    {
        using var db = new AppDbContext();
        var data = db.UserAccounts.FirstOrDefault(x => x.Id == user.Id);
        if (data == null)
        {
            return new ResponceMsg { IsSuccess = false, Message = "User not found." };
        }

        var emailTaken = db.UserAccounts.Any(x => x.Id != user.Id && x.Email.Equals(user.Email));
        if (emailTaken)
        {
            return new ResponceMsg { IsSuccess = false, Message = "Email is already in use." };
        }

        data.Email = user.Email;
        data.DisplayName = user.DisplayName;
        data.Role = string.IsNullOrWhiteSpace(user.Role) ? data.Role : user.Role.Trim().ToLowerInvariant();
        data.Password = string.IsNullOrWhiteSpace(user.Password) ? data.Password : user.Password;
        data.NotifyEmailEnabled = user.NotifyEmailEnabled;
        data.NotifyPushEnabled = user.NotifyPushEnabled;
        data.LoginAlertsEnabled = user.LoginAlertsEnabled;

        db.SaveChanges();

        return new ResponceMsg { IsSuccess = true, Message = "User updated successfully." };
    }

    protected ResponceMsg ExecuteUserAccountDeleteAction(int id)
    {
        using var db = new AppDbContext();
        var data = db.UserAccounts.FirstOrDefault(x => x.Id == id);
        if (data == null)
        {
            return new ResponceMsg { IsSuccess = false, Message = "User not found." };
        }

        if (db.TimeCapsules.Any(c => c.OwnerUserId == id))
        {
            return new ResponceMsg
            {
                IsSuccess = false,
                Message = "Cannot delete user while capsules exist. Remove capsules first."
            };
        }

        db.UserAccounts.Remove(data);
        db.SaveChanges();

        return new ResponceMsg { IsSuccess = true, Message = "User deleted successfully." };
    }
}
