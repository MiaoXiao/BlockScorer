using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopMovement : MonoBehaviour
{
    public bool IsRotatingHoops = false;
    public float RotateSpeed = 15f;

    public bool IsSpinningHoopsVertically = false;
    public bool IsSpinningHoopsHorizontally = false;
    public bool IsSpinningHoopsForward = false;
    public float SpinSpeed = 15f;

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

        if (IsRotatingHoops)
            RotateHoops();

        if (IsSpinningHoopsVertically)
            SpinHoops(Vector3.up);

        if (IsSpinningHoopsHorizontally)
            SpinHoops(Vector3.right);

        if (IsSpinningHoopsForward)
            SpinHoops(Vector3.forward);

    }

    /// <summary>
    /// Rotates all children around itself
    /// </summary>
    private void RotateHoops()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).transform.RotateAround(transform.parent.transform.position, transform.parent.transform.up, Time.deltaTime * RotateSpeed);
        }
    }

    /// <summary>
    /// Spins each of the children
    /// </summary>
    private void SpinHoops(Vector3 direction)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            direction *= -1;
            transform.GetChild(i).transform.Rotate(transform.position + direction * Time.deltaTime * SpinSpeed);
        }
    }
}
