using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlatformMovement : MonoBehaviour
{
    public bool IsRotating = false;
    public float RotateSpeed = 15f;

    public float TimeToRaise = 4f;

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
    /// Raise center platform
    /// </summary>
    public void StartRaisePlatform()
    {
        StartCoroutine("RaisePlatform");
    }

    IEnumerator RaisePlatform()
    {
      
        float time_elpased = 0f;
        Vector3 original_pos = transform.position;
        Vector3 new_pos = new Vector3(transform.position.x, 0, transform.position.z);
        while (time_elpased < TimeToRaise)
        {
            time_elpased += Time.deltaTime;
            transform.position = Vector3.Lerp(original_pos, new_pos, time_elpased / TimeToRaise);
            yield return null;
        }
    }

    /// <summary>
    /// Rotate main platform vertically
    /// </summary>
    private void RotatePlatform()
    {
        transform.Rotate(transform.up * Time.deltaTime * RotateSpeed);
    }
}
