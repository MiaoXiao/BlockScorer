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
        CratePickUp crate = other.GetComponent<CratePickUp>();

        if (crate == null || other.isTrigger)
            return;

        UC.TotalScore += crate.PointsGained * scoreMultiplier;
        UC.TotalTimeLeft += crate.TimeGained * scoreMultiplier;
        other.gameObject.SetActive(false);
    }
}
