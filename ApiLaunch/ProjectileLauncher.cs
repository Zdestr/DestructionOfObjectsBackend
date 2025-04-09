using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float launchForce = 20f;
    [SerializeField] private Transform launchPoint;
    
    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
        
        if (launchPoint == null)
            launchPoint = transform;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LaunchProjectile();
        }
        
        // Тестовое разрушение объекта по нажатию клавиши T
        if (Input.GetKeyDown(KeyCode.T))
        {
            DestructibleObject[] destructibles = FindObjectsOfType<DestructibleObject>();
            foreach (var destructible in destructibles)
            {
                destructible.TestDestruction();
            }
        }
    }
    
    private void LaunchProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 direction = ray.direction;
            
            rb.AddForce(direction * launchForce, ForceMode.Impulse);
            
            // Уничтожаем снаряд через 5 секунд, чтобы не засорять сцену
            Destroy(projectile, 5f);
        }
    }
}
