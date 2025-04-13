using Microsoft.AspNetCore.Mvc;
using WebCRM.Application.Interfaces;
using WebCRM.Domain.Entities;

namespace WebCRM.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadsController : ControllerBase
    {
        private readonly ICrmService _crmService;

        public LeadsController(ICrmService crmService)
        {
            _crmService = crmService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeadEntity>>> GetAllLeads()
        {
            var leads = await _crmService.GetAllLeadsAsync();
            return Ok(leads);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeadEntity>> GetLeadById(long id)
        {
            var lead = await _crmService.GetLeadByIdAsync(id);
            if (lead == null) return NotFound();
            return Ok(lead);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<LeadEntity>>> GetLeadsByStatus(LeadStatus status)
        {
            var leads = await _crmService.GetLeadsByStatusAsync(status);
            return Ok(leads);
        }

        [HttpPost]
        public async Task<ActionResult<LeadEntity>> CreateLead(LeadEntity lead)
        {
            var createdLead = await _crmService.CreateLeadAsync(lead);
            return CreatedAtAction(nameof(GetLeadById), new { id = createdLead.Id }, createdLead);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLead(long id, LeadEntity lead)
        {
            try
            {
                await _crmService.UpdateLeadAsync(lead);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLead(long id)
        {
            var result = await _crmService.DeleteLeadAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
} 