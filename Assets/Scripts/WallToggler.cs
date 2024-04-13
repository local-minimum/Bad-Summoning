using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallToggler : MonoBehaviour
{
    [SerializeField]
    GameObject[] Walls;

    [SerializeField]
    bool AllowRepeatPresses;

    [SerializeField]
    Direction InteractDirection;
       
    GridCell cell;

    [SerializeField]
    Texture OnTex;
    [SerializeField]
    Texture OffTex;

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
        var playerLooksRightDirection = lookDirection == InteractDirection;
        var playerOnRightCell = toCell == cell;
        var buttonAllowsPress = On || AllowRepeatPresses;

        Debug.Log($"{name}: Dir {playerLooksRightDirection} Cell {playerOnRightCell} Can {buttonAllowsPress}");
        
        Interactable = playerLooksRightDirection && playerOnRightCell && buttonAllowsPress;
    }

    bool _isOn = true;
    bool On
    {
        get => _isOn;
        set
        {
            _isOn = value;
            GetComponent<Renderer>().material.mainTexture = value ? OnTex : OffTex;

            foreach (var wall in Walls)
            {
                wall.SetActive(!wall.activeSelf);
            }

            if (!value && !AllowRepeatPresses)
            {
                Interactable = false;
            } 
        }
        
    }

    bool _interactable;
    

    bool Interactable
    {
        get => _interactable;
        set
        {
            if (!_interactable && value)
            {
                RegisterCallback();
            } else if (_interactable && !value)
            {
                UnregisterCallback();
            }
            _interactable = value;            
        }
    }

    PlayerInput _playerInput;
    PlayerInput Input
    {
        get
        {
            if (_playerInput == null)
            {
                _playerInput = FindObjectOfType<PlayerInput>();
            }
            return _playerInput;
        }
    }

    /// <summary>
    /// Inputs/Interact event
    /// </summary>
    const string interactActionId = "911be32a-e601-4f55-bcb3-49f94aa29fff";

    void RegisterCallback()
    {
        foreach(var evt in Input.actionEvents)
        {
            if (evt.actionId != interactActionId)
            {
                Debug.Log($"{evt.actionName}: {evt.actionId}");
                continue;
            }

            evt.AddListener(DoInteract);            
            return;
        }

        Debug.LogWarning("Could not register callback");
    }

    void UnregisterCallback()
    {
        foreach (var evt in Input.actionEvents)
        {
            if (evt.actionId != interactActionId) continue;

            evt.RemoveListener(DoInteract);
            return;
        }

        Debug.LogWarning("Could not unregister callback");
    }

    public void DoInteract(InputAction.CallbackContext context)
    {

        Debug.Log("Press button");

        if (!Interactable) { return; }

        On = !On;
    }
}
