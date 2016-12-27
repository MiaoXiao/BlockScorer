using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopMovement : MonoBehaviour
{
    public float hoopRotateSpeed = 15f;

    private GameController GC;
    private GameObject Player;
    private UIController UC;

    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        Player = GameObject.FindGameObjectWithTag("Player");
        UC = GameObject.FindGameObjectWithTag("GUI").GetComponent<UIController>();
    }

    private void Update()
    {
        if (GC.GamePaused)
            return;

        RotateHoops();
    }

    /// <summary>
    /// Rotates all children around itself
    /// </summary>
    private void RotateHoops()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).transform.RotateAround(transform.parent.transform.position, transform.parent.transform.up, Time.deltaTime * hoopRotateSpeed);
        }
    }
}
