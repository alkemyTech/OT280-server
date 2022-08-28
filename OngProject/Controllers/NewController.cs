using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewController : ControllerBase
    {
        private readonly INewService _newService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NewController(INewService newService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._newService = newService;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
    }
}
