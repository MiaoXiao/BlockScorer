using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlatformMovement : MonoBehaviour
{
    public bool IsRotating = false;
    public float RotateSpeed = 15f;

    private GameController GC;

    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Update()
    {
        if (GC.GamePaused)
            return;

        if (IsRotating)
            RotatePlatform();
    }

    /// <summary>
    /// Rotate main platform vertically
    /// </summary>
    private void RotatePlatform()
    {
        transform.Rotate(transform.up * Time.deltaTime * RotateSpeed);
    }
}
