using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GameEndEvent(Direction direction);

public class GameEnd : MonoBehaviour
{
    public static event GameEndEvent OnGameEnd;

    [SerializeField]
    Direction forceDirection = Direction.South;

    private void OnEnable()
    {
        PlayerController.OnPlayerMove += PlayerController_OnPlayerMove;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerMove -= PlayerController_OnPlayerMove; 
    }

    GridCell cell;

    private void Start()
    {
        cell = GetComponentInParent<GridCell>();
    }

    private void PlayerController_OnPlayerMove(GridCell fromCell, GridCell toCell, Direction lookDirection)
    {
        if (toCell == cell)
        {
            GridEntity.BlockMovement(this);
            PromptUI.instance.ShowText("The End");
            OnGameEnd?.Invoke(forceDirection);
        }
    }
}
