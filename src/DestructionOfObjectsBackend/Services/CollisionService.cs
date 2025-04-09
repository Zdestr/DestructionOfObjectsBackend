using DestructionOfObjectsBackend.Models;

namespace DestructionOfObjectsBackend.Services
{
    public class CollisionService : ICollisionService
    {
        private readonly ILogger<CollisionService> _logger;
        private readonly Random _random = new Random();

        public CollisionService(ILogger<CollisionService> logger)
        {
            _logger = logger;
        }

        public CollisionResponse CalculateDestruction(CollisionRequest request)
        {
            // Определяем, может ли объект быть разрушен (например, на основе силы удара и типа объекта)
            bool canBeDestroyed = CanObjectBeDestroyed(request);
            
            if (!canBeDestroyed)
            {
                return new CollisionResponse { CanBeDestroyed = false };
            }

            // Рассчитываем силу разрушения на основе физических параметров
            float destructionForce = CalculateDestructionForce(request);
            
            // Рассчитываем параметры фрагментов
            var fragments = GenerateFragments(request, destructionForce);
            
            // Определяем время исчезновения и затухания
            float disappearTime = CalculateDisappearTime(request);
            float fadeTime = CalculateFadeTime(request);
            
            return new CollisionResponse
            {
                CanBeDestroyed = true,
                DestructionForce = destructionForce,
                Fragments = fragments,
                DisappearTime = disappearTime,
                FadeTime = fadeTime
            };
        }

        private bool CanObjectBeDestroyed(CollisionRequest request)
        {
            float thresholdForce = DetermineThresholdForce(request.ObjectType);
            return request.ImpactForce > thresholdForce;
        }

        private float DetermineThresholdForce(string objectType)
        {
            return objectType switch
            {
                "Glass" => 5.0f,
                "Wood" => 10.0f,
                "Stone" => 20.0f,
                "Metal" => 30.0f,
                _ => 15.0f,  // Значение по умолчанию
            };
        }

        private float CalculateDestructionForce(CollisionRequest request)
        {
            // Формула для расчета силы разрушения на основе параметров удара
            return request.ImpactForce * 1.5f;
        }

        private List<FragmentData> GenerateFragments(CollisionRequest request, float destructionForce)
        {
            int fragmentCount = CalculateFragmentCount(request, destructionForce);
            var fragments = new List<FragmentData>();
            
            for (int i = 0; i < fragmentCount; i++)
            {
                fragments.Add(GenerateFragmentData(request, destructionForce));
            }
            
            return fragments;
        }

        private int CalculateFragmentCount(CollisionRequest request, float destructionForce)
        {
            int baseCount = (int)(destructionForce / 5);
            
            float modifier = request.ObjectType switch
            {
                "Glass" => 2.0f,  // Стекло дает больше осколков
                "Wood" => 1.2f,
                "Stone" => 1.0f,
                "Metal" => 0.7f,  // Металл дает меньше осколков
                _ => 1.0f,
            };
            
            return Math.Max(3, (int)(baseCount * modifier));
        }

        private FragmentData GenerateFragmentData(CollisionRequest request, float destructionForce)
        {
            return new FragmentData
            {
                InitialVelocity = GenerateRandomVelocity(request.ImpactDirection, destructionForce),
                AngularVelocity = GenerateRandomAngularVelocity(),
                Mass = CalculateFragmentMass(request.ObjectMass)
            };
        }

        private Vector3 GenerateRandomVelocity(Vector3 impactDirection, float destructionForce)
        {
            float spreadFactor = 0.5f;  // Насколько сильно разлетаются фрагменты
            
            return new Vector3
            {
                X = impactDirection.X + (float)(_random.NextDouble() * 2 - 1) * spreadFactor * destructionForce,
                Y = impactDirection.Y + (float)(_random.NextDouble() * 2 - 1) * spreadFactor * destructionForce,
                Z = impactDirection.Z + (float)(_random.NextDouble() * 2 - 1) * spreadFactor * destructionForce
            };
        }

        private Vector3 GenerateRandomAngularVelocity()
        {
            return new Vector3
            {
                X = (float)(_random.NextDouble() * 10 - 5),
                Y = (float)(_random.NextDouble() * 10 - 5),
                Z = (float)(_random.NextDouble() * 10 - 5)
            };
        }

        private float CalculateFragmentMass(float objectMass)
        {
            return objectMass * (float)_random.NextDouble() * 0.2f;
        }

        private float CalculateDisappearTime(CollisionRequest request)
        {
            return request.ObjectType switch
            {
                "Glass" => 5.0f,
                "Wood" => 10.0f,
                "Stone" => 15.0f,
                "Metal" => 20.0f,
                _ => 10.0f,
            };
        }

        private float CalculateFadeTime(CollisionRequest request)
        {
            return CalculateDisappearTime(request) * 0.3f;
        }
    }
}
