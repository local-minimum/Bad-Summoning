using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void AttackEvent(AttackModifier modifier);

public class AttackUI : MonoBehaviour
{
    [SerializeField, Range(0, 4)]
    float speed = 0.8f;

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

    void Show()
    {
        Fighting = true;
        SetRandomZone();
        transform.ShowAllChildren();
        Marker.speed = speed;
    }


    private void Hide()
    {
        Fighting = false;
        transform.HideAllChildren();
    }

    private void OnEnable()
    {
        Hide();
        PlayerController.OnPlayerMove += PlayerController_OnPlayerMove;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerMove -= PlayerController_OnPlayerMove;
    }

    private void PlayerController_OnPlayerMove(GridCell fromCell, GridCell toCell, Direction lookDirection)
    {
        var facingEnemy = toCell.Neighbour(lookDirection).HasEnemy;
        if (Fighting && !facingEnemy)
        {
            Hide();
        } else if (!Fighting && facingEnemy)
        {
            Show();
        }
    }

    public void DoAttack(InputAction.CallbackContext context)
    {
        if (!Fighting || !context.performed) { return; }

        var progress = Marker.Progress;

        foreach (var z in AttackZones)
        {
            if (!z.gameObject.activeSelf) continue;

            var modifier = z.GetModifier(progress);
            Debug.Log($"Performed an {modifier} attack");

            OnAttack?.Invoke(modifier);
        }
    }
}
