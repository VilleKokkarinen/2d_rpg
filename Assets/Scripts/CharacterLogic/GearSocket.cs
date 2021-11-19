using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSocket : MonoBehaviour
{
    public Animator animator { get; set; }

    protected SpriteRenderer spriteRenderer;

    private Animator parentAnimator;

    private AnimatorOverrideController animatorOverrideController;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        parentAnimator = GetComponentInParent<Animator>();

        animator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        animator.runtimeAnimatorController = animatorOverrideController;
    }


    public virtual void SetXY(float x, float y)
    {

        animator.SetFloat("x", x);
        animator.SetFloat("y", y);
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);

    }


    public void Equip(AnimationClip[] animations)
    {
        spriteRenderer.color = Color.white;
        
        animatorOverrideController["Player_MA_Attack_Back"] = animations[0];
        animatorOverrideController["Player_MA_Attack_Front"] = animations[1];
        animatorOverrideController["Player_MA_Attack_Left"] = animations[2];
        animatorOverrideController["Player_MA_Attack_Right"] = animations[3];

        animatorOverrideController["Player_Idle_Back"] = animations[4];
        animatorOverrideController["Player_Idle_Front"] = animations[5];
        animatorOverrideController["Player_Idle_Left"] = animations[6];
        animatorOverrideController["Player_Idle_Right"] = animations[7];

        animatorOverrideController["Player_Walk_Back"] = animations[8];
        animatorOverrideController["Player_Walk_Front"] = animations[9];
        animatorOverrideController["Player_Walk_Left"] = animations[10];
        animatorOverrideController["Player_Walk_Right"] = animations[11];
    }

    public void DeEquip()
    {
        animatorOverrideController["Player_MA_Attack_Back"] = null;
        animatorOverrideController["Player_MA_Attack_Front"] = null;
        animatorOverrideController["Player_MA_Attack_Left"] = null;
        animatorOverrideController["Player_MA_Attack_Right"] = null;

        animatorOverrideController["Player_Idle_Back"] = null;
        animatorOverrideController["Player_Idle_Front"] = null;
        animatorOverrideController["Player_Idle_Left"] = null;
        animatorOverrideController["Player_Idle_Right"] = null;

        animatorOverrideController["Player_Walk_Back"] = null;
        animatorOverrideController["Player_Walk_Front"] = null;
        animatorOverrideController["Player_Walk_Left"] = null;
        animatorOverrideController["Player_Walk_Right"] = null;

        Color c = spriteRenderer.color;
        c.a = 0;

        spriteRenderer.color = c;

    }
}
