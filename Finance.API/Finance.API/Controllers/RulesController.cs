using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Finance.API.DTOs;
using Finance.API.Interfaces;
using Finance.API.Models;

namespace Finance.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly IRuleService _ruleService;

        public RulesController(IRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinancialRule>>> GetMyRules()
        {
            return Ok(await _ruleService.GetUserRulesAsync(GetUserId()));
        }

        [HttpPost]
        public async Task<ActionResult<FinancialRule>> CreateRule(FinancialRuleDto dto)
        {
            var rule = await _ruleService.CreateRuleAsync(dto, GetUserId());
            return Ok(rule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRule(int id)
        {
            var success = await _ruleService.DeleteRuleAsync(id, GetUserId());
            if (!success) return NotFound();
            return NoContent();
        }
    }
}