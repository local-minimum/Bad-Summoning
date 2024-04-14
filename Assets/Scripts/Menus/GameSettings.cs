using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    static string InvertedControlsKey = "Input.InvertTurnStrafe";
    public static bool InvertedControls
    {
        get => PlayerPrefs.GetInt(InvertedControlsKey, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(InvertedControlsKey, value ? 1 : 0);
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
