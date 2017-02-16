using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombBehavior : MonoBehaviour
{
    [SerializeField]
    private float ExplosionForce = 5f;

    [SerializeField]
    private float ExplosionRadius = 5f;

    [SerializeField]
    private float UpwardsForce = 5f;

    public Text BombTimer;

    private GameController GC;
    private GameObject Player;
    private UIController UC;

    private CountdownTimer BombClock;

    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        Player = GameObject.FindGameObjectWithTag("Player");
        UC = GameObject.FindGameObjectWithTag("GUI").GetComponent<UIController>();

        BombClock = GetComponent<CountdownTimer>();

        BombClock.timerDone += Explode;
        BombClock.timeSet += UpdateBombTimer;
    }

    private void Update()
    {
        if (GC.GamePaused)
            return;

        BombTimer.gameObject.transform.position = transform.position + Vector3.up * (transform.localScale.y) * 1.5f;
        //BombTimer.gameObject.transform.LookAt(Player.transform.position, Vector3.up);
        BombTimer.gameObject.transform.rotation = Quaternion.LookRotation(BombTimer.gameObject.transform.position - Player.transform.position);
    }

    /// <summary>
    /// The bomb explodes, pushing away all other rigidbodies and deactivating itself
    /// </summary>
    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, UpwardsForce);

        }
        GC.GetComponents<AudioSource>()[2].Play();

        GC.LoseCrate(gameObject);
    }

    /// <summary>
    /// Update bomb timer
    /// </summary>
    public void UpdateBombTimer(int seconds)
    {
        BombTimer.text = seconds.ToString();
    }
    
    /// <summary>
    /// When this bomb is removed from play, reset its timer
    /// </summary>
    private void OnDisable()
    {
        BombClock.ResetTime();
    }
}
