using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
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

        public UserController(IGenericRepository<Users> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IEnumerable<Users>> Get()
        {
            return await _genericRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<Users>> GetById(string id)
        {
            var user = await _genericRepository.GetById(id);
            if (user == null)
                return (IEnumerable<Users>)NotFound();
            return (IEnumerable<Users>)Ok(user);
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
        //public async Task<IActionResult> Update(int id, Users user)
        //{
        //    //if (id != user.Id)
        //    //    return BadRequest();

        //    await _genericRepository.Update(user);
        //    _unitOfWork.Commit();

        //    // Following up the REST standart on update we need to return NoContent
        //    return NoContent();
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _genericRepository.GetById(id);

            if (user == null)
                return BadRequest();

            await _genericRepository.DeleteAsync(user);
            _unitOfWork.Commit();

            return Ok(user);
        }
    }
}
