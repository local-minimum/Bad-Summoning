using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridEntity : BlockableActions
{
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

    protected GridCell oldCell { get; private set; }
    private GridCell _cell;
    protected GridCell cell => _cell;

    protected void ChangeCell(GridCell newCell, System.Action<GridCell, GridCell> OnChange, bool allowTeleport = false)
    {
        if (newCell == null || newCell.Occupied)
        {
            return;
        }

        if (oldCell != null && newCell.Coords.ManhattanDistance(cell.Coords) > 1 && !allowTeleport)
        {
            Debug.LogWarning($"Teleporting is not allowed! {cell.Coords}->{newCell.Coords} = {cell.Coords.ManhattanDistance(newCell.Coords)}");
            return;
        }

        Debug.Log($"OldCell: {oldCell?.Coords} Cell: {cell?.Coords} NewCell: {newCell.Coords}");
        oldCell = cell;

        _cell = newCell;        
        transform.position = cell.Coords.ToPosition();
        
        OnChange?.Invoke(oldCell, cell);
    }
}
