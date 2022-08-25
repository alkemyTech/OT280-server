using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;

namespace OngProject.Services.Interfaces
{
    public interface IActivityService : IGenericService<Activities>
    {
        void UpdateActivity(Activities activity, ActivityDTO activityDTO);
    }
}
