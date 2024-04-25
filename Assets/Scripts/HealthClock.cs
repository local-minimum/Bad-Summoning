using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void HealthTimeZero();

public class HealthClock : MonoBehaviour
{
    public static event HealthTimeZero OnHealthTimeZero;

    [SerializeField]
    AudioClip[] CountDownSounds;

    [SerializeField]
    Sprite[] NormalNumbers;

    [SerializeField]
    Sprite[] BadNumbers;

    [SerializeField]
    Image Tens;

    [SerializeField]
    Image Singles;

    [SerializeField, Range(0, 5)]
    float tickFrequency = 1;

    [SerializeField, Range(0, 99)]
    int StartHealthTime= 60;

    [SerializeField, Range(0, 1)]
    float showBadTime = 0.2f;

    [SerializeField]
    AudioSource Speaker;

    [SerializeField]
    AudioClip CompletionVoiceline;

    public int HealthTime {  get; private set; }

    private void Start()
    {
        HealthTime = StartHealthTime;
        SyncDisplay();
        nextTick = tickFrequency;
    }

    bool showingBad = false;
    float hideBadTime;

    int lastSingle;
    int lastTen;

    void SyncDisplay(bool badTime = false)
    {
        var single = HealthTime % 10;
        var ten = (HealthTime - single) / 10;
        
        if (badTime)
        {
            Singles.sprite = single != lastSingle ? BadNumbers.GetNthOrDefault(single, BadNumbers[0]) : NormalNumbers.GetNthOrDefault(single, NormalNumbers[0]);
            Tens.sprite = ten != lastTen ? BadNumbers.GetNthOrDefault(ten, BadNumbers[0]) : NormalNumbers.GetNthOrDefault(ten, NormalNumbers[0]);
            showingBad = true;
            hideBadTime = Time.timeSinceLevelLoad + showBadTime;
        } else
        {
            Singles.sprite = NormalNumbers.GetNthOrDefault(single, NormalNumbers[0]);
            Tens.sprite = NormalNumbers.GetNthOrDefault(ten, NormalNumbers[0]);
            showingBad = false;
        }

        lastSingle = single;
        lastTen = ten;
    }

    float nextTick;

    private void OnEnable()
    {
        Enemy.OnAttackPlayer += Enemy_OnAttackPlayer;
        Fireball.OnHitPlayer += Fireball_OnHitPlayer;
        GameEnd.OnGameEnd += GameEnd_OnGameEnd;
        Pentagram.OnComplete += Pentagram_OnComplete;
        PlayerController.OnPlayerMove += PlayerController_OnPlayerMove;
    }


    private void OnDisable()
    {
        Enemy.OnAttackPlayer -= Enemy_OnAttackPlayer;
        Fireball.OnHitPlayer -= Fireball_OnHitPlayer;
        GameEnd.OnGameEnd -= GameEnd_OnGameEnd;
        Pentagram.OnComplete -= Pentagram_OnComplete;
        PlayerController.OnPlayerMove -= PlayerController_OnPlayerMove;
    }

    private void PlayerController_OnPlayerMove(GridCell fromCell, GridCell toCell, Direction lookDirection)
    {
        InPentagram = toCell.gameObject.GetComponentInChildren<Pentagram>() != null;
    }

    private void Pentagram_OnComplete()
    {
        FreeFromTime = true;
    }

    private void GameEnd_OnGameEnd(Direction direction)
    {
        FreeFromTime = true;
    }

    private void Fireball_OnHitPlayer(Fireball ball)
    {
        if (FreeFromTime || InPentagram) return;

        HealthTime = Mathf.Max(0, HealthTime - ball.BallDamage);

        PlayCountdownSound();
        SyncDisplay(true);

        nextTick = Time.timeSinceLevelLoad + tickFrequency;
        CheckDeath();
    }

    private void Enemy_OnAttackPlayer(int damage)
    {
        if (FreeFromTime || InPentagram) return;

        HealthTime = Mathf.Max(0, HealthTime - damage);

        PlayCountdownSound();
        SyncDisplay(true);

        nextTick = Time.timeSinceLevelLoad + tickFrequency;
        CheckDeath();
    }

    void PlayCountdownSound()
    {
        if (CountDownSounds == null)
        {
            return;
        }

        var sound = CountDownSounds.GetNthOrDefault(HealthTime, null);
        if (sound != null)
        {
            Speaker?.PlayOneShot(sound);
        }        
    }

    bool _FreeFromTime = false;
    public bool FreeFromTime { 
        get => _FreeFromTime; 
        set
        {
            if (_FreeFromTime == false && value == true)
            {
                Speaker.PlayOneShot(CompletionVoiceline);
            }
            _FreeFromTime = value;

        }
    }

    bool InPentagram { get; set; } = true;


    private void Update()
    {
        if (InPentagram)
        {
            HealthTime = StartHealthTime;
            SyncDisplay(false);
            return;
        }

        if (FreeFromTime || HealthTime == 0) return;

        if (showingBad && Time.timeSinceLevelLoad > hideBadTime)
        {
            SyncDisplay(false);
        }

        if (Time.timeSinceLevelLoad > nextTick)
        {
            nextTick = Time.timeSinceLevelLoad + tickFrequency;
            HealthTime--;

            PlayCountdownSound();
            SyncDisplay(true);

            CheckDeath();
        }
    }

    void CheckDeath()
    {
        if (FreeFromTime) return;

        if (HealthTime <= 0)
        {
            OnHealthTimeZero?.Invoke();
            HealthTime = StartHealthTime;
            nextTick = Time.timeSinceLevelLoad + tickFrequency;
        }
    }
}
