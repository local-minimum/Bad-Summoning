using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashDamageUI : MonoBehaviour
{
    [SerializeField]
    Image Splash;

    [SerializeField]
    AnimationCurve Transparency;

    [SerializeField, Range(0, 2)]
    float duration = 0.7f;

    void Start()
    {
        transform.HideAllChildren();
    }

    private void OnEnable()
    {
        Fireball.OnHitPlayer += Fireball_OnHitPlayer;
    }

    private void OnDisable()
    {
        Fireball.OnHitPlayer -= Fireball_OnHitPlayer;
    }

    private void Fireball_OnHitPlayer(Fireball ball)
    {
        animationStart = Time.timeSinceLevelLoad;
        animating = true;
        Sync(0);
        transform.ShowAllChildren();
    }

    float animationStart;
    bool animating = false;

    void Sync(float progress)
    {
        var c = Color.white;
        c.a = Transparency.Evaluate(progress);
        Splash.color = c;
    }

    private void Update()
    {
        if (!animating) { return; }

        var progress = Mathf.Clamp01((Time.timeSinceLevelLoad - animationStart) /  duration);
        Sync(progress);

        if (progress == 1)
        {
            animating = false;
            transform.HideAllChildren();
        }
    }

}
