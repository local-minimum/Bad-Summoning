using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackModifier { Fail, Default, Crit };

public class AttackZone : MonoBehaviour
{
    [SerializeField]
    RectTransform InZone;

    [SerializeField]
    RectTransform CritZone;

    static bool ProgressInZone(RectTransform zone, float progress)
    {
        if (zone == null) return false;

        return zone.anchorMin.x <= progress && progress <= zone.anchorMax.x;
    }

    public AttackModifier GetModifier(float progress)
    {
        if (ProgressInZone(CritZone, progress)) return AttackModifier.Crit;

        if (ProgressInZone(InZone, progress)) return AttackModifier.Default;

        return AttackModifier.Fail;
    }
}
