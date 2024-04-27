using UnityEngine;

public enum SpinDirection { None, Clockwise, CounterClockwise };

public delegate void SpinPlayerEvent(SpinDirection direction);

public class Spinner : MonoBehaviour
{
    public static event SpinPlayerEvent OnSpinPlayer;

    [SerializeField]
    SpinDirection spin;

    [SerializeField]
    AnimationCurve hintTransparency;

    [SerializeField]
    AnimationCurve hintRotationSpeed;

    [SerializeField, Range(0, 40)]
    float hintSpeedMultiplier = 5f;

    GridCell cell;

    [SerializeField]
    Renderer hint;

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

    private void Update()
    {
        if (spin  == SpinDirection.None)
        {
            if (hint.enabled)
            {
                hint.enabled = false;
            }
            return;
        }

        if (!hint.enabled)
        {
            hint.enabled = true;
        }

        // Alpha
        var color = Color.white;
        color.a = hintTransparency.Evaluate(Time.timeSinceLevelLoad);
        hint.material.color = color;

        // Rotate
        var direction = spin == SpinDirection.CounterClockwise ? -1 : 1;
        var rotation = direction * hintRotationSpeed.Evaluate(Time.timeSinceLevelLoad) * hintSpeedMultiplier * Time.deltaTime;
        hint.transform.Rotate(Vector3.up, rotation, Space.World);
    }
}
