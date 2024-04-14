using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void AttackEvent(AttackModifier modifier);

public class AttackUI : BlockableActions
{
    [SerializeField, Range(0, 4)]
    float speed = 0.8f;

    [SerializeField, Range(0, 2)]
    float betweenAttackPause = 0.75f;

    public static event AttackEvent OnAttack;

    List<AttackZone> _attackZones;
    List<AttackZone> AttackZones
    {
        get
        {
            if (_attackZones == null)
            {
                _attackZones = GetComponentsInChildren<AttackZone>(true).ToList();
            }
            return _attackZones;
        }
    }

    AttackMarker _marker;
    AttackMarker Marker { 
        get {
            if (_marker == null) _marker = GetComponentInChildren<AttackMarker>(true);
            return _marker; 
        } 
    }

    void SetRandomZone()
    {
        var zone = AttackZones.GetRandomElement();
        
        foreach (var z  in AttackZones)
        {
            z.gameObject.SetActive(z == zone);
        }
    }

    bool Fighting;
    bool Hidden;

    void Show()
    {
        SetRandomZone();
        transform.ShowAllChildren();
        Marker.speed = speed;
        Hidden = false;
    }


    private void Hide()
    {
        transform.HideAllChildren();
        Hidden = true;
    }

    private void OnEnable()
    {
        Hide();
        PlayerController.OnPlayerMove += PlayerController_OnPlayerMove;
        Enemy.OnEnemyDeath += Enemy_OnEnemyDeath;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerMove -= PlayerController_OnPlayerMove;
        Enemy.OnEnemyDeath -= Enemy_OnEnemyDeath;
    }

    private void Enemy_OnEnemyDeath(Enemy e)
    {
        Fighting = false;
        Hide();
    }

    private void PlayerController_OnPlayerMove(GridCell fromCell, GridCell toCell, Direction lookDirection)
    {
        var facingEnemy = toCell.Neighbour(lookDirection)?.HasEnemy ?? false;
        if (Fighting && !facingEnemy)
        {
            Hide();
            Fighting = false;
        } else if (!Fighting && facingEnemy)
        {
            Show();
            Fighting = true;
        }
    }

    public void DoAttack(InputAction.CallbackContext context)
    {
        if (ActionsBlocked || !Fighting || Hidden || !context.performed) { return; }

        var progress = Marker.Progress;

        foreach (var z in AttackZones)
        {
            if (!z.gameObject.activeSelf) continue;

            var modifier = z.GetModifier(progress);
            Debug.Log($"Performed an {modifier} attack");

            OnAttack?.Invoke(modifier);
        }

        Hide();
        reShowAttackTime = Time.timeSinceLevelLoad + betweenAttackPause;
    }

    float reShowAttackTime;

    private void Update()
    {
        if (Fighting && Hidden && Time.timeSinceLevelLoad > reShowAttackTime) { 
            Show();
        }
    }
}
