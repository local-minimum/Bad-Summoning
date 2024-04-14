using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGlide : MonoBehaviour
{
    [SerializeField]
    Transform Start;

    [SerializeField]
    Transform End;

    [SerializeField, Range(0, 10)]
    float loopTime;

    void Update()
    {
        transform.position = Vector3.Lerp(Start.transform.position, End.transform.position, (Time.timeSinceLevelLoad / loopTime) % 1);
    }
}
