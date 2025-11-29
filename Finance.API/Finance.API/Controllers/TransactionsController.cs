using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Finance.API.DTOs;
using Finance.API.Interfaces;
using Finance.API.Models;

namespace Finance.API.Controllers
{
    // Защита: только авторизованные пользователи
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // Вспомогательный метод для получения Id текущего пользователя из JWT-токена
        private string GetUserId()
        {
            // ClaimTypes.NameIdentifier содержит Id пользователя (user.Id)
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? throw new UnauthorizedAccessException("User ID not found in token.");
        }


        // -------------------------------------------------------------------
        // 1. GET ALL: Получить все транзакции текущего пользователя
        // -------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            var userId = GetUserId();
            var transactions = await _transactionService.GetUserTransactionsAsync(userId);
            return Ok(transactions);
        }

        // -------------------------------------------------------------------
        // 2. GET BY ID: Получить транзакцию по Id
        // -------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var userId = GetUserId();
            // Сервис обрабатывает проверку владения транзакцией
            var transaction = await _transactionService.GetUserTransactionByIdAsync(id, userId);

            if (transaction == null)
            {
                // Возвращаем 404 (NotFound), чтобы не сообщать злоумышленнику о существовании чужих данных
                return NotFound();
            }

            return Ok(transaction);
        }

        // -------------------------------------------------------------------
        // 3. POST: Создать новую транзакцию
        // -------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction([FromBody] TransactionDto transactionDto)
        {
            var userId = GetUserId();
            var transaction = await _transactionService.CreateTransactionAsync(transactionDto, userId);

            // 201 CreatedAtAction
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
        }

        // -------------------------------------------------------------------
        // 4. PUT: Обновить существующую транзакцию
        // -------------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, [FromBody] TransactionDto transactionDto)
        {
            var userId = GetUserId();
            // Сервис обрабатывает обновление и проверку владения
            var success = await _transactionService.UpdateTransactionAsync(id, transactionDto, userId);

            if (!success)
            {
                return NotFound();
            }

            return NoContent(); // 204 Success
        }

        // -------------------------------------------------------------------
        // 5. DELETE: Удалить транзакцию
        // -------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var userId = GetUserId();
            // Сервис обрабатывает удаление и проверку владения
            var success = await _transactionService.DeleteTransactionAsync(id, userId);

            if (!success)
            {
                return NotFound();
            }

            return NoContent(); // 204 Success
        }
    }
}