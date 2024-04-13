using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpinDirection { None, Clockwise, CounterClockwise };

public delegate void SpinPlayerEvent(SpinDirection direction);

public class Spinner : MonoBehaviour
{
    public static event SpinPlayerEvent OnSpinPlayer;

    [SerializeField]
    SpinDirection spin;

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
        if (fromCell != cell && toCell == cell && spin != SpinDirection.None)
        {
            OnSpinPlayer?.Invoke(spin);
        } 
    }
}
