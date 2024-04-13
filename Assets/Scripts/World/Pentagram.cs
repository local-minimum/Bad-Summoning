using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagram : MonoBehaviour
{
    [SerializeField]
    Texture[] Stages;

    int step;

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += Enemy_OnEnemyDeath;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= Enemy_OnEnemyDeath;
    }

    private void Enemy_OnEnemyDeath(Enemy enemy)
    {
        step++;
        GetComponentInChildren<Renderer>().material.mainTexture = Stages.GetNthOrLast(step);

        if (step >= Stages.Length - 1) {
            Debug.Log("No clock!");
        }
    }
}
