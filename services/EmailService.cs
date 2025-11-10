using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;


namespace SubscriptionsApi.Services
{
    public class EmailService
    {
        public async Task SendContactEmail(dynamic data)
        {
            var smtpClient = new SmtpClient(Environment.GetEnvironmentVariable("SMTP_HOST"))
            {
                Port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")),
                Credentials = new NetworkCredential(
                    Environment.GetEnvironmentVariable("SMTP_USER"),
                    Environment.GetEnvironmentVariable("SMTP_PASS")
                ),
                EnableSsl = true,
            };

            string body = $@"
                <h3>Hola {data.FullName} </h3>
                <p>Gracias por tu interés en Planifika para <strong>{data.Company}</strong>.</p>

                <p>
                ✅ Teléfono: {data.Phone}<br>
                ✅ Accesos solicitados: {data.AccessCount}
                </p>

                <p>Nos pondremos en contacto muy pronto.</p>
                <br>
                <em>Equipo Planifika</em>
            ";

            var mailMessage = new MailMessage(
                Environment.GetEnvironmentVariable("MAIL_FROM"),
                data.Email,
                "Solicitud recibida - Planifika",
                body
            );

            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.SubjectEncoding = Encoding.UTF8;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
