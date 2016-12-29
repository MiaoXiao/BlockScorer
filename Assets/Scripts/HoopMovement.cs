using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopMovement : MonoBehaviour
{
    [SerializeField]
    private bool IsRotatingHoops = false;
    [SerializeField]
    private float RotateSpeed = 15f;

    [SerializeField]
    private bool IsSpinningHoopsVertically = false;
    [SerializeField]
    private bool IsSpinningHoopsHorizontally = false;
    [SerializeField]
    private bool IsSpinningHoopsForward = false;
    [SerializeField]
    private float SpinSpeed = 15f;

    [SerializeField]
    private float TimeToRaiseHoops = 4f;

    private GameController GC;

    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void SetIsRotatingHoops(bool active)
    {
        IsRotatingHoops = active;
    }

    public void SetIsSpinningHoopsVertically(bool active)
    {
        IsSpinningHoopsVertically = active;
    }

    public void SetIsSpinningHoopsHorizontally(bool active)
    {
        IsSpinningHoopsHorizontally = active;
    }

    public void SetIsSpinningHoopsForward(bool active)
    {
        IsSpinningHoopsForward = active;
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
    /// Raise all hoops
    /// </summary>
    public void StartRaiseHoops(float height)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            StartCoroutine(RaiseHoops(height, i));
        }

    }

    IEnumerator RaiseHoops(float height, int child)
    {
        Transform child_transform = transform.GetChild(child).transform;
        float time_elpased = 0f;
        Vector3 original_pos = child_transform.position;
        Vector3 new_pos = new Vector3(child_transform.position.x, child_transform.position.y + height, child_transform.position.z);
        while (time_elpased < TimeToRaiseHoops)
        {
            time_elpased += Time.deltaTime;
            transform.GetChild(child).transform.position = Vector3.Lerp(original_pos, new_pos, time_elpased / TimeToRaiseHoops);
            yield return null;
        }
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
