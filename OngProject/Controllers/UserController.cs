using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs.Category;
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

        /// <summary>
        /// Lista todos los usuarios existentes en el sistema. Rol: admin
        /// </summary>
        /// <returns>Lista de usuarios como Users[]</returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>
        /// <response code="403">Usuario no autorizado</response>
        /// <response code="404">Recurso no encontrado</response>        //[Authorize(Roles="Admin")]
        [HttpGet]
        public async Task<IEnumerable<Users>> Get()
        {
            return await _genericRepository.GetAllAsync();
        }

        /// <summary>
        /// Se obtiene los datos de un usuario por su id. Rol: admin
        /// </summary>
        /// <returns>Objeto de la clase Users</returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>
        /// <response code="403">Usuario no autorizado</response>
        /// <response code="404">Recurso no encontrado</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Users))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles="Admin")]
        [HttpGet("{id}")]
        public async Task<IEnumerable<Users>> GetById(string id)
        {
            var user = await _genericRepository.GetById(id);
            if (user == null)
                return (IEnumerable<Users>)NotFound();
            return (IEnumerable<Users>)Ok(user);
        }

        /// <summary>
        /// Creación de un ususario. Rol: admin
        /// </summary>
        /// <returns>Código HTTP con el resultado de la operacion</returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>
        /// <response code="403">Usuario no autorizado</response>
        /// <response code="404">Recurso no encontrado</response>
        //[Authorize(Roles="Admin")]
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

        /// <summary>
        /// Borra un usuario del sistema por su id. Rol: admin
        /// </summary>
        /// <returns>JSON del usuario borrado</returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>
        /// <response code="403">Usuario no autorizado</response>
        /// <response code="404">Recurso no encontrado</response>
        //[Authorize(Roles="Admin")]
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
