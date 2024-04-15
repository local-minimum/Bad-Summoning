using TMPro;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    static string InvertedControlsKey = "Input.InvertTurnStrafe";
    static bool _invertedSynced = false;
    static bool _cachedInvertedValue = false;
    public static bool InvertedControls
    {
        get {
            if (_invertedSynced) return _cachedInvertedValue;

            _cachedInvertedValue = PlayerPrefs.GetInt(InvertedControlsKey, 0) == 1;
            _invertedSynced = true;
            return _cachedInvertedValue;
        }
        set
        {
            PlayerPrefs.SetInt(InvertedControlsKey, value ? 1 : 0);
            _invertedSynced = true;
            _cachedInvertedValue = value;
        }
    }

    [SerializeField, TextArea]
    string RawInstructions;

    [SerializeField]
    SimpleButton ToggleIntertedBtn;

    [SerializeField]
    TextMeshProUGUI Text;

    [SerializeField]
    SimpleButtonGroup ButtonGroup;

    bool hoveringInverted;

    string Instructions => RawInstructions
        .Replace("{{inverted}}", InvertedControls ? "X" : " ")
        .Replace("{{hoverStart}}", hoveringInverted ? "<color=#910101>" : "")
        .Replace("{{hoverEnd}}", hoveringInverted ? "</color>" : "")
        .Replace("{{turn}}", InvertedControls ? "Strafe" : "Turn")
        .Replace("{{strafe}}", InvertedControls ? "Turn" : "Strafe");

    public void ToggleInverted()
    {
        InvertedControls = !InvertedControls;
        Text.text = Instructions;
    }

    private void OnEnable()
    {
        Text.text = Instructions;
        ButtonGroup.OnSelectButton += ButtonGroup_OnSelectButton;
    }

    private void OnDisable()
    {
        ButtonGroup.OnSelectButton -= ButtonGroup_OnSelectButton;
    }
    private void ButtonGroup_OnSelectButton(SimpleButton selected)
    {
        hoveringInverted = selected == ToggleIntertedBtn;
        Text.text = Instructions;
    }
}
