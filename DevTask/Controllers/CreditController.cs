using Data;
using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreditController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var credits = await _unitOfWork.Credits.GetAllWithRelationsAsync(Enums.CreditRelations.Invoices);

            return Ok(credits);
        }

        [HttpGet("getsummary")]
        public async Task<IActionResult> GetSummary()
        {
            CreditSummary summary = new CreditSummary();

            summary = await _unitOfWork.Credits.GetCreditsSummaryAsync();

            return Ok(summary);
        }
    }
}
