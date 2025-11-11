using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionsApi.Data;
using SubscriptionsApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubscriptionsApi.Controllers
{
    [Route("api/v1/invoices")]
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
                return BadRequest(new { message = "Datos de factura inv�lidos" });

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

        // ===========================================
        // PATCH: api/invoice/{id}
        // ===========================================
        [HttpPatch("{id}")]
        public async Task<ActionResult<Invoice>> UpdateInvoice(int id, [FromBody] Invoice invoiceUpdate)
        {
            if (invoiceUpdate == null)
                return BadRequest(new { message = "Datos de factura inválidos" });

            try
            {
                // Cargar la factura con relaciones desde el principio para evitar consulta adicional
                var invoice = await _context.Invoices
                    .Include(i => i.Currency)
                    .Include(i => i.Subscription)
                    .Include(i => i.SubscriptionStatus)
                    .Include(i => i.PaymentMethod)
                    .FirstOrDefaultAsync(i => i.IdInvoice == id);

                if (invoice == null)
                    return NotFound(new { message = "Factura no encontrada" });

                // Guardar valores originales para comparar después
                var originalCurrencyId = invoice.IdCurrency;
                var originalSubscriptionId = invoice.IdSubscription;
                var originalStatusId = invoice.IdSubscriptionStatus;
                var originalPaymentMethodId = invoice.IdPaymentMethod;

                // Actualizar solo los campos que se proporcionen
                if (invoiceUpdate.IdSubscription != 0)
                    invoice.IdSubscription = invoiceUpdate.IdSubscription;

                if (invoiceUpdate.IdSubscriptionStatus != 0)
                    invoice.IdSubscriptionStatus = invoiceUpdate.IdSubscriptionStatus;

                if (invoiceUpdate.IdPaymentMethod != 0)
                    invoice.IdPaymentMethod = invoiceUpdate.IdPaymentMethod;

                if (invoiceUpdate.IdCurrency != 0)
                    invoice.IdCurrency = invoiceUpdate.IdCurrency;

                if (invoiceUpdate.IdOrganization != 0)
                    invoice.IdOrganization = invoiceUpdate.IdOrganization;

                if (invoiceUpdate.Total != 0)
                    invoice.Total = invoiceUpdate.Total;

                if (invoiceUpdate.StartDate != default(DateTime))
                    invoice.StartDate = invoiceUpdate.StartDate;

                if (invoiceUpdate.EndDate != default(DateTime))
                    invoice.EndDate = invoiceUpdate.EndDate;

                await _context.SaveChangesAsync();

                // Recargar solo las relaciones que cambiaron
                if (invoiceUpdate.IdCurrency != 0 && invoiceUpdate.IdCurrency != originalCurrencyId)
                    await _context.Entry(invoice).Reference(i => i.Currency).LoadAsync();
                
                if (invoiceUpdate.IdSubscription != 0 && invoiceUpdate.IdSubscription != originalSubscriptionId)
                    await _context.Entry(invoice).Reference(i => i.Subscription).LoadAsync();
                
                if (invoiceUpdate.IdSubscriptionStatus != 0 && invoiceUpdate.IdSubscriptionStatus != originalStatusId)
                    await _context.Entry(invoice).Reference(i => i.SubscriptionStatus).LoadAsync();
                
                if (invoiceUpdate.IdPaymentMethod != 0 && invoiceUpdate.IdPaymentMethod != originalPaymentMethodId)
                    await _context.Entry(invoice).Reference(i => i.PaymentMethod).LoadAsync();

                return Ok(invoice);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { 
                    message = "Error de integridad referencial. Verifica que todos los IDs existan.",
                    error = ex.InnerException?.Message 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la factura", error = ex.Message });
            }
        }
    }
}
