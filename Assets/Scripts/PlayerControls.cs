using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{

    public TextMeshProUGUI gameOverText;
     
    private bool gameOver = false;
    public float movementSpeed = 5f;
    private int maxRescueSoldiers = 3;
    private int currentRescueSoldiers = 0;
    private int totalrescueSoldiers = 0;

    public int requiredRescueSoldiersToWin = 8;
    
    
    public TextMeshProUGUI soldierCountText;
    public TextMeshProUGUI rescuedSoliders;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI maxText;

    public AudioSource rescueSound;
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

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("80'sGame");
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = 10;
        }
        else
        {
            
            movementSpeed = 5;
        }

        if (gameOver) movementSpeed = 0;

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Soldiers"))  
        {

            rescueSound.Play();
            
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
                
                maxText.text = "MAX SOLIDER CAPACITY!!!";
                maxText.color = Color.red;
                maxText.fontSize = 36;
 
                
                
                //Debug.Log("Maximum resuce");
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
                
                //Debug.Log("There is " + currentRescueSoldiers);
            }

            if (totalrescueSoldiers == requiredRescueSoldiersToWin)
            {
                movementSpeed = 0;
                winText.text = "You Win";
                winText.color = Color.green;
                winText.fontSize = 100;
                winText.alignment = TextAlignmentOptions.Center;
            }
            
        }

        if (other.CompareTag("Tree"))
        {
             
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



