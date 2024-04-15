using UnityEngine;
using UnityEngine.InputSystem;

public class WallToggler : BlockableActions
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

    [SerializeField]
    AudioClip[] ToggleSounds;

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

        // Debug.Log($"{name}: Dir {playerLooksRightDirection} Cell {playerOnRightCell} Can {buttonAllowsPress}");
        
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

    [SerializeField]
    string prompt;

    bool Interactable
    {
        get => _interactable;
        set
        {
            if (!_interactable && value)
            {
                RegisterCallback();
                PromptUI.instance.ShowText(prompt);
            } else if (_interactable && !value)
            {
                UnregisterCallback();
                PromptUI.instance.HideText(prompt);
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

        if (ActionsBlocked || !Interactable) { return; }

        On = !On;
        Speaker.PlayOneShot(ToggleSounds.GetRandomElement());
    }
}
