using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public Text textTarget;

    public Text timeLeft;

    public Text Score;

    public Slider throwMeter;

    public Image throwFillImage;

    public Image cursorImage;

    private GameController GC;

    private int currentTimeLeft;

    public delegate void NextSecond();
    public NextSecond nextSecond;

    public delegate void CrateScored(int value);
    public CrateScored crateScored;

    public delegate void CrateLost(int value);
    public CrateLost crateLost;

    public delegate void ThrowCharge(float value);
    public ThrowCharge throwCharge;

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
        SetActiveThrowMeter(false);
		DeactivateTargetName();
        currentTimeLeft = GC.StartingTime;
        timeLeft.text = currentTimeLeft.ToString();
        nextSecond += ClockUpdate;
        crateScored += UpdateScoreGui;
        crateScored += AddTime;
        throwCharge += UpdateThrowCharge;
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

    public void ActivateTargetName(string itemName)
	{
		textTarget.text = itemName + "\n[E]";
	}
	
	public void DeactivateTargetName()
	{
		textTarget.text = "";
	}

    /// <summary>
    /// Reset total score to 0 and update gui
    /// </summary>
    public void ResetScore()
    {
        GC.TotalScore = 0;
        Score.text = "0";
    }

    /// <summary>
    /// Add value to the current score
    /// </summary>
    public void UpdateScoreGui(int value)
    {
        GC.TotalScore += value;
        Score.text = GC.TotalScore.ToString();
    }

    /// <summary>
    /// Decrease the timer by 1 sec
    /// </summary>
    private void ClockUpdate()
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

    /// <summary>
    /// Add seconds to the clock
    /// </summary>
    private void AddTime(int seconds)
    {
        currentTimeLeft += seconds;
        timeLeft.text = currentTimeLeft.ToString();
    }

    /// <summary>
    /// Update throw charge gui
    /// </summary>
    public void UpdateThrowCharge(float value)
    {
        if (value < 0 || value > 1)
            return;

        throwMeter.normalizedValue = value;

        throwFillImage.color = Color.Lerp(Color.yellow, Color.red, value);

    }

    /// <summary>
    /// Show or hide throw meter
    /// </summary>
    public void SetActiveThrowMeter(bool visible)
    {
        throwMeter.transform.gameObject.SetActive(visible);
    }

}
