using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static bool BGameEnded = false;
    
    [SerializeField] private TextMeshProUGUI gameoverTextUGUI;

    public static bool BGamePaused = false;
    public GameObject PauseMenuUi;

    

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            BGameEnded = true;
        }
        GamePauseCondition();


        IEnumerator gameoverCoroutine = GameOverCondition();
        StartCoroutine(gameoverCoroutine);
    }

    private void GamePauseCondition()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (BGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        BGamePaused = false;
    }
    private void Pause()
    {
        PauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        BGamePaused = true;
    }

    public void LoadMenu()
    {
        //SceneManager
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator GameOverCondition()
    {
        if (BGameEnded)
        {

            string[] randomText = { "Blasted", "Busted", "Wasted", "Burned", "Incinerated" };
            int randomValue = Random.Range(0, randomText.Length);
            //gameoverText.text = "You are " + randomText[randomValue] + " !";
            //gameoverText.gameObject.SetActive(true);
            
            gameoverTextUGUI.text = "You are " + randomText[randomValue] + " !" + " Game will restart in 3 seconds";
            gameoverTextUGUI.gameObject.SetActive(true);
            //yield return new WaitForSeconds(3f);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            BGameEnded = false;
            
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Level 1 Scene");
            

        }
    }
}
