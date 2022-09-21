using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OngProject.Controllers;
using OngProject.Core.Models;
using OngProject.DataAccess;
using OngProject.Repositories;
using OngProject.Repositories.Interfaces;
using OngProject.Services;
using OngProject.Services.Interfaces;

namespace OngProject.UnitTests
{
    public class ContactControllerTest
    {
        private readonly ContactController _controller;
        private readonly IContactService _contactService;

        public ContactControllerTest(IContactService contactService)
        {
            _contactService = contactService;
            _controller = new ContactController(_contactService);
        }

        [Fact]
        public void GetAllContactOK()
        {
            var result = _controller.GetsAll();

            Assert.IsType<OkObjectResult>(result);

        }
    }


}