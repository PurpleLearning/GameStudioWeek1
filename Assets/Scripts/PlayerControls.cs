using System;
using TMPro;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    public TextMeshProUGUI gameOverText;
    private bool gameOver = false;
    
    public float movementSpeed = 5f;
    private int maxRescueSoldiers = 3;
    private int currentRescueSoldiers = 0;


    public TextMeshProUGUI soldierCountText;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector2.down * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * movementSpeed * Time.deltaTime);
        }
        
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Soldiers"))  
        {
            
            if (currentRescueSoldiers < maxRescueSoldiers)
            {
                currentRescueSoldiers++;   
                
                //Debug.Log(" Collected " + currentRescueSoldiers + "/" + maxRescueSoldiers);
                
                if (soldierCountText != null)
                {
                    soldierCountText.text = "Soldiers in Helicopter: " + currentRescueSoldiers + "/" + maxRescueSoldiers;
                }
                 
                Destroy(other.gameObject);
            }
            else
            {
                Debug.Log("Maximum resuce");
            }
        }
        
        if (other.CompareTag("Hospital"))
        {
            if (currentRescueSoldiers > 0)
            {
                currentRescueSoldiers = 0;
                soldierCountText.text = "Soldiers in Helicopter: " + currentRescueSoldiers + "/" + maxRescueSoldiers;
                
                Debug.Log("There is " + currentRescueSoldiers);
            }
        }

        if (other.CompareTag("Tree"))
        {
            movementSpeed = 0;
            gameOver = true;
            
            
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

