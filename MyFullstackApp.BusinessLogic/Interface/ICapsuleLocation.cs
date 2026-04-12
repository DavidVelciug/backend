using MyFullstackApp.Domains.Models.Base;
using MyFullstackApp.Domains.Models.Capsule;

namespace MyFullstackApp.BusinessLogic.Interface;

public interface ICapsuleLocation
{
    List<CapsuleLocationDto> GetAllCapsuleLocationsAction();
    CapsuleLocationDto? GetCapsuleLocationByIdAction(int id);
    CapsuleLocationDto? GetCapsuleLocationByCapsuleIdAction(int capsuleId);
    ResponceMsg ResponceCapsuleLocationCreateAction(CapsuleLocationDto location);
    ResponceMsg ResponceCapsuleLocationUpdateAction(CapsuleLocationDto location);
    ResponceMsg ResponceCapsuleLocationDeleteAction(int id);
}
