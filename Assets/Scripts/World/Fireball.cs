using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public delegate void HitPlayerEvent(Fireball ball);

public class Fireball : MonoBehaviour
{
    public static event HitPlayerEvent OnHitPlayer;

    [SerializeField]
    PlayerFacingBillboard billboard;

    public Vector3 Speed { get; set; }
    public int BallDamage { get; set; } 

    private void OnEnable()
    {
        PlayerController.OnPlayerMove += PlayerController_OnPlayerMove;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerMove -= PlayerController_OnPlayerMove;
    }

    GridCell Cell => GridCell.Map.GetValueOrDefault(transform.position.ToVector2Int());

    private void PlayerController_OnPlayerMove(GridCell fromCell, GridCell toCell, Direction lookDirection)
    {
        if (toCell == Cell)
        {
            OnHitPlayer?.Invoke(this);
            Recycle();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerController>() != null)
        {
            OnHitPlayer?.Invoke(this);
        }

        Recycle();
    }

    private void Recycle()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position += Speed * Time.deltaTime;
        billboard.Sync();
    }
}
