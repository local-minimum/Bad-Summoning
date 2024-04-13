using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacingBillboard : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerController.OnPlayerMove += PlayerController_OnPlayerMove;
    }

    public void OnDisable()
    {
        PlayerController.OnPlayerMove -= PlayerController_OnPlayerMove;
    }

    private void PlayerController_OnPlayerMove(GridCell fromCell, GridCell toCell)
    {
        var lookAt = toCell.Coords.ToPosition();
        var lookDirection = lookAt - transform.position;
        lookDirection.y = 0; 
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }
}
