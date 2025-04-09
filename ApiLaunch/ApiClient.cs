using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    [SerializeField] private string apiUrl = "http://localhost:5000/api/collision";
    
    public IEnumerator ProcessCollision(CollisionRequest request, Action<CollisionResponse> callback)
    {
        string jsonData = JsonUtility.ToJson(request);
        
        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            
            Debug.Log($"Sending request to API: {jsonData}");
            
            yield return www.SendWebRequest();
            
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"API Error: {www.error}");
                callback(null);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.Log($"API Response: {jsonResponse}");
                
                CollisionResponse response = JsonUtility.FromJson<CollisionResponse>(jsonResponse);
                callback(response);
            }
        }
    }
}
