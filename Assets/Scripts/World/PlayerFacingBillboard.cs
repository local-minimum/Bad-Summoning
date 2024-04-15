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

    public Vector3 LookTarget { get; set; }

    private void PlayerController_OnPlayerMove(GridCell fromCell, GridCell toCell, Direction d)
    {
        LookTarget = toCell.Coords.ToPosition();
        Sync();
    }

    public void Sync()
    {
        var lookDirection = LookTarget - transform.position;
        lookDirection.y = 0;
        if (lookDirection.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }
}
