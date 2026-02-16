using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    public float movementSpeed = 5f;
    
    
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
}
