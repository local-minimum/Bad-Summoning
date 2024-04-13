using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GridEntity
{
    private void Update()
    {
        if (cell == null)
        {            
            ChangeCell(GridCell.Map[transform.position.ToVector2Int()], (oldCell, newCell) => { 
                if (oldCell != null) {
                    oldCell.HasEnemy = false;
                }                
                newCell.HasEnemy = true;
                Debug.Log($"{name} spawned at {newCell.name} / {newCell.Coords}");
            });     
        }
    }
}
