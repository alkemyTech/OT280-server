using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;

namespace OngProject.Services
{
    public class ActivityService: GenericService<Activities>, IActivityService
    {
        private IActivityRepository _activityRepository;
        public ActivityService(IActivityRepository activityRepositor) : base(activityRepositor)
        {
            this._activityRepository = activityRepositor;
        }

        public async void UpdateActivity(Activities activity, ActivityDTO activityDTO)
        {
            activity.Name = activityDTO.Name;
            activity.Content = activityDTO.Content;

            await _activityRepository.Update(activity);
           
        }
    }
}
