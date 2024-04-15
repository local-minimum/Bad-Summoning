using UnityEngine;
using UnityEngine.InputSystem;


public delegate void PlayerMoveEvent(GridCell fromCell, GridCell toCell, Direction lookDirection);

public class PlayerController : GridEntity
{
    public static event PlayerMoveEvent OnPlayerMove;

    [SerializeField]
    Direction startDirection = Direction.East;

    [SerializeField]
    AudioClip[] AngrySounds;
    [SerializeField]
    AudioClip[] WalkSounds;
    [SerializeField]
    AudioClip[] BallHits;

    [SerializeField]
    Transform RayCaster;

    Direction _lookDirection;
    public Direction LookDirection
    {
        get { return _lookDirection; }
        private set
        {
            _lookDirection = value;
            transform.rotation = _lookDirection.ToLookVector().AsQuaternion();
            Debug.Log($"Player Rotate: {LookDirection}");
            if (!ActionsBlocked)
                OnPlayerMove?.Invoke(cell, cell, value);
        }
    }

    private void Translate(Direction direction)
    {
        if (cell == null)
        {
            Debug.Log($"{name} is not on the grid!");            
            return;
        }

        var newCell = cell.Neighbour(direction);
        if (newCell == null)
        {
            Debug.Log($"{name} can't move {direction} from {cell.name} / {cell.Coords}");
            return;
        }

        Speaker.PlayOneShot(WalkSounds.GetRandomElement());
        ChangeCell(newCell);
    }
    
    private void ChangeCell(GridCell toCell)
    {
        if (Physics.Raycast(
            RayCaster.transform.position, 
            LookDirection.ToLookVector().ToDirection(),
            out var hitInfo,
            3.5f
        ))
        {
            var ball = hitInfo.transform.GetComponent<Fireball>();
            if (ball != null)
            {
                ball.CollideWithPlayer();
            }
        }

        ChangeCell(toCell, (oldCell, newCell) => {
            
            if (oldCell != null) oldCell.HasPlayer = false;
            newCell.HasPlayer = true;

            Debug.Log($"Player Move: {newCell.Coords} {LookDirection}");
            OnPlayerMove?.Invoke(oldCell, newCell, LookDirection);
         });        
    }

    #region InputHandling
    public void OnMoveForward(InputAction.CallbackContext context)
    {
        if (ActionsBlocked) return;
        if (context.performed) Translate(LookDirection);
    }

    public void OnMoveBackward(InputAction.CallbackContext context)
    {
        if (ActionsBlocked) return;
        if (context.performed) Translate(LookDirection.Inverse());
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (ActionsBlocked) return;
        if (context.performed)
        {
            if (GameSettings.InvertedControls)
            {
                LookDirection = LookDirection.RotateCCW();
            } else
            {
                Translate(LookDirection.RotateCCW());
            }            
        }
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (ActionsBlocked) return;
        if (context.performed)
        {
            if (GameSettings.InvertedControls)
            {
                LookDirection = LookDirection.RotateCW();
            }
            else
            {
                Translate(LookDirection.RotateCW());
            }           
        }
    }

    public void OnRotateCCW(InputAction.CallbackContext context)
    {
        if (ActionsBlocked) return;
        if (context.performed)
        {
            if (GameSettings.InvertedControls)
            {
                Translate(LookDirection.RotateCCW());
            } else
            {
                LookDirection = LookDirection.RotateCCW();
            }
            
        }
    }
    public void OnRotateCW(InputAction.CallbackContext context)
    {
        if (ActionsBlocked) return;
        if (context.performed)
        {
            if (GameSettings.InvertedControls)
            {
                Translate(LookDirection.RotateCW());

            }
            else
            {
                LookDirection = LookDirection.RotateCW();
            }            
        }
    }
    #endregion

    private void OnEnable()
    {
        Spinner.OnSpinPlayer += Spinner_OnSpinPlayer;
        HealthClock.OnHealthTimeZero += HealthClock_OnHealthTimeZero;
        GameEnd.OnGameEnd += GameEnd_OnGameEnd;
        Fireball.OnHitPlayer += Fireball_OnHitPlayer;
        Enemy.OnAttackPlayer += Enemy_OnAttackPlayer;
    }


    private void OnDisable()
    {
        Spinner.OnSpinPlayer -= Spinner_OnSpinPlayer;
        HealthClock.OnHealthTimeZero -= HealthClock_OnHealthTimeZero;        
        GameEnd.OnGameEnd -= GameEnd_OnGameEnd;
        Fireball.OnHitPlayer -= Fireball_OnHitPlayer;
        Enemy.OnAttackPlayer -= Enemy_OnAttackPlayer;
    }

    private void Enemy_OnAttackPlayer(int damage)
    {
        Speaker.PlayOneShot(AngrySounds.GetRandomElement());
    }

    private void Fireball_OnHitPlayer(Fireball ball)
    {
        Speaker.PlayOneShot(BallHits.GetRandomElement());
    }

    private void GameEnd_OnGameEnd(Direction direction)
    {
        LookDirection = direction;
    }

    private void HealthClock_OnHealthTimeZero()
    {
        Respawn();
    }

    SpinDirection CauseSpin = SpinDirection.None;

    private void Spinner_OnSpinPlayer(SpinDirection direction)
    {
        CauseSpin = direction;
    }

    static Vector2Int SpawnCoordinates;

    void Respawn()
    {
        ChangeCell(GridCell.Map[SpawnCoordinates]);
        LookDirection = startDirection;
        Speaker.PlayOneShot(AngrySounds.GetRandomElement());
    }

    private void Update()
    {
        if (cell == null)
        {
            SpawnCoordinates = transform.position.ToVector2Int();
            Respawn();
        }

        switch (CauseSpin)
        {
            case SpinDirection.Clockwise:
                LookDirection = LookDirection.RotateCW();
                CauseSpin = SpinDirection.None;
                break;
            case SpinDirection.CounterClockwise:
                LookDirection = LookDirection.RotateCCW();
                CauseSpin = SpinDirection.None;
                break;
        }
    }

}
