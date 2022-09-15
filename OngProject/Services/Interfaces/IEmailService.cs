namespace OngProject.Services.Interfaces
{
    public interface IEmailService
    {
        bool IsConfigured();

        void SendWelcome(string email);

        void SendSuccessContact(string email);
    }
}
