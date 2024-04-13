using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class TakeWoundUI : MonoBehaviour
{
    [SerializeField]
    Image WoundImage;

    [SerializeField]
    AnimationCurve EaseIn;

    [SerializeField, Range(0, 1)]
    float EaseInDuration = 0.3f;

    private void OnEnable()
    {
        Enemy.OnAttackPlayer += Enemy_OnAttackPlayer;
    }

    private void OnDisable()
    {
        Enemy.OnAttackPlayer -= Enemy_OnAttackPlayer;
    }

    float easeStart;
    
    private void Enemy_OnAttackPlayer(int damage)
    {
        transform.ShowAllChildren();
        WoundImage.enabled = true;
        easeStart = Time.timeSinceLevelLoad;
    }

    private void Start()
    {
        transform.HideAllChildren();
    }

    private void Update()
    {
        if (!WoundImage.enabled) { return; }

        float progress = Mathf.Clamp01((Time.timeSinceLevelLoad - easeStart) / EaseInDuration);

        if (progress == 1)
        {
            WoundImage.enabled = false;
            transform.HideAllChildren();
        } else
        {
            var scaledProgress = EaseIn.Evaluate(progress);
            WoundImage.transform.localScale = new Vector3(scaledProgress, scaledProgress, 1);
        }
    }
}
