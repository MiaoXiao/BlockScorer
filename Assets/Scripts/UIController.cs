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

    public CountdownTimer MainClock;

    private GameController GC;

    /*
    /// <summary>
    /// Setting a new time
    /// </summary>
    public delegate void TimeChange(int new_time);
    public TimeChange timeChange;
    */

    /// <summary>
    /// Setting a new score
    /// </summary>
    public delegate void ScoreChange(int new_score);
    public ScoreChange scoreChange;

    /// <summary>
    /// Update charge meter
    /// </summary>
    public delegate void ChargeMeterChange(float charge_percentage);
    public ChargeMeterChange chargeMeterChange;

    /// <summary>
    /// Total score so far
    /// </summary>
    private int _TotalScore = 0;
    public int TotalScore
    {
        get
        {
            return _TotalScore;
        }
        set
        {
            _TotalScore = value;
            scoreChange(_TotalScore);
        }
    }

    /*
    /// <summary>
    /// Time left so far
    /// </summary>
    private int _TotalTimeLeft = 0;
    public int TotalTimeLeft
    {
        get
        {
            return _TotalTimeLeft;
        }
        set
        {
            _TotalTimeLeft = value;
            timeChange(_TotalTimeLeft);
        }
    }
    */

    /// <summary>
    /// Charge percentage so far
    /// </summary>
    private float _ChargeMeter = 0f;
    public float ChargeMeter
    {
        get
        {
            return _ChargeMeter;
        }
        set
        {
            _ChargeMeter = value;
            UpdateThrowCharge(_ChargeMeter);
        }
    }

    /// <summary>
    /// Counts up to a full second, then resets
    /// </summary>
    private float CountToSecond = 0f;

    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        scoreChange += UpdateScoreGui;

        MainClock.timeSet += UpdateTimeGui;
        MainClock.secondElapsed += UpdateTimeGui;
        MainClock.timerDone += GC.EndGame;

        chargeMeterChange += UpdateThrowCharge;
    }

    private void Start()
	{
        SetActiveThrowMeter(false);
		DeactivateTargetName();
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
    /// Update score gui
    /// </summary>
    public void UpdateScoreGui(int new_score)
    {
        Score.text = new_score.ToString();
    }

    /// <summary>
    /// Set time gui
    /// </summary>
    private void UpdateTimeGui(int seconds)
    {
        timeLeft.text = seconds.ToString();
    }

    /// <summary>
    /// Update throw charge gui
    /// </summary>
    public void UpdateThrowCharge(float charge_percentage)
    {
        if (charge_percentage < 0 || charge_percentage > 1)
            return;

        throwMeter.normalizedValue = charge_percentage;

        throwFillImage.color = Color.Lerp(Color.yellow, Color.red, charge_percentage);

    }

    /// <summary>
    /// Show or hide throw meter
    /// </summary>
    public void SetActiveThrowMeter(bool visible)
    {
        throwMeter.transform.gameObject.SetActive(visible);
    }

}
