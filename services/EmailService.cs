using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SubscriptionsApi.Services
{
    public class EmailService
    {
        private readonly HttpClient _http;

        public EmailService(HttpClient http)
        {
            _http = http;
        }

        public async Task SendLeadConfirmation(string toEmail, string name, string company)
        {
            var payload = new
            {
                from = "onboarding@resend.dev",
                to = new[] { toEmail },
                subject = "Solicitud recibida - Planifika",
                html = $"<p>Hola <strong>{name}</strong>,</p><p>Recibimos tu solicitud para <strong>{company}</strong>. ¡Gracias!</p>"
            };

            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _http.PostAsync("/emails", content);

            // lanza excepción si no fue 2xx
            resp.EnsureSuccessStatusCode();
        }

        // método genérico para enviar facturas u otros templates
        public async Task SendHtmlEmail(string toEmail, string subject, string htmlBody)
        {
            var payload = new
            {
                from = "onboarding@resend.dev",
                to = new[] { toEmail },
                subject = subject,
                html = htmlBody
            };

            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _http.PostAsync("/emails", content);
            resp.EnsureSuccessStatusCode();
        }
    }
}
