using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private string objectType = "Glass"; // Glass, Wood, Stone, Metal
    [SerializeField] private GameObject fragmentPrefab;
    [SerializeField] private int fragmentsCountPreview = 8; // для тестирования
    [SerializeField] private float minImpactForce = 5f;
    
    private ApiClient apiClient;
    private Rigidbody rb;
    
    private void Start()
    {
        apiClient = FindObjectOfType<ApiClient>();
        rb = GetComponent<Rigidbody>();
        
        if (apiClient == null)
        {
            Debug.LogError("ApiClient не найден в сцене! Добавьте компонент ApiClient на объект в сцене.");
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Расчет силы удара
        float impactForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        
        Debug.Log($"Collision detected with force: {impactForce}");
        
        if (impactForce < minImpactForce)
        {
            Debug.Log("Impact force too low for destruction");
            return;
        }
        
        // Получаем точку удара и направление
        ContactPoint contact = collision.GetContact(0);
        Vector3 impactPoint = contact.point;
        Vector3 impactDirection = contact.normal;
        
        // Создаем запрос к API
        CollisionRequest request = new CollisionRequest
        {
            objectId = gameObject.name,
            impactForce = impactForce,
            impactPoint = new Vector3Data(impactPoint),
            impactDirection = new Vector3Data(impactDirection),
            objectType = objectType,
            objectMass = rb.mass,
            objectDimensions = new Vector3Data(transform.localScale)
        };
        
        // Отправляем запрос к API
        StartCoroutine(apiClient.ProcessCollision(request, OnCollisionResponseReceived));
    }
    
    private void OnCollisionResponseReceived(CollisionResponse response)
    {
        if (response == null)
        {
            Debug.LogError("Не удалось получить ответ от API");
            return;
        }
        
        if (!response.canBeDestroyed)
        {
            Debug.Log("Объект не может быть разрушен согласно API");
            return;
        }
        
        // Создаем фрагменты
        CreateFragments(response);
        
        // Скрываем оригинальный объект
        gameObject.SetActive(false);
        
        // Можно также уничтожить объект позже
        Destroy(gameObject, response.disappearTime + 1f);
    }
    
    private void CreateFragments(CollisionResponse response)
    {
        Debug.Log($"Создание {response.fragments.Count} фрагментов");
        
        foreach (var fragment in response.fragments)
        {
            // Создаем фрагмент
            GameObject fragmentObj = Instantiate(fragmentPrefab, transform.position, transform.rotation);
            
            // Настраиваем размер и цвет (например, случайный размер)
            float randomScale = Random.Range(0.2f, 0.5f);
            fragmentObj.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            
            // Применяем физические параметры от API
            Rigidbody fragmentRb = fragmentObj.GetComponent<Rigidbody>();
            if (fragmentRb != null)
            {
                fragmentRb.mass = fragment.mass;
                fragmentRb.velocity = fragment.initialVelocity.ToUnityVector();
                fragmentRb.angularVelocity = fragment.angularVelocity.ToUnityVector();
            }
            
            // Настраиваем исчезновение через время
            StartCoroutine(DestroyAfterTime(fragmentObj, response.disappearTime, response.fadeTime));
        }
    }
    
    private IEnumerator DestroyAfterTime(GameObject obj, float delay, float fadeTime)
    {
        // Ждем указанное время
        yield return new WaitForSeconds(delay - fadeTime);
        
        // Затем постепенно уменьшаем непрозрачность (если есть Renderer)
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && fadeTime > 0)
        {
            Material material = renderer.material;
            Color startColor = material.color;
            
            float elapsed = 0;
            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
                material.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
        }
        
        // Уничтожаем объект
        Destroy(obj);
    }
    
    // Для тестирования без API
    public void TestDestruction()
    {
        List<FragmentData> testFragments = new List<FragmentData>();
        
        for (int i = 0; i < fragmentsCountPreview; i++)
        {
            testFragments.Add(new FragmentData
            {
                initialVelocity = new Vector3Data(Random.insideUnitSphere * 5),
                angularVelocity = new Vector3Data(Random.insideUnitSphere * 5),
                mass = rb.mass / fragmentsCountPreview
            });
        }
        
        CollisionResponse testResponse = new CollisionResponse
        {
            canBeDestroyed = true,
            destructionForce = 15f,
            fragments = testFragments,
            disappearTime = 5f,
            fadeTime = 1.5f
        };
        
        OnCollisionResponseReceived(testResponse);
    }
}
