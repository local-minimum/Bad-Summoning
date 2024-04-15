using UnityEngine;

public delegate void PentagramCompleteEvent();


public class Pentagram : MonoBehaviour
{
    public static event PentagramCompleteEvent OnComplete;

    [SerializeField]
    Texture[] Stages;

    [SerializeField, Range(0, 20)]
    int StartStep = 1;

    int step;

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += Enemy_OnEnemyDeath;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= Enemy_OnEnemyDeath;
    }

    private void Start()
    {
        step = StartStep;
        Sync();
    }

    private void Enemy_OnEnemyDeath(Enemy enemy)
    {
        step++;
        Sync();
    }

    void Sync()
    {
        GetComponentInChildren<Renderer>().material.mainTexture = Stages.GetNthOrLast(step);

        if (step >= Stages.Length - 1)
        {
            Debug.Log("No clock!");
            OnComplete?.Invoke();
        }
    }
}
