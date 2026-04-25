using AutoMapper;
using MyFullstackApp.BusinessLogic.Core.Users;
using MyFullstackApp.BusinessLogic.Interface;
using MyFullstackApp.Domains.Models.Base;
using MyFullstackApp.Domains.Models.User;

namespace MyFullstackApp.BusinessLogic.Functions.Users;

public class UserFlow : UserAction, IUserAccount
{
    public UserFlow(IMapper mapper) : base(mapper) { }

    public List<UserAccountDto> GetAllUserAccountsAction() => ExecuteGetAllUserAccountsAction();

    public UserAccountDto? GetUserAccountByIdAction(int id) => GetUserAccountDataByIdAction(id);

    public UserLoginResultDto LoginUserAction(UserLoginRequestDto request) => ExecuteLoginUserAction(request);

    public ResponceMsg ResponceUserAccountCreateAction(UserAccountDto user) => ExecuteUserAccountCreateAction(user);

    public ResponceMsg ResponceUserAccountUpdateAction(UserAccountDto user) => ExecuteUserAccountUpdateAction(user);

    public ResponceMsg ResponceUserAccountDeleteAction(int id) => ExecuteUserAccountDeleteAction(id);
}
