using SubscriptionsApi.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SubscriptionsApi.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendLeadConfirmation(EnterpriseLead lead)
        {
            string body = $@"
            <div style='font-family:Arial;padding:20px;background:#fafafa'>
                <h2 style='color:#3A6EA5'>¡Gracias por tu solicitud, {lead.FullName}!</h2>
                <p>Hemos recibido tu interés en adquirir {lead.AccessCount} accesos para tu empresa: <b>{lead.Company}</b>.</p>
                <p>Uno de nuestros asesores se pondrá en contacto contigo al teléfono <b>{lead.Phone}</b></p>
                <br>
                <small>Equipo Planifika | DrimSoft</small>
            </div>";

            var message = new MailMessage();
            message.To.Add(lead.Email);
            message.From = new MailAddress(_config["EMAIL_FROM"]);
            message.Subject = "Confirmación de solicitud empresarial";
            message.IsBodyHtml = true;
            message.Body = body;

            var smtp = new SmtpClient(_config["SMTP_HOST"])
            {
                Port = int.Parse(_config["SMTP_PORT"]),
                Credentials = new NetworkCredential(
                    _config["SMTP_USER"],
                    _config["SMTP_PASS"]),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}
