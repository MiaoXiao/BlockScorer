using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// Starting time in seconds
    /// </summary>
    public int StartingTime = 60;

    /// <summary>
    /// Whether pause screen or game over screen is shown or not
    /// </summary>
    public bool GamePaused
    {
        get
        {
            return PauseMenu.activeInHierarchy || GameOverMenu.activeInHierarchy;
        }
    }

    /// <summary>
    /// After a crate is scored or lost.
    /// </summary>
    public delegate void CrateEvaluated();
    public CrateEvaluated crateEvaluated;

    [SerializeField]
    private GameObject GeneralHoopParent;

    [SerializeField]
    private GameObject SuperHoopParent;

    [SerializeField]
    private GameObject MainPlatform;

    [SerializeField]
    private GameObject PauseMenu;

    [SerializeField]
    private GameObject GameOverMenu;

    private UIController UC;
    private CountdownTimer MainClock;

    private void Awake()
    {
        UC = GameObject.FindGameObjectWithTag("GUI").GetComponent<UIController>();
        MainClock = GetComponent<CountdownTimer>();
    }

    private void Start()
    {
        FreezeGameState(false);
        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
    }

    /// <summary>
    /// Restarts the scene
    /// </summary>
    public void RestartGame()
    {
        Debug.Log("restart game");
        SceneManager.LoadScene("MainScene");
        
    }

    /// <summary>
    /// End the game and show the game over screen.
    /// </summary>
    public void EndGame()
    {
        Debug.Log("game over");
        GameOverMenu.SetActive(true);
        FreezeGameState(true);
    }

    /// <summary>
    /// Quit the game and exit the application.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    /// <summary>
    /// Show the pause screen, or hide it
    /// </summary>
    public void PauseScreen()
    {
        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
        if (GamePaused)
        {
            FreezeGameState(true);
        }
        else
        {
            FreezeGameState(false);
        }
    }

    /// <summary>
    /// For pausing update functions and locking or unlocking the cursor
    /// </summary>
    private void FreezeGameState(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseScreen();
        }
    }

    /// <summary>
    /// Crate is lost, lose time
    /// </summary>
    public void LoseCrate(GameObject crate)
    {
        CratePickUp crate_info = crate.GetComponent<CratePickUp>();
        //Give time penalty and depool the object
        MainClock.AddToCurrentTime(-crate_info.TimeLost);
        crate.SetActive(false);

        UC.SetRecentScoreChange(0);
        UC.SetRecentTimeChange(-crate_info.TimeLost);

        if (crateEvaluated != null)
            crateEvaluated();
    }
}
