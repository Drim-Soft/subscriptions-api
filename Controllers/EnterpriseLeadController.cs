using Microsoft.AspNetCore.Mvc;
using SubscriptionsApi.Data;
using SubscriptionsApi.Models;
using SubscriptionsApi.Services;
using System.Threading.Tasks;

namespace SubscriptionsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnterpriseLeadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _email;

        public EnterpriseLeadController(ApplicationDbContext context, EmailService email)
        {
            _context = context;
            _email = email;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EnterpriseLead lead)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.EnterpriseLeads.AddAsync(lead);
            await _context.SaveChangesAsync();

            await _email.SendLeadConfirmation(
                lead.Email,
                lead.FullName,
                lead.Company
            );

            return Ok(new
            {
                message = "Lead registrado y correo enviado correctamente"
            });
        }
    }
}
