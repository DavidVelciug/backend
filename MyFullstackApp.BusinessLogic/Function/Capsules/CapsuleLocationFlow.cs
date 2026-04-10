using AutoMapper;
using MyFullstackApp.BusinessLogic.Core.Capsules;
using MyFullstackApp.BusinessLogic.Interface;
using MyFullstackApp.Domains.Models.Base;
using MyFullstackApp.Domains.Models.Capsule;

namespace MyFullstackApp.BusinessLogic.Functions.Capsules;

public class CapsuleLocationFlow : CapsuleLocationAction, ICapsuleLocation
{
    public CapsuleLocationFlow(IMapper mapper) : base(mapper) { }

    public List<CapsuleLocationDto> GetAllCapsuleLocationsAction() => ExecuteGetAllCapsuleLocationsAction();

    public CapsuleLocationDto? GetCapsuleLocationByIdAction(int id) => GetCapsuleLocationDataByIdAction(id);

    public CapsuleLocationDto? GetCapsuleLocationByCapsuleIdAction(int capsuleId) =>
        GetCapsuleLocationDataByCapsuleIdAction(capsuleId);

    public ResponceMsg ResponceCapsuleLocationCreateAction(CapsuleLocationDto location) =>
        ExecuteCapsuleLocationCreateAction(location);

    public ResponceMsg ResponceCapsuleLocationUpdateAction(CapsuleLocationDto location) =>
        ExecuteCapsuleLocationUpdateAction(location);

    public ResponceMsg ResponceCapsuleLocationDeleteAction(int id) => ExecuteCapsuleLocationDeleteAction(id);
}