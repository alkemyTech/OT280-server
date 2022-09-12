namespace OngProject.Services.Interfaces
{
    public interface IEmailService
    {
        void SendWelcome(string email);

        void SendSuccessContact(string email);
    }
}
