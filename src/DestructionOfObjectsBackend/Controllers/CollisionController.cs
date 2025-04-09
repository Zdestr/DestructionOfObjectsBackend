using Microsoft.AspNetCore.Mvc;
using DestructionOfObjectsBackend.Models;
using DestructionOfObjectsBackend.Services;

namespace DestructionOfObjectsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollisionController : ControllerBase
    {
        private readonly ICollisionService _collisionService;
        private readonly ILogger<CollisionController> _logger;

        public CollisionController(ICollisionService collisionService, ILogger<CollisionController> logger)
        {
            _collisionService = collisionService;
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        public ActionResult<CollisionResponse> ProcessCollision(CollisionRequest request)
        {
            _logger.LogInformation($"Получен запрос на обработку столкновения: ObjectId={request.ObjectId}, Force={request.ImpactForce}");

            try
            {
                var response = _collisionService.CalculateDestruction(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке столкновения");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return Ok("Collision API работает!");
        }
    }
}
