using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SimpleButton : MonoBehaviour
{


    [SerializeField]
    Color DefaultColor;
    [SerializeField]
    Color SelectedColor;

    TextMeshProUGUI _colorTarget;
    TextMeshProUGUI ColorTarget {
        get { 
            if (_colorTarget == null)
            {
                _colorTarget = GetComponentInChildren<TextMeshProUGUI>();
            }
            return _colorTarget; 
        }
    }

    [SerializeField]
    UnityEvent OnClick;

    SimpleButtonGroup _group;
    SimpleButtonGroup Group
    {
        get { 
            if (_group == null)
            {
                _group = GetComponentInParent<SimpleButtonGroup>();
            }
            return _group; 
        }
    }
    public void Selected()
    {
        Debug.Log($"Selected {name}");
        ColorTarget.color = SelectedColor;
        Group.Selected = this;
    }

    public void DeSelect()
    {
        Debug.Log($"Deselected {name}");
        ColorTarget.color = DefaultColor;
    }

    public void Click()
    {
        Debug.Log($"Clicked {name}");
        OnClick?.Invoke();
    }
}
