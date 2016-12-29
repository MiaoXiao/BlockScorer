using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private GameController GC;
    private UIController UC;

    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        UC = GameObject.FindGameObjectWithTag("GUI").GetComponent<UIController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            GC.GameOver();
        else
        {
            GC.LoseCrate(other.gameObject);
        }

    }
}
