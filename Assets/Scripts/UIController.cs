using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public Text TextTarget;

    public Text TimeLeft;

    public Text RecentTimeChange;

    public Text Score;

    public Text RecentScoreChange;

    public Slider ThrowMeter;

    public Image ThrowFillImage;

    public Image CursorImage;

    [SerializeField]
    private float FadeOutLength = 2f;

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
        //scoreChange += SetRecentScoreChange;

        MainClock.timeSet += UpdateTimeGui;
        //MainClock.timeSet += SetRecentTimeChange;

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
		TextTarget.text = itemName + "\n[E]";
	}
	
	public void DeactivateTargetName()
	{
		TextTarget.text = "";
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
        TimeLeft.text = seconds.ToString();
    }

    /// <summary>
    /// Update throw charge gui
    /// </summary>
    public void UpdateThrowCharge(float charge_percentage)
    {
        if (charge_percentage < 0 || charge_percentage > 1)
            return;

        ThrowMeter.normalizedValue = charge_percentage;

        ThrowFillImage.color = Color.Lerp(Color.yellow, Color.red, charge_percentage);

    }

    /// <summary>
    /// Show or hide throw meter
    /// </summary>
    public void SetActiveThrowMeter(bool visible)
    {
        ThrowMeter.transform.gameObject.SetActive(visible);
    }

    /// <summary>
    /// Show recent score or time changes in the gui
    /// </summary>
    public void SetRecentScoreChange(int score_change)
    {
        if (ScoreChangeCoroutine != null)
            StopCoroutine(ScoreChangeCoroutine);

        //Remove text from gui, then set alpha back to 1
        RecentScoreChange.text = "";
        RecentScoreChange.color = new Color(RecentScoreChange.color.r, RecentScoreChange.color.g, RecentScoreChange.color.b, 1f);

        ScoreChangeCoroutine = StartCoroutine(FadeOutGui(RecentScoreChange, score_change));
    }

    private Coroutine ScoreChangeCoroutine;
    private Coroutine TimeChangeCoroutine;
    /// <summary>
    /// Show recent score or time changes in the gui
    /// </summary>
    public void SetRecentTimeChange(int time_change)
    {
        if (TimeChangeCoroutine != null)
            StopCoroutine(TimeChangeCoroutine);

        //Remove text from gui, then set alpha back to 1
        RecentTimeChange.text = "";
        RecentTimeChange.color = new Color(RecentTimeChange.color.r, RecentTimeChange.color.g, RecentTimeChange.color.b, 1f);

        TimeChangeCoroutine = StartCoroutine(FadeOutGui(RecentTimeChange, time_change));
    }

    IEnumerator FadeOutGui(Text gui_to_fadeout, int value)
    {
        string text_to_display;
        if (value >= 0)
            text_to_display = "+" + value.ToString();
        else
            text_to_display = value.ToString();

        gui_to_fadeout.text = text_to_display;

        yield return new WaitForSeconds(2f);

        Color faded_color = new Color(gui_to_fadeout.color.r, gui_to_fadeout.color.g, gui_to_fadeout.color.b, 0f);

        float fadeout_time = 0f;
        while(fadeout_time < FadeOutLength)
        {  
            fadeout_time += Time.deltaTime;
            gui_to_fadeout.color = Color.Lerp(gui_to_fadeout.color, faded_color, fadeout_time / FadeOutLength);
            yield return null;
        }

        if (gui_to_fadeout == RecentScoreChange)
            ScoreChangeCoroutine = null;
        else
            TimeChangeCoroutine = null;
    }

}
