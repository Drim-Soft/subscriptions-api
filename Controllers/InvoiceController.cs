using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionsApi.Data;
using SubscriptionsApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubscriptionsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===========================================
        // GET: api/invoice
        // ===========================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            var invoices = await _context.Invoices
                .Include(i => i.Currency)
                .Include(i => i.Subscription)
                .Include(i => i.SubscriptionStatus)
                .Include(i => i.PaymentMethod)
                .ToListAsync();

            return Ok(invoices);
        }

        // ===========================================
        // GET: api/invoice/{id}
        // ===========================================
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoiceById(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Currency)
                .Include(i => i.Subscription)
                .Include(i => i.SubscriptionStatus)
                .Include(i => i.PaymentMethod)
                .FirstOrDefaultAsync(i => i.IdInvoice == id);

            if (invoice == null)
                return NotFound(new { message = "Factura no encontrada" });

            return Ok(invoice);
        }

        // ===========================================
        // POST: api/invoice
        // ===========================================
        [HttpPost]
        public async Task<ActionResult<Invoice>> CreateInvoice([FromBody] Invoice invoice)
        {
            if (invoice == null)
                return BadRequest(new { message = "Datos de factura inválidos" });

            try
            {
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetInvoiceById),
                    new { id = invoice.IdInvoice },
                    invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la factura", error = ex.Message });
            }
        }
    }
}
