using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void GameEndEvent(Direction direction);

public class GameEnd : MonoBehaviour
{
    public static event GameEndEvent OnGameEnd;

    [SerializeField]
    Direction forceDirection = Direction.South;

    [SerializeField]
    string message = "This is where you need to be";

    [SerializeField]
    string endScene;

    [SerializeField, Range(0, 10)]
    float loadDelay = 4f;

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
            GridEntity.BlockAction(this);
            PromptUI.instance.ShowText(message);
            OnGameEnd?.Invoke(forceDirection);
            endTime = Time.timeSinceLevelLoad + loadDelay;
            ending = true;
        }
    }

    float endTime;
    bool ending;

    private void Update()
    {
        if (!ending || Time.timeSinceLevelLoad < endTime) { return; }

        SceneManager.LoadScene(endScene);
    }

}
