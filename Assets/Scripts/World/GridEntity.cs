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

    private GridCell _cell;
    protected GridCell cell => _cell;

    protected void ChangeCell(GridCell newCell, System.Action<GridCell, GridCell> OnChange)
    {
        if (newCell == null || newCell.Occupied)
        {
            return;
        }


        var oldCell = cell;

        _cell = newCell;        
        transform.position = cell.Coords.ToPosition();
        
        OnChange?.Invoke(oldCell, cell);
    }
}
