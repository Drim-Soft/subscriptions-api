using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionsApi.Data;
using SubscriptionsApi.Models;
using System.Threading.Tasks;

namespace SubscriptionsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            if (!ModelState.IsValid) return BadRequest("Datos inválidos.");

            _context.EnterpriseLeads.Add(lead);
            await _context.SaveChangesAsync();

            // ✅ mandar email al cliente
            await _email.SendLeadConfirmation(lead);

            return Ok(new { message = "Lead registrado", id = lead.Id });
        }
    }
}
