using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public delegate void PlayerMoveEvent(GridCell fromCell, GridCell toCell, Direction lookDirection);

public class PlayerController : GridEntity
{
    public static event PlayerMoveEvent OnPlayerMove;

    Direction _lookDirection;
    public Direction LookDirection
    {
        get { return _lookDirection; }
        private set
        {
            _lookDirection = value;
            transform.rotation = _lookDirection.ToLookVector().AsQuaternion();
            OnPlayerMove?.Invoke(cell, cell, value);
        }
    }

    private void Translate(Direction direction)
    {
        if (cell == null)
        {
            Debug.Log($"{name} is not on the grid!");
            return;
        }

        var newCell = cell.Neighbour(direction);
        if (newCell == null)
        {
            Debug.Log($"{name} can't move {direction} from {cell.name} / {cell.Coords}");
            return;
        }

        ChangeCell(newCell);
    }


    private void ChangeCell(GridCell toCell)
    {
        ChangeCell(toCell, (oldCell, newCell) => {
            if (oldCell != null) oldCell.HasPlayer = false;
            newCell.HasPlayer = true;

            OnPlayerMove?.Invoke(oldCell, newCell, LookDirection);
         });        
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
            ChangeCell(GridCell.Map[transform.position.ToVector2Int()]);            

            LookDirection = LookDirection;
        }
    }

}
