using UnityEngine;
using System.Collections;

public class CombatBehaviours
{
    public enum DataFileType
    {
        TextFile,
        XML,
        Database
    };

    public enum Alignment
    {
        Left,
        Center,
        Right
    };

    public enum CharacterState
    {
        Idle,
        Attack,
        Defense,
        Damage,
        Down,
        Die
    };

    public enum Force
    {
        PlayerParty,
        Enemy
    };

    public static bool CheckDataFileType(DataFileType value)
    {
        return value == DataFileType.TextFile || value == DataFileType.XML || value == DataFileType.Database;
    }

    public static bool CheckAlignment(Alignment value)
    {
        return value == Alignment.Left || value == Alignment.Center || value == Alignment.Right;
    }

    public static bool CheckPartyMemberState(CharacterState value)
    {
        return value == CharacterState.Idle || value == CharacterState.Attack || value == CharacterState.Defense || value == CharacterState.Damage || value == CharacterState.Down;
    }

    public static bool CheckEnemyState(CharacterState value)
    {
        return value == CharacterState.Idle || value == CharacterState.Attack || value == CharacterState.Defense || value == CharacterState.Damage || value == CharacterState.Die;
    }

    public static bool CheckForce(Force value)
    {
        return value == Force.PlayerParty || value == Force.Enemy;
    }
}