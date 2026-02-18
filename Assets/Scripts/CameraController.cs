using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed= 2f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector2.right * cameraSpeed * Time.deltaTime);
        
        
    }
}
