using UnityEngine;
using TMPro;
public class ObjectCleaner : MonoBehaviour

{

    private int cleanerMover = 3;
    
    private int soldierCount = 0;
    public TextMeshProUGUI gameOverText;
    public PlayerControls PlayerControls;
    public CameraController Cameracontroller;
 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * cleanerMover * Time.deltaTime);

        if (soldierCount == 3)
        {

            
            PlayerControls.gameOver = true;
            Cameracontroller.cameraSpeed = 0;
            cleanerMover = 0;
    
            
            
        }
        
        
    }
    
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tree") || other.CompareTag("Hospital"))   
        {
            Destroy(other.gameObject);
            Debug.Log("DIEEE");
        }

        if (other.CompareTag("Soldiers"))
        {
            Destroy(other.gameObject);
            soldierCount++;
        }
        
    }
    
    
}
