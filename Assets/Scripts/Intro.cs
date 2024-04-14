using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField]
    Sprite[] Sprites;

    [SerializeField, Range(0, 120)]
    float[] ShowTimes;

    int SpriteIdx;

    [SerializeField]
    Image MainImage;

    [SerializeField]
    SimpleButton SkipContinue;

    [SerializeField]
    string LevelScene = "Level";

    private void Start()
    {
        nextSubTime = Time.timeSinceLevelLoad + initialPause;
        MainImage.enabled = false;
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
            
            if (subIdx > Subtitles.Length)
            {
                noMoreSubs = true;
                SkipContinue.Text = "Continue";
            }
        }

        if (SpriteIdx < ShowTimes.Length && Time.timeSinceLevelLoad > ShowTimes[SpriteIdx])
        {
            if (SpriteIdx == 0)
            { 
                MainImage.enabled = true; 
            }

            MainImage.sprite = Sprites[SpriteIdx];
            SpriteIdx++;
        }
    }

    public void DoSkipContinue()
    {
        SceneManager.LoadScene(LevelScene);
    }
}
