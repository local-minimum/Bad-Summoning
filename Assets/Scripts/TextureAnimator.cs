using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAnimator : MonoBehaviour
{
    [SerializeField]
    Texture[] Textures;

    [SerializeField, Range(0, 2)]
    float frequency = 0.5f;

    [SerializeField]
    Renderer Rend;

    int index;
    float nextChange;

    private void Start()
    {
        nextChange = frequency;
    }

    
    void Update()
    {
        if (Time.timeSinceLevelLoad  > nextChange)
        {
            index++;
            Rend.material.mainTexture = Textures.GetWrappingNth(index);

            nextChange = Time.timeSinceLevelLoad + frequency;
        }
    }
}
