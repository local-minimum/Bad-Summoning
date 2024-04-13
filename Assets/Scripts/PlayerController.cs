using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public delegate void PlayerMoveEvent(GridCell fromCell, GridCell toCell);

public class PlayerController : MonoBehaviour
{
    public static event PlayerMoveEvent OnPlayerMove;

    private static HashSet<MonoBehaviour> movementBlockers = new HashSet<MonoBehaviour>();
    public static void BlockMovement(MonoBehaviour behaviour) => movementBlockers.Add(behaviour);
    public static void RemoveMovementBlock(MonoBehaviour behaviour) => movementBlockers.Remove(behaviour);
    public static bool MovementBlocked => movementBlockers.Count > 0;

    GridCell cell;

    Direction _lookDirection;
    public Direction LookDirection
    {
        get { return _lookDirection; }
        private set
        {
            _lookDirection = value;
            transform.rotation = _lookDirection.ToLookVector().AsQuaternion();
        }
    }

    private void Translate(Direction direction)
    {
        if (cell == null)
        {
            Debug.Log("Player is not on the grid!");
            return;
        }

        var newCell = cell.Neighbour(direction);
        if (newCell == null)
        {
            Debug.Log($"Can't move {direction} from {name} / {cell.Coords}");
            return;
        }

        ChangeCell(newCell);
    }


    private void ChangeCell(GridCell newCell)
    {        
        if (newCell == null)
        {
            return;
        }

        if (cell != null) {
            cell.HasPlayer = false;
        }
        
        var oldCell = cell;

        cell = newCell;
        cell.HasPlayer = true;
        transform.position = cell.Coords.ToPosition();

        OnPlayerMove?.Invoke(oldCell, cell);
    }

    #region InputHandling
    public void OnMoveForward(InputAction.CallbackContext context)
    {
        if (MovementBlocked) return;
        if (context.performed) Translate(LookDirection);
    }

    public void OnMoveBackward(InputAction.CallbackContext context)
    {
        if (MovementBlocked) return;
        if (context.performed) Translate(LookDirection.Inverse());
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (MovementBlocked) return;
        if (context.performed) Translate(LookDirection.RotateCCW());
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (MovementBlocked) return;
        if (context.performed) Translate(LookDirection.RotateCW());
    }

    public void OnRotateCCW(InputAction.CallbackContext context)
    {
        if (MovementBlocked) return;
        if (context.performed) LookDirection = LookDirection.RotateCCW();
    }
    public void OnRotateCW(InputAction.CallbackContext context)
    {
        if (MovementBlocked) return;
        if (context.performed) LookDirection = LookDirection.RotateCW();
    }
    #endregion
   
    private void Update()
    {
        if (cell == null)
        {
            cell = GridCell.Map[transform.position.ToVector2Int()];
            ChangeCell(cell);            

            LookDirection = LookDirection;
        }
    }

}
