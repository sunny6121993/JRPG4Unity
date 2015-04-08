using UnityEngine;
using System.Collections;

public class CombatCalculator
{
    public static string[] GetMoveOrder(CombatPartyMemberController[] partyMemberControllers, CombatEnemyController[] enemyControllers)
    {
        string[] moveOrder = new string[partyMemberControllers.Length + enemyControllers.Length];
        for (int i = 0; i < partyMemberControllers.Length; i++)
        {
            moveOrder[i] = "P" + i;
        }
        for (int i = 0; i < enemyControllers.Length; i++)
        {
            moveOrder[partyMemberControllers.Length + i] = "E" + i;
        }

        float[] speeds = new float[partyMemberControllers.Length + enemyControllers.Length];
        for (int i = 0; i < partyMemberControllers.Length; i++)
        {
            speeds[i] = partyMemberControllers[i].Data.Spd * ((partyMemberControllers[i].CurrentState == CombatBehaviours.CharacterState.Defense) ? 1.5f : 1.0f);
        }
        for (int i = 0; i < enemyControllers.Length; i++)
        {
            speeds[partyMemberControllers.Length + i] = enemyControllers[i].Data.Spd * ((enemyControllers[i].CurrentState == CombatBehaviours.CharacterState.Defense) ? 1.5f : 1.0f);
        }

        // Sort speeds in descending order
        for (int i = 0; i < speeds.Length - 1; i++)
        {
            for (int j = 0; j < speeds.Length - 1 - i; j++)
            {
                if (speeds[j] < speeds[j + 1])
                {
                    float tempSpd = speeds[j + 1];
                    speeds[j + 1] = speeds[j];
                    speeds[j] = tempSpd;

                    string tempNo = moveOrder[j + 1];
                    moveOrder[j + 1] = moveOrder[j];
                    moveOrder[j] = tempNo;
                }
            }
        }

        return moveOrder;
    }

    public static int GetPartyMemberAttackDamage(CombatPartyMemberController partyMemberController, CombatEnemyController enemyController)
    {
        if (Random.value * 150 < enemyController.Data.Spd * 10)
        {
            return -1;
        }
        else
        {
            return Mathf.Max((int)(partyMemberController.Data.Atk * (int)(Random.value * 15 + 1) / 10.0f - enemyController.Data.Def * ((enemyController.CurrentState == CombatBehaviours.CharacterState.Defense) ? 1.3f : 1.0f)), 0);
        }
    }

    public static int GetEnemyAttackDamage(CombatEnemyController enemyController, CombatPartyMemberController partyMemberController)
    {
        if (Random.value * 150 < partyMemberController.Data.Spd * 10)
        {
            return -1;
        }
        else
        {
            return Mathf.Max((int)(enemyController.Data.Atk * (int)(Random.value * 15 + 1) / 10.0f - partyMemberController.Data.Def * ((partyMemberController.CurrentState == CombatBehaviours.CharacterState.Defense) ? 1.3f : 1.0f)), 0);
        }
    }
}
