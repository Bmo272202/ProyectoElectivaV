using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using ProyectoElectivaV.DTOs.Usuarios;

namespace ProyectoElectivaV.Service.Email
{
    public class CorreoService : ICorreoService
    {
        private readonly IConfiguration _configuration;

        public CorreoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void EnviarEmail(CorreoDTO correo)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(correo.Para));
            email.Subject = correo.Asunto;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = correo.Contenido,
            };

            using var smtp = new SmtpClient();
            smtp.Connect(
                _configuration.GetSection("Email:Host").Value,
                Convert.ToInt32(_configuration.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );

            smtp.Authenticate(_configuration.GetSection("Email:UserName").Value, _configuration.GetSection("Email:PassWord").Value);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
