using Microsoft.AspNetCore.Mvc;
using SubscriptionsApi.Services;
using SubscriptionsApi.Models;
using System.Threading.Tasks;

namespace SubscriptionsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterpriseLeadController : ControllerBase
    {
        private readonly EmailService _email;

        public EnterpriseLeadController(EmailService email)
        {
            _email = email;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EnterpriseLead lead)
        {
            if (!ModelState.IsValid) return BadRequest("Datos inválidos.");

            // SOLO enviamos correo
            await _email.SendLeadConfirmation(lead);

            return Ok(new { message = "Solicitud leída correctamente, correo enviado" });
        }
    }
}
