using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BaseGame()
    {
        SceneManager.LoadScene("80'sGame");
    }

    public void GauntletGame()
    {
        SceneManager.LoadScene("Gauntlet");
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("StartingScene");
    }
    
}


