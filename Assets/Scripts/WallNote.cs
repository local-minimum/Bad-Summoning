using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNote : MonoBehaviour
{
    [SerializeField]
    Direction ReadWhenFacing = Direction.North;

    [SerializeField]
    string Message;

    GridCell cell;

    private void Start()
    {
        cell = GetComponentInParent<GridCell>();
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerMove += PlayerController_OnPlayerMove;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerMove -= PlayerController_OnPlayerMove;
    }

    private void PlayerController_OnPlayerMove(GridCell fromCell, GridCell toCell, Direction lookDirection)
    {
        if (toCell == cell && lookDirection == ReadWhenFacing)
        {
            PromptUI.instance.ShowText(Message);
        } else if (fromCell == cell)
        {
            PromptUI.instance.HideText(Message);
        }
    }
}
