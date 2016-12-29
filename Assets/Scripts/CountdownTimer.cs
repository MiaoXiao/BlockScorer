using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField]
    private int StartingTimer = 0;
    public int CurrentTime { get; private set; }

    private float CountToSecond = 0f;
    private GameController GC;

    /// <summary>
    /// One second has gone by
    /// </summary>
    public delegate void SecondElapsed(int seconds);
    public SecondElapsed secondElapsed;

    /// <summary>
    /// Time has been altered
    /// </summary>
    public delegate void TimeSet(int seconds);
    public TimeSet timeSet;

    /// <summary>
    /// Timer hit 0
    /// </summary>
    public delegate void TimerDone();
    public TimerDone timerDone;


    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Use this for initialization
    private void Start ()
    {
        CurrentTime = StartingTimer;
        if (secondElapsed != null)
            secondElapsed(CurrentTime);
	}
	
	// Update is called once per frame
	private void Update ()
    {
        if (GC.GamePaused && CurrentTime > 0)
            return;

        CountToSecond += Time.deltaTime;
        if (CountToSecond >= 1f)
        {
            CurrentTime -= 1;
            CountToSecond = 0f;

            if (timerDone != null && CurrentTime == -1)
            {
                timerDone();
            }
            else if (secondElapsed != null)
            {
                secondElapsed(CurrentTime);
            }
        }
    }

    public void AddToCurrentTime(int seconds)
    {
        CurrentTime += seconds;
        if (timeSet != null)
            timeSet(CurrentTime);
    }

    public void ResetTime()
    {
        CurrentTime = StartingTimer;
        CountToSecond = 0f;
    }
}
