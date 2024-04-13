using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

enum WoundPhase { Resting, Fill, Intermission, Linger }

public class Enemy : GridEntity
{
    [SerializeField]
    Image leftHandWound;
    [SerializeField]
    Image rightHandWound;

    [SerializeField, Range(0, 1)]
    float woundAnimationDuration = 0.3f;
    [SerializeField, Range(0, 1)]
    float woundIntermission = 0.5f;
    [SerializeField, Range(0, 2)]
    float lastWoundLinger = 1f;

    private void OnEnable()
    {
        leftHandWound.enabled = false;
        rightHandWound.enabled = false;

        AttackUI.OnAttack += AttackUI_OnAttack;
    }

    private void OnDisable()
    {
        AttackUI.OnAttack -= AttackUI_OnAttack;
    }

    private void AttackUI_OnAttack(AttackModifier modifier)
    {
        if (modifier == AttackModifier.Crit)
        {
            showWounds = 3;
        } else if (modifier == AttackModifier.Default)
        {
            showWounds = 2;
        } else
        {
            showWounds = 1;
        }

        woundPhase = WoundPhase.Fill;
        woundPhaseStart = Time.timeSinceLevelLoad;
        SynchVisibleWound(0);
    }

    float woundPhaseStart;
    int showWounds;
    WoundPhase woundPhase = WoundPhase.Resting;

    void SynchVisibleWound(float fill)
    {
        if (showWounds % 2 == 0)
        {
            leftHandWound.enabled = true;
            rightHandWound.enabled = false;
            leftHandWound.fillAmount = fill;
        } else
        {
            leftHandWound.enabled = false;
            rightHandWound.enabled = true;
            rightHandWound.fillAmount = fill;
        }
    }

    private void Update()
    {
        if (cell == null)
        {            
            ChangeCell(GridCell.Map[transform.position.ToVector2Int()], (oldCell, newCell) => { 
                if (oldCell != null) {
                    oldCell.HasEnemy = false;
                }                
                newCell.HasEnemy = true;
                Debug.Log($"{name} spawned at {newCell.name} / {newCell.Coords}");
            });     
        }

        if (woundPhase == WoundPhase.Resting) { return; }

        if (woundPhase == WoundPhase.Fill)
        {
            var fill = Mathf.Clamp01((Time.timeSinceLevelLoad - woundPhaseStart) / woundAnimationDuration);
            SynchVisibleWound(fill);

            if (fill == 1)
            {
                woundPhase = WoundPhase.Intermission;
                woundPhaseStart = Time.timeSinceLevelLoad;
            }
        } else if (woundPhase == WoundPhase.Intermission)
        {
            var progress = Mathf.Clamp01((Time.timeSinceLevelLoad - woundPhaseStart) / woundIntermission);

            if (progress == 1)
            {
                showWounds--;
                if (showWounds <= 0)
                {
                    woundPhase = WoundPhase.Linger;
                }
                else
                {
                    woundPhase = WoundPhase.Fill;
                }
                woundPhaseStart = Time.timeSinceLevelLoad;
            }
        } else if (woundPhase == WoundPhase.Linger)
        {
            var progress = Mathf.Clamp01((Time.timeSinceLevelLoad - woundPhaseStart) / lastWoundLinger);
            if (progress == 1)
            {
                woundPhase = WoundPhase.Resting;
                leftHandWound.enabled = false;
                rightHandWound.enabled = false;
            }
        }
    }
}
