using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField, Range(0, 5)]
    float initialPause;

    [SerializeField, TextArea]
    string[] Subtitles;

    [SerializeField, Range(0, 20)]
    float[] SwapOutTime;

    int subIdx;

    float nextSubTime;

    [SerializeField]
    TextMeshProUGUI Subtitle;

    [SerializeField]
    AudioSource Speaker;    

    bool speechNotStarted = true;
    bool noMoreSubs = false;

    private void Start()
    {
        nextSubTime = Time.timeSinceLevelLoad + initialPause;
    }

    private void Update()
    {
        if (speechNotStarted && Time.timeSinceLevelLoad > initialPause)
        {
            Speaker.Play();
            speechNotStarted = false;
        }

        if (!noMoreSubs && Time.timeSinceLevelLoad > nextSubTime)
        {
            Subtitle.text = Subtitles.GetNthOrDefault(subIdx, "");
            nextSubTime += SwapOutTime.GetNthOrLast(subIdx);
            subIdx++;
            
            if (subIdx >= Subtitles.Length)
            {
                noMoreSubs = true;
            }
        }
    }
}
