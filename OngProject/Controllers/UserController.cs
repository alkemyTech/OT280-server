using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<Users> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IGenericRepository<Users> genericRepository, 
            IUnitOfWork unitOfWork,
            IUserService userService,
            IMapper mapper)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._mapper = mapper;
        }

        [Authorize(Roles="Admin")]
        [HttpGet]
        public async Task<IEnumerable<Users>> Get()
        {
            return await _genericRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetById(string id)
        {
            var user = await _genericRepository.GetById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var created = await _genericRepository.CreateAsync(users);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(string id, Users user)
        //{
        //    //if (id != user.Id)
        //    //    return BadRequest();

        //    await _genericRepository.Update(user);
        //    _unitOfWork.Commit();

        //    return NoContent();
        //}

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserUpdateDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(UserUpdateDTO userDTO)
        {
            var entity = await _userService.GetById(userDTO.Id);

            if (entity == null)
                return NotFound();

            var user = await _userService.UpdateUser(entity, userDTO);
            var userUpdate = _mapper.Map<UserUpdateDTO>(entity);
            _unitOfWork.Commit();

            return new OkObjectResult(userUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _genericRepository.GetById(id);

            if (user == null)
                return BadRequest();

            var deleted = _genericRepository.Delete_(user);

            if (deleted)
                _unitOfWork.Commit();

            return Ok(user);
        }
    }
}
