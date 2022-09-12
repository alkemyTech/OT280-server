using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OngProject.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OngProject.Services
{
    public class EmailService : IEmailService
    {
        private static readonly IConfiguration _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        private static string _apiKey = _config["SendGrid:ApiKey"];
        private static string _fromEmailAddress = _config["Email:From:Email"];
        private static string _fromName = _config["Email:From:Name"];

        // reemplazar este hardcoding cuando se haga el seed de Organization
        private string _facebok = "Somos_Más";
        private string _instagram = "SomosMás";
        private string _phone = "1160112988";
        private string _email = "somosfundacionmas@gmail.com";
        
        public void SendWelcome(string email)
        {
            var dynamicTemplateData = generateDynamicTemplateData
                (
                    _subject: "¡Bienvenid@ a Somos Más!",
                    _title: "¡Bienvedid@ a Somos Más!",
                    _body: "Tu registro ha sido satisfactorio. Gracias por sumarte a nuestra comunidad."
                );

            Execute(email, "TemplateId:Generic", dynamicTemplateData).Wait();
        }

        public void SendSuccessContact(string email)
        {
            throw new System.NotImplementedException();
        }

        private object generateDynamicTemplateData(string _subject, string _title, string _body)
        {
            return new
            {
                subject = _subject,
                title = _title,
                body = _body,
                facebook = _facebok,
                instagram = _instagram,
                phone = _phone,
                email = _email
            };
        }

        static async Task Execute(string email, string template, object dynamicTemplateData)
        {
            var templateId = _config[template];

            var client = new SendGridClient(_apiKey);

            var from = new EmailAddress(_fromEmailAddress, _fromName);
            var to = new EmailAddress(email);

            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicTemplateData);

            var response = await client.SendEmailAsync(msg);
        }

    }
}
