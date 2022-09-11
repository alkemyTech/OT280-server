using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services;
using OngProject.Services.Interfaces;

namespace OngProject.Controllers
{
    //[Authorize(Roles = "admin")]
    [Route("news")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var _new = await _newService.GetById(id);

            if (_new == null)
                return BadRequest();

            _newService.DeleteNew(_new);
            _unitOfWork.Commit();

            NewDeleteResponseDTO _newDTO = 
                _mapper.Map<NewDeleteResponseDTO>(_new);

            return Ok(_newDTO);
        }
        
    }
}
