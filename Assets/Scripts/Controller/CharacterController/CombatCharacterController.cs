using UnityEngine;
using System.Collections;

public abstract class CombatCharacterController : MonoBehaviour
{
    protected CombatEnemyController enemyController;
    protected CombatBehaviours.CharacterState currentState;
    protected CombatBehaviours.CharacterState previousState;
    protected Vector3 position;
    protected Vector3 originalPosition;
    protected bool attackMotionDone;
    protected bool damageMotionDone;
    protected int damage;

    public CombatEnemyController EnemyController
    {
        get { return enemyController; }
        set { enemyController = value; }
    }

    public CombatBehaviours.CharacterState CurrentState
    {
        get { return currentState; }
        set { currentState = CombatBehaviours.CheckEnemyState(value) ? value : CombatBehaviours.CharacterState.Idle; }
    }

    public CombatBehaviours.CharacterState PreviousState
    {
        get { return previousState; }
        set { previousState = CombatBehaviours.CheckEnemyState(value) ? value : CombatBehaviours.CharacterState.Idle; }
    }

    public Vector3 Position
    {
        get { return position; }
        set
        {
            position = value;
            gameObject.transform.position = position;
        }
    }

    public Vector3 OriginalPosition
    {
        get { return originalPosition; }
        set { originalPosition = value; }
    }

    public bool AttackMotionDone
    {
        get { return attackMotionDone; }
        set { attackMotionDone = value; }
    }

    public bool DamageMotionDone
    {
        get { return damageMotionDone; }
        set { damageMotionDone = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = Mathf.Max(value, -1); }
    }

    protected abstract void InitState();
    protected abstract void CheckState();
}