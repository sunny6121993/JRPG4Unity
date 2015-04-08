using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatGui : MonoBehaviour
{
    protected bool isCombatEnd;
    protected bool isPlayerPartyWin;
    protected bool isEnemyWin;
    protected Rect gameOverLabelRect;
    protected GUIStyle gameOverLabelStyle;
    protected Rect gameResultRect;
    protected GUIStyle gameResultStyle;
    protected string resultText;
    protected Rect gameResultBackRect;
    protected Texture2D gameResultBackTexture;

    public bool IsCombatEnd
    {
        get { return isCombatEnd; }
        set { isCombatEnd = value; }
    }

    public bool IsPlayerPartyWin
    {
        get { return isPlayerPartyWin; }
        set
        {
            isPlayerPartyWin = value;
            isEnemyWin = !value;
        }
    }

    public bool IsEnemyWin
    {
        get { return isEnemyWin; }
        set
        {
            isEnemyWin = value;
            isPlayerPartyWin = !value;
        }
    }

    protected void InitGuiStyle()
    {
        gameOverLabelStyle = new GUIStyle();
        gameOverLabelStyle.fontSize = 40;
        gameOverLabelStyle.fontStyle = FontStyle.BoldAndItalic;
        gameOverLabelStyle.normal.textColor = Color.red;

        gameResultStyle = new GUIStyle();
        gameResultStyle.fontSize = 20;
        gameResultStyle.fontStyle = FontStyle.Bold;
        gameResultStyle.normal.textColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
    }

    protected void InitRect()
    {
        float gameOverLabelRectWidth = gameOverLabelStyle.CalcSize(new GUIContent("Game Over")).x;
        float gameOverLabelRectHeight = gameOverLabelStyle.CalcSize(new GUIContent("Game Over")).y;

        gameOverLabelRect = new Rect(Screen.width / 2.0f - gameOverLabelRectWidth / 2.0f, Screen.height / 2.0f - gameOverLabelRectHeight / 2.0f, gameOverLabelRectWidth, gameOverLabelRectHeight);
    }

    protected void InitTexture()
    {
        gameResultBackTexture = new Texture2D(1, 1);
        gameResultBackTexture.SetPixel(0, 0, new Color(0.5f, 0.5f, 1.0f, 0.7f));
        gameResultBackTexture.Apply();
    }

	// Use this for initialization
	void Start()
    {
        InitGuiStyle();
        InitRect();
        InitTexture();
	}
	
	// Update is called once per frame
	void Update()
    {
	    
	}

    void OnGUI()
    {
        if (IsCombatEnd)
        {
            if (IsEnemyWin)
            {
                GUI.Label(gameOverLabelRect, "Game Over", gameOverLabelStyle);
            }
            else if (IsPlayerPartyWin)
            {
                GUI.DrawTexture(gameResultBackRect, gameResultBackTexture);
                GUI.Label(gameResultRect, resultText, gameResultStyle);
            }
        }
    }

    public void GenerateResult(int totalExpGain, int totalMoneyGain, string[] levelUpPartyMembers, int[] levelUpPartyMembersOldLv, int[] levelUpPartyMembersNewLv)
    {
        resultText = "Result:\nExp Gain: " + totalExpGain + "\nMoney Gain: " + totalMoneyGain;

        if (levelUpPartyMembers.Length > 0)
        {
            resultText += "\n";
            for (int i = 0; i < levelUpPartyMembers.Length; i++)
            {
                resultText += "\n" + levelUpPartyMembers[i] + " - Lv." + levelUpPartyMembersOldLv[i] + "->" + levelUpPartyMembersNewLv[i];
            }
        }

        float gameResultRectWidth = gameResultStyle.CalcSize(new GUIContent(resultText)).x;
        float gameResultRectHeight = gameResultStyle.CalcSize(new GUIContent(resultText)).y;

        gameResultRect = new Rect(Screen.width / 2.0f - gameResultRectWidth / 2.0f, Screen.height / 2.0f - gameResultRectHeight / 2.0f * 1.5f, gameResultRectWidth, gameResultRectHeight);

        gameResultBackRect = new Rect(gameResultRect.x - 5.0f, gameResultRect.y - 5.0f, gameResultRect.width + 10.0f, gameResultRect.height + 10.0f);
    }
}
