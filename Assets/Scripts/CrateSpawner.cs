using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrateSpawner : MonoBehaviour
{
    [SerializeField]
    private int StartStage = 0;

    [SerializeField]
    private int LastStage = 29;

    [SerializeField]
    private List<CrateDistribution> StageList = new List<CrateDistribution>();

    [SerializeField]
    private List<EnviornmentTrigger> EnviornmentTriggerList = new List<EnviornmentTrigger>();

    /// <summary>
    /// Origin point for spawning crates
    /// </summary>
    [SerializeField]
    private GameObject SpawnLocation;

    /// <summary>
    /// Area around the spawn location to spawn new crates
    /// </summary>
    [SerializeField]
    private Vector2 SpawnArea;

    /// <summary>
    /// Maximum initial force for crates that just spawned
    /// </summary>
    [SerializeField]
    private float MaximumInitForce = 0.5f;

    private int _CurrentStage = -1;
    public int CurrentStage
    {
        get
        {
            return _CurrentStage;
        }
        set
        {
            CheckGameOver(value);
            if (value == _CurrentStage || value < 0 || value >= StageList.Count)
                return;

            _CurrentStage = value;

            nextStage(_CurrentStage);
        }
    }

    /// <summary>
    /// Moving on to a new stage
    /// </summary>
    public delegate void NextStage(int stage);
    public NextStage nextStage;

    private int CurrNumbCrates = 0;

    private ObjectPooler[] AllObjPools;

    private GameController GC;
    private UIController UC;

    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        UC = GameObject.FindGameObjectWithTag("GUI").GetComponent<UIController>();

        AllObjPools = GetComponents<ObjectPooler>();
    }

    private void Start()
    {
        GC.crateEvaluated += CheckStageDone;
        nextStage += GoToNextStage;
        nextStage += CheckEnvionment;
        nextStage += UC.SetStageName;
        CurrentStage = StartStage - 1;
    }

    /// <summary>
    /// Go to the specified stage
    /// </summary>
    private void GoToNextStage(int stage)
    {
        //Debug.Log("going to stage " + stage);
        SpawnObjects(StageList[CurrentStage]);
    }

    private void CheckGameOver(int stage)
    {
        if (stage == LastStage)
        {
            GC.VictoryScreen();
        }
    }


    /// <summary>
    /// Decrease number of current crates or go to the next stage if there are 0 crates;
    /// </summary>
    private void CheckStageDone()
    {
        CurrNumbCrates--;
        if (CurrNumbCrates <= 0)
        {
            CurrentStage++;
        }
        //Debug.Log("numb crates is now " + CurrNumbCrates);
    }

    /// <summary>
    /// Enable eniornment hazards if necessary
    /// </summary>
    private void CheckEnvionment(int stage)
    {
        foreach(EnviornmentTrigger trigger in EnviornmentTriggerList)
        {
            if (trigger.StageWhereTriggerOccurs - 1 == stage)
            {
                trigger.SetEnviornmentEvent.Invoke();
                return;
            }
        }
    }

    /// <summary>
    /// Begin spawning new crates at random locations within the spawn area.
    /// CrateDistribution contains information about which crates should appear, and how many total crates there are
    /// </summary>
    private void SpawnObjects(CrateDistribution cp)
    {
        for (int i = 0; i < AllObjPools.Length; ++i)
        {
            for (int j = 0; j < cp.CrateTypeDistribution[i]; ++j)
            {
                float rand_pos_x = Random.Range(-SpawnArea.x, SpawnArea.x);
                float rand_pos_y = Random.Range(-SpawnArea.y, SpawnArea.y);
                Vector3 initial_position = new Vector3(rand_pos_x, SpawnLocation.transform.position.y, rand_pos_y);

                float rand_force_x = Random.Range(-MaximumInitForce, MaximumInitForce);
                float rand_force_y = Random.Range(-MaximumInitForce, MaximumInitForce);
                float rand_force_z = Random.Range(-MaximumInitForce, MaximumInitForce);
                Vector3 initital_force = new Vector3(rand_force_x, rand_force_y, rand_force_z);

                float rand_rot_x = Random.Range(0, 360);
                float rand_rot_y = Random.Range(0, 360);
                float rand_rot_z = Random.Range(0, 360);
                float rand_rot_w = Random.Range(0, 360);
                Quaternion initial_rotation = new Quaternion(rand_rot_x, rand_rot_y, rand_rot_z, rand_rot_w);

                GameObject crate = AllObjPools[i].RetrieveCopy();

                crate.transform.position = initial_position;
                crate.transform.rotation = initial_rotation;
                crate.GetComponent<Rigidbody>().AddForce(initital_force, ForceMode.Impulse);
            }
        }
        CurrNumbCrates = cp.Total;
    }
}
