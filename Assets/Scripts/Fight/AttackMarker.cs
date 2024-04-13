using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMarker : MonoBehaviour
{
    [SerializeField]
    float minAnchor = 0;

    [SerializeField]
    float maxAnchor = 1;

    [SerializeField, Range(0, 1)]
    float internalProgress;

    public float Progress => Mathf.Lerp(minAnchor, maxAnchor, internalProgress);

    public float speed { get; set; } = 0;

    float direction = 1;

    private void Update()
    {
        var step = speed * direction * Time.deltaTime;
        internalProgress = Mathf.Clamp01(internalProgress += step);
        if (internalProgress == 0)
        {
            direction = 1;
        } else if (internalProgress == 1)
        {
            direction = -1;
        }

        RectTransform rt = GetComponent<RectTransform>();

        var x = Progress;
        rt.anchorMin = new Vector2(x, 0);
        rt.anchorMax = new Vector2(x, 1);
    }
}
