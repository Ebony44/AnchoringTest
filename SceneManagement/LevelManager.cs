using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("EndCondition")]
    public static bool BGameEnded = false;
    public static bool BGameWon = false;
    // for keep moving player and environment.
    public static bool BEndConditionMet = false;
    
    
    [SerializeField] private TextMeshProUGUI gameoverTextUGUI;

    public static bool BGamePaused = false;
    public GameObject PauseMenuUi;
    

    void Awake()
    {
        BGameEnded = false;
        BGameWon = false;
        BEndConditionMet = false;
    }
    void Update()
    {
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
        BGamePaused = false;
        SceneManager.LoadScene(2);
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator GameOverCondition()
    {
        if (BGameEnded && !BGameWon)
        {

            string[] randomText = { "Blasted", "Busted", "Wasted", "Burned", "Incinerated" };
            int randomValue = Random.Range(0, randomText.Length);
            
            gameoverTextUGUI.text = "You are " + randomText[randomValue] + " !" + " Game will restart in 3 seconds";
            gameoverTextUGUI.gameObject.SetActive(true);
            //yield return new WaitForSeconds(3f);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            BGameEnded = false;
            BEndConditionMet = true;

            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Level 1 Scene");

        }
        else if (BGameWon && !BGameEnded)
        {
            
            gameoverTextUGUI.text = "You find the way! Game will restart in 3 seconds";
            BGameWon = false;
            BGameEnded = false;
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Level 1 Scene");
        }

    }
}
