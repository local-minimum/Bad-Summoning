using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridEntity : MonoBehaviour
{
    private static HashSet<MonoBehaviour> movementBlockers = new HashSet<MonoBehaviour>();
    public static void BlockMovement(MonoBehaviour behaviour) => movementBlockers.Add(behaviour);
    public static void RemoveMovementBlock(MonoBehaviour behaviour) => movementBlockers.Remove(behaviour);
    public static bool MovementBlocked => movementBlockers.Count > 0;

    protected GridCell cell;

    protected void ChangeCell(GridCell newCell, System.Action<GridCell, GridCell> OnChange)
    {
        if (newCell == null || newCell.Occupied)
        {
            return;
        }


        var oldCell = cell;

        cell = newCell;        
        transform.position = cell.Coords.ToPosition();
        
        OnChange?.Invoke(oldCell, cell);
    }
}
