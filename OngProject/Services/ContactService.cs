using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;

namespace OngProject.Services
{
    public class ContactService : GenericService<Contact>, IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository) : base(contactRepository)
        {
            _contactRepository = contactRepository;
        }
    }
}
