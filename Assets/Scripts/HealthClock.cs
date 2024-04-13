using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void HealthTimeZero();

public class HealthClock : MonoBehaviour
{
    public static event HealthTimeZero OnHealthTimeZero;

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
    }

    private void OnDisable()
    {
        Enemy.OnAttackPlayer -= Enemy_OnAttackPlayer;
        Fireball.OnHitPlayer -= Fireball_OnHitPlayer;
    }
    private void Fireball_OnHitPlayer(Fireball ball)
    {
        HealthTime = Mathf.Max(0, HealthTime - ball.BallDamage);
        SyncDisplay(true);

        nextTick = Time.timeSinceLevelLoad + tickFrequency;
        CheckDeath();
    }

    private void Enemy_OnAttackPlayer(int damage)
    {
        HealthTime = Mathf.Max(0, HealthTime - damage);

        SyncDisplay(true);

        nextTick = Time.timeSinceLevelLoad + tickFrequency;
        CheckDeath();
    }

    private void Update()
    {
        if (HealthTime == 0) return;

        if (showingBad && Time.timeSinceLevelLoad > hideBadTime)
        {
            SyncDisplay(false);
        }

        if (Time.timeSinceLevelLoad > nextTick)
        {
            nextTick = Time.timeSinceLevelLoad + tickFrequency;
            HealthTime--;
            SyncDisplay(true);
            CheckDeath();
        }
    }

    void CheckDeath()
    {
        if (HealthTime <= 0)
        {
            OnHealthTimeZero?.Invoke();
            HealthTime = StartHealthTime;
            nextTick = Time.timeSinceLevelLoad + tickFrequency;
        }
    }
}
