using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerControls : MonoBehaviour
{
    public bool gameOver = false;
    
    
    public float maxSpeed = 10f;
    public float acceleration = 15f;
    public float deceleration = 10f;
    public float airResistance = 5f;
    public float rotationSpeed = 5f;
    
    private Vector2 currentVelocity = Vector2.zero;
    private Vector2 targetDirection = Vector2.zero;
    
    private int maxRescueSoldiers = 3;
    private int currentRescueSoldiers = 0;
    private int totalrescueSoldiers = 0;
    
    public int requiredRescueSoldiersToWin = 8;
    
    public TextMeshProUGUI soldierCountText;
    public TextMeshProUGUI rescuedSoliders;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI maxText;
    public TextMeshProUGUI gameOverText;
    
    public AudioSource rescueSound;
    public AudioSource screamingMan;
    public CameraController CameraController;
    
    public String CurrentScene;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D helicopterRigidBody;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        helicopterRigidBody = GetComponent<Rigidbody2D>();
        
       
        if (helicopterRigidBody == null)
        {
            helicopterRigidBody = gameObject.AddComponent<Rigidbody2D>();
            helicopterRigidBody.gravityScale = 0f;  
            
            
            helicopterRigidBody.linearDamping = airResistance;  
            helicopterRigidBody.angularDamping = 0.5f;  
            
            helicopterRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;  
        }
        else
        {
            
            helicopterRigidBody.linearDamping = airResistance;
            helicopterRigidBody.angularDamping = 0.5f;
        }
    }
    
    
    void FixedUpdate()
    {
        if (gameOver)
        {
             
            helicopterRigidBody.linearVelocity *= 0.95f;
            return;
        }
        
         
        Vector2 inputDirection = Vector2.zero;
        
        if (Input.GetKey(KeyCode.UpArrow))
            inputDirection.y += 1;
        if (Input.GetKey(KeyCode.DownArrow))
            inputDirection.y -= 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            inputDirection.x -= 1;
        if (Input.GetKey(KeyCode.RightArrow))
            inputDirection.x += 1;
        
        
        if (inputDirection.magnitude > 1)
            inputDirection.Normalize();
        
       
        float currentMaxSpeed = maxSpeed;
        float currentAcceleration = acceleration;
        
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
        {
            currentMaxSpeed = maxSpeed * 1.5f; 
            currentAcceleration = acceleration * 1.2f;  
        }
        
        
        if (inputDirection.magnitude > 0)
        {
             
            currentVelocity = Vector2.MoveTowards(
                currentVelocity, 
                inputDirection * currentMaxSpeed, 
                currentAcceleration * Time.fixedDeltaTime
            );
            
            
            float targetRotation = -inputDirection.x * 15f; 
            transform.rotation = Quaternion.Lerp(
                transform.rotation, 
                Quaternion.Euler(0, 0, targetRotation), 
                rotationSpeed * Time.fixedDeltaTime
            );
            
           
            if (inputDirection.x != 0)
                spriteRenderer.flipX = inputDirection.x > 0;
        }
        else
        {
             
            currentVelocity = Vector2.MoveTowards(
                currentVelocity, 
                Vector2.zero, 
                deceleration * Time.fixedDeltaTime
            );
            
             
            transform.rotation = Quaternion.Lerp(
                transform.rotation, 
                Quaternion.identity, 
                rotationSpeed * Time.fixedDeltaTime
            );
        }
        
       
        helicopterRigidBody.linearVelocity = currentVelocity;
    }
    
  
    
    void Update()
    {
        // Handle UI and input that doesn't affect physics
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("80'sGame");
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Soldiers"))  
        {
           
            
            if (currentRescueSoldiers < maxRescueSoldiers)
            {
                currentRescueSoldiers++;   
                
                if (soldierCountText != null)
                {
                    soldierCountText.text = "Soldiers in Helicopter: " + currentRescueSoldiers + "/" + maxRescueSoldiers;
                }
                 
                Destroy(other.gameObject);
                
                int audioRandomiser = Random.Range(1, 5);
            
                if (audioRandomiser == 1)
                {
                    screamingMan.Play();
                }
                else 
                {
                    rescueSound.Play();
                }
                
            }
            else
            {
                maxText.gameObject.SetActive(true);
                maxText.text = "MAX SOLIDER CAPACITY!!!";
                maxText.color = Color.red;
                maxText.fontSize = 36;
            }
        }
        
        if (other.CompareTag("Hospital"))
        {
            if (currentRescueSoldiers > 0)
            {
                totalrescueSoldiers = currentRescueSoldiers + totalrescueSoldiers; 
                currentRescueSoldiers = 0;
                
                soldierCountText.text = "Soldiers in Helicopter: " + currentRescueSoldiers + "/" + maxRescueSoldiers;
                rescuedSoliders.text = "Rescued Soldiers: " + totalrescueSoldiers;
                maxText.gameObject.SetActive(false);
            }
            
            if (totalrescueSoldiers == requiredRescueSoldiersToWin)
            {
                gameOver = true;
                winText.text = "You Win";
                winText.color = Color.green;
                winText.fontSize = 100;
                winText.alignment = TextAlignmentOptions.Center;
            }
        }
        
        if (other.CompareTag("Tree"))
        {
            gameOver = true;
            
            if (CurrentScene != "80'sGame")
            {
                CameraController.cameraSpeed = 0;
            }
            
            if (gameOverText != null)
            {
                gameOverText.text = "GAME OVER";
                gameOverText.color = Color.red;
                gameOverText.fontSize = 100;
                gameOverText.alignment = TextAlignmentOptions.Center;
            }
        }
    }
}