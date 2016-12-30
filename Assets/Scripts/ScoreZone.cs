using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    /// <summary>
    /// Score multiplier if scored in this zone
    /// </summary>
    [SerializeField]
    private int scoreMultiplier = 1;

    /// <summary>
    /// Time multiplier if scored in this zone
    /// </summary>
    [SerializeField]
    private int timeMultiplier = 1;


    private GameController GC;
    private GameObject Player;
    private UIController UC;

    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        Player = GameObject.FindGameObjectWithTag("Player");
        UC = GameObject.FindGameObjectWithTag("GUI").GetComponent<UIController>();
    }

    private void OnTriggerExit(Collider other)
    {
        CratePickUp crate_info = other.GetComponent<CratePickUp>();

        if (crate_info == null || other.isTrigger)
            return;

        //Debug.Log(crate_info.PointsGained);

        UC.TotalScore += crate_info.PointsGained * scoreMultiplier;
        UC.MainClock.AddToCurrentTime(crate_info.TimeGained * scoreMultiplier);
        UC.SetRecentScoreChange(crate_info.PointsGained * scoreMultiplier);
        UC.SetRecentTimeChange(crate_info.TimeGained * timeMultiplier);

        GetComponent<AudioSource>().Play();

        other.gameObject.SetActive(false);

        GC.crateEvaluated();
    }
}
