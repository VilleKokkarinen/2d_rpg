using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Transform[] exitPoints;

    private bool updateDirection = false;
    
    private float FieldOfView = 120;

    protected override void Update()
    {
        LookAtTarget();
        base.Update();
    }

    public void Shoot(int exitIndex)
    {
        SpellScript s = Instantiate(projectilePrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
        s.Initialize(Target.HitBox, baseDamage, this);
    }

    private void LateUpdate()
    {
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        if (updateDirection)
        {
            Vector2 dir = Vector2.zero;

            if (spriteRenderer.sprite.name.Contains("Up"))
            {
                dir = Vector2.up;
            }
            else if (spriteRenderer.sprite.name.Contains("Down"))
            {
                dir = Vector2.down;
            }
            else if (spriteRenderer.sprite.name.Contains("Left"))
            {
                dir = Vector2.left;
            }
            else if (spriteRenderer.sprite.name.Contains("Right"))
            {
                dir = Vector2.right;
            }

            MyAnimator.SetFloat("x", dir.x);
            MyAnimator.SetFloat("y", dir.y);

            updateDirection = false;
        }       
    }


    private void LookAtTarget()
    {
        if(Target != null)
        {
            Vector2 directionToTarget = (Target.transform.position - transform.position).normalized;

            Vector2 facing = new Vector2(MyAnimator.GetFloat("x"), MyAnimator.GetFloat("y"));

            float angleToTarget = Vector2.Angle(facing, directionToTarget);

            if(angleToTarget > 80)
            {
                MyAnimator.SetFloat("x", directionToTarget.x);
                MyAnimator.SetFloat("y", directionToTarget.y);

                updateDirection = true;
            }
        }
    }


}
