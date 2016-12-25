using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public Text textTarget;

    public Text timeLeft;

    public Text Score;

    private GameController GC;

    private int currentTimeLeft;

    public delegate void NextSecond();
    public NextSecond nextSecond;

    public delegate void CrateScored();
    public CrateScored crateScored;

    /// <summary>
    /// Counts up to a full second, then resets
    /// </summary>
    private float CountToSecond = 0f;

    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Start()
	{
		DeactivateTargetName();
        currentTimeLeft = GC.StartingTime;
        timeLeft.text = currentTimeLeft.ToString();
        nextSecond += UpdateTimer;
        crateScored += UpdateScoreGui;
    }
	
	public void ActivateTargetName(string itemName)
	{
		textTarget.text = itemName + "\n[E]";
	}
	
	public void DeactivateTargetName()
	{
		textTarget.text = "";
	}

    /// <summary>
    /// Add points to the total score and update gui
    /// </summary>
    public void AddScore(int points)
    {
        GC.TotalScore += points;
    }

    /// <summary>
    /// Reset total score to 0 and update gui
    /// </summary>
    public void ResetScore()
    {
        GC.TotalScore = 0;
    }

    public void UpdateScoreGui()
    {
        Score.text = GC.TotalScore.ToString();
    }

    /// <summary>
    /// Decrease the timer by 1 sec
    /// </summary>
    private void UpdateTimer()
    {
        currentTimeLeft -= 1;
        if (currentTimeLeft >= 0)
        {
            timeLeft.text = currentTimeLeft.ToString();
        }
        else
        {
            GC.EndGame();
        }
    }

    private void Update()
    {
        if (GC.GamePaused)
            return;

        CountToSecond += Time.deltaTime;
        if (CountToSecond >= 1f)
        {
            nextSecond();
            CountToSecond = 0f;
        }
    }
}
