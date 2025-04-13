using Microsoft.AspNetCore.Mvc;
using WebCRM.Application.Interfaces;
using WebCRM.Domain.Entities;

namespace WebCRM.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DealsController : ControllerBase
    {
        private readonly ICrmService _crmService;

        public DealsController(ICrmService crmService)
        {
            _crmService = crmService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DealEntity>>> GetAllDeals()
        {
            var deals = await _crmService.GetAllDealsAsync();
            return Ok(deals);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DealEntity>> GetDealById(long id)
        {
            var deal = await _crmService.GetDealByIdAsync(id);
            if (deal == null) return NotFound();
            return Ok(deal);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<DealEntity>>> GetDealsByStatus(DealStatus status)
        {
            var deals = await _crmService.GetDealsByStatusAsync(status);
            return Ok(deals);
        }

        [HttpPost]
        public async Task<ActionResult<DealEntity>> CreateDeal(DealEntity deal)
        {
            var createdDeal = await _crmService.CreateDealAsync(deal);
            return CreatedAtAction(nameof(GetDealById), new { id = createdDeal.Id }, createdDeal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeal(long id, DealEntity deal)
        {
            try
            {
                await _crmService.UpdateDealAsync(deal);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeal(long id)
        {
            var result = await _crmService.DeleteDealAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
} 