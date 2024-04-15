using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireballShooter : MonoBehaviour
{
    [SerializeField]
    Fireball FireballPrefab;

    [SerializeField, Range(0, 10)]
    float velocity = 1f;

    [SerializeField, Range(0, 10)]
    float shotFrequency = 5f;

    [SerializeField, Range(0, 20)]
    int ballDamage = 10;

    [SerializeField, Range(0, 20)]
    float initialDelay = 1f;

    [SerializeField]
    AudioClip[] ShootSounds;

    List<Fireball> Fireballs = new List<Fireball>();

    AudioSource _speaker;
    protected AudioSource Speaker
    {
        get
        {
            if (_speaker == null)
            {
                _speaker = GetComponent<AudioSource>();
            }
            return _speaker;
        }
    }

    Fireball GetFireball()
    {
        var ball = Fireballs?.FirstOrDefault(f => !f.gameObject.activeSelf);
        if (ball != null) return ball;

        var newBall = Instantiate(FireballPrefab, transform);
        Fireballs.Add(newBall);
        return newBall;
    }

    private void Shoot()
    {
        var ball = GetFireball();
        if (ball == null)
        {
            Debug.LogError($"{name} Did not get a fireball!");
            return;
        }

        ball.transform.localPosition = Vector3.zero;

        ball.Speed = transform.forward * velocity;
        ball.BallDamage = ballDamage;

        ball.GetComponent<PlayerFacingBillboard>().LookTarget = ballLookTarget;

        ball.gameObject.SetActive(true);

        Speaker?.PlayOneShot(ShootSounds.GetRandomElement());
    }

    float nextBall;

    private void OnEnable()
    {
        nextBall = Time.timeSinceLevelLoad + initialDelay;
        PlayerController.OnPlayerMove += PlayerController_OnPlayerMove;
    }

    Vector3 ballLookTarget;
    private void OnDisable()
    {
        PlayerController.OnPlayerMove -= PlayerController_OnPlayerMove;
    }

    private void PlayerController_OnPlayerMove(GridCell fromCell, GridCell toCell, Direction lookDirection)
    {
        ballLookTarget = toCell.Coords.ToPosition();
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > nextBall)
        {
            nextBall = Time.timeSinceLevelLoad + shotFrequency;
            Shoot();
        }
    }
}
