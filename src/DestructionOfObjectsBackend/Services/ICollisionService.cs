using DestructionOfObjectsBackend.Models;

namespace DestructionOfObjectsBackend.Services
{
    public interface ICollisionService
    {
        CollisionResponse CalculateDestruction(CollisionRequest request);
    }
}
