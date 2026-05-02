using AutoMapper;
using MyFullstackApp.BusinessLogic.Core.Capsules;
using MyFullstackApp.BusinessLogic.Interface;
using MyFullstackApp.Domains.Models.Base;
using MyFullstackApp.Domains.Models.Capsule;

namespace MyFullstackApp.BusinessLogic.Functions.Capsules;

public class TimeCapsuleFlow : TimeCapsuleAction, ITimeCapsule
{
    public TimeCapsuleFlow(IMapper mapper) : base(mapper) { }

    public List<TimeCapsuleDto> GetAllTimeCapsulesAction() => ExecuteGetAllTimeCapsulesAction();

    public TimeCapsuleDto? GetTimeCapsuleByIdAction(int id) => GetTimeCapsuleDataByIdAction(id);
    public TimeCapsuleDto? GetTimeCapsuleByIdForUserAction(int id, int viewerUserId) =>
        GetTimeCapsuleDataByIdForUserAction(id, viewerUserId);

    public List<TimeCapsuleDto> GetTimeCapsulesByOwnerAction(int ownerUserId) =>
        ExecuteGetTimeCapsulesByOwnerAction(ownerUserId);
    public List<TimeCapsuleDto> GetOpenedCapsulesForUserAction(int userId) =>
        ExecuteGetOpenedCapsulesForUserAction(userId);

    public List<TimeCapsuleDto> GetPublicFeedAction() => ExecuteGetPublicFeedAction();

    public ResponceMsg ResponceTimeCapsuleCreateAction(TimeCapsuleDto capsule) =>
        ExecuteTimeCapsuleCreateAction(capsule);

    public ResponceMsg ResponceTimeCapsuleUpdateAction(TimeCapsuleDto capsule) =>
        ExecuteTimeCapsuleUpdateAction(capsule);

    public ResponceMsg ResponceTimeCapsuleDeleteAction(int id) => ExecuteTimeCapsuleDeleteAction(id);
    public ResponceMsg ResponceTimeCapsuleDeleteByOwnerAction(int id, int ownerUserId) =>
        ExecuteTimeCapsuleDeleteByOwnerAction(id, ownerUserId);
}
