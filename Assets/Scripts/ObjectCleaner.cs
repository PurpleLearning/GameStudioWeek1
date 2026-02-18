using UnityEngine;

public class ObjectCleaner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * 2 * Time.deltaTime);
    }
    
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tree") || other.CompareTag("Soldiers") || other.CompareTag("Hospital"))   
        {
            Destroy(other.gameObject);
            Debug.Log("DIEEE");
        }
    }
    
    
}
