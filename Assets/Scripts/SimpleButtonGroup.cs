using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButtonGroup : MonoBehaviour
{
    [SerializeField]
    SimpleButton[] Buttons;

    SimpleButton _Selected;
    public SimpleButton Selected
    {
        get => _Selected;
        set
        {
            if (value != _Selected)
            {
                _Selected.DeSelect();
            }
            _Selected = value;
        }
        
    }
}
