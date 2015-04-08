using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatCubeController : CombatCharacterController
{
    protected Quaternion originalRotation;
    protected float angle;
    
    protected override void InitState()
    {
        CurrentState = CombatBehaviours.CharacterState.Idle;
    }

	// Use this for initialization
	void Start()
    {
        gameObject.name += EnemyController.No;

        InitState();
        originalRotation = gameObject.transform.rotation;

        AttackMotionDone = false;
        DamageMotionDone = false;

        angle = 0;
	}

    protected void DoIdle()
    {
        if (PreviousState == CombatBehaviours.CharacterState.Defense)
        {
            gameObject.transform.rotation = originalRotation;
        }
        gameObject.transform.Rotate(-30.0f * Time.deltaTime, 0.0f, -30.0f * Time.deltaTime);
    }

    protected void DoAttack()
    {
        if (!AttackMotionDone)
        {
            angle += 30.0f;
            Position = new Vector3(OriginalPosition.x, OriginalPosition.y, -0.1f * Mathf.Sin(angle * Mathf.Deg2Rad));

            if (angle >= 180)
            {
                AttackMotionDone = true;
                angle = 0.0f;
                Position = OriginalPosition;
            }
        }
    }

    protected void DoDefense()
    {
        gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
    }

    protected void DoDamage()
    {
        if (Damage > 0 && !DamageMotionDone)
        {
            angle += 30.0f;
            Position = new Vector3(OriginalPosition.x, OriginalPosition.y + 0.1f * Mathf.Sin(angle * Mathf.Deg2Rad), OriginalPosition.z);

            if (angle >= 360)
            {
                DamageMotionDone = true;
                angle = 0.0f;
                Position = OriginalPosition;
            }
        }
    }

    protected override void CheckState()
    {
        switch (CurrentState)
        {
            case CombatBehaviours.CharacterState.Idle:
                DoIdle();
                break;
            case CombatBehaviours.CharacterState.Attack:
                DoAttack();
                break;
            case CombatBehaviours.CharacterState.Defense:
                DoDefense();
                break;
            case CombatBehaviours.CharacterState.Damage:
                DoDamage();
                break;
            case CombatBehaviours.CharacterState.Die:
                break;
        }
    }

	// Update is called once per frame
	void Update()
    {
        CheckState();
	}
}