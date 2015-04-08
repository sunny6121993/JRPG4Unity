using UnityEngine;
using System.Collections;

public class CombatPartyMemberGui : MonoBehaviour
{
    protected CombatPartyMemberController partyMemberController;
    protected Rect statusRect;
    protected Rect iconRect;
    protected Rect hpLabelRect;
    protected Rect hpBarFrontRect;
    protected Rect hpBarBackRect;
    protected Rect mpLabelRect;
    protected Rect mpBarFrontRect;
    protected Rect mpBarBackRect;
    protected int no;
    protected int totalPartyMemberNum;
    protected int hp;
    protected int maxHp;
    protected int mp;
    protected int maxMp;
    protected Texture2D iconTexture;
    protected Texture2D statusBackgroundTexture;
    protected Texture2D hpBarFrontNormalTexture;
    protected Texture2D hpBarFrontDangerTexture;
    protected Texture2D hpBarBackTexture;
    protected Texture2D mpBarFrontTexture;
    protected Texture2D mpBarBackTexture;
    protected int maxPartyMemberNum;
    protected CombatBehaviours.Alignment align;
    protected GUISkin guiSkin;
    protected bool isActive;
    protected Rect activeBorderRect;
    protected Texture2D activeBorderTexture;
    protected Rect attackButtonRect;
    protected Rect defenseButtonRect;
    protected Texture2D crosshairTexture;
    protected Rect[] crosshairRects;
    protected bool isSelectingEnemy;
    protected bool isDamageDisplayed;
    protected Rect damageRect;
    protected int damage;
    protected GUIStyle damageLabelStyle;
    protected bool isAttacked;
    protected bool isDown;
    protected Texture2D downIconTexture;

    public CombatPartyMemberController PartyMemberController
    {
        get { return partyMemberController; }
        set { partyMemberController = value; }
    }

    public int No
    {
        get { return no; }
        set { no = Mathf.Max(value, 0); }
    }

    public int TotalPartyMemberNum
    {
        get { return totalPartyMemberNum; }
        set { totalPartyMemberNum = Mathf.Max(value, 1); }
    }

    public int Hp
    {
        get { return hp; }
        set { hp = Mathf.Max(value, 0); }
    }

    public int MaxHp
    {
        get { return maxHp; }
        set { maxHp = Mathf.Max(value, Hp); }
    }

    public int Mp
    {
        get { return mp; }
        set { mp = Mathf.Max(value, 0); }
    }

    public int MaxMp
    {
        get { return maxMp; }
        set { maxMp = Mathf.Max(value, Mp); }
    }

    public Texture2D IconTexture
    {
        get { return iconTexture; }
        set { iconTexture = value; }
    }

    public int MaxPartyMemberNum
    {
        get { return maxPartyMemberNum; }
        set { maxPartyMemberNum = Mathf.Max(value, 1); }
    }

    public CombatBehaviours.Alignment Align
    {
        get { return align; }
        set { align = CombatBehaviours.CheckAlignment(value) ? value : CombatBehaviours.Alignment.Left; }
    }

    public GUISkin GuiSkin
    {
        get { return guiSkin; }
        set { guiSkin = value; }
    }

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    public Texture2D CrosshairTexture
    {
        get { return crosshairTexture; }
        set { crosshairTexture = value; }
    }

    public bool IsSelectingEnemy
    {
        get { return isSelectingEnemy; }
        set { isSelectingEnemy = value; }
    }

    public bool IsDamageDisplayed
    {
        get { return isDamageDisplayed; }
        set { isDamageDisplayed = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public bool IsAttacked
    {
        get { return isAttacked; }
        set { isAttacked = value; }
    }

    public bool IsDown
    {
        get { return isDown; }
        set { isDown = value; }
    }

    protected void InitRect()
    {
        // Initialize status rectangle
        float statusRectWidth = (float)Screen.width / (MaxPartyMemberNum + 1);
        float statusRectHeight = (float)Screen.height / 5.0f;
        float statusRectSpaceWidth = statusRectWidth / (MaxPartyMemberNum + 1);
        float statusRectSpaceHeight = statusRectHeight / 4.0f;

        float statusRectX = 0;
        switch (Align)
        {
            case CombatBehaviours.Alignment.Left:
                statusRectX = statusRectSpaceWidth * (No + 1) + statusRectWidth * No;
                break;
            case CombatBehaviours.Alignment.Center:
                float totalWidth = statusRectWidth * TotalPartyMemberNum + statusRectSpaceWidth * (TotalPartyMemberNum - 1);
                float totalLeft = Screen.width / 2.0f - totalWidth / 2.0f;
                statusRectX = totalLeft + statusRectWidth * No + statusRectSpaceWidth * No;
                break;
            case CombatBehaviours.Alignment.Right:
                statusRectX = Screen.width - statusRectSpaceWidth * (TotalPartyMemberNum - No) - statusRectWidth * (TotalPartyMemberNum - No);
                break;
        }

        statusRect = new Rect(statusRectX, Screen.height - statusRectSpaceHeight - statusRectHeight, statusRectWidth, statusRectHeight);

        float statusRectInsideSpaceWidth = statusRect.width / 20.0f;

        // Initialize icon rectangle
        float iconRectWidth = statusRect.height - statusRectInsideSpaceWidth * 2;

        iconRect = new Rect(statusRect.x + statusRectInsideSpaceWidth, statusRect.y + statusRectInsideSpaceWidth, iconRectWidth, iconRectWidth);

        // Initialize HP label rectangle
        float hpLabelRectWidth = statusRect.width - iconRect.width - statusRectInsideSpaceWidth * 3;
        float hpLabelRectHeight = iconRect.height / 4.0f;

        hpLabelRect = new Rect(iconRect.x + iconRect.width + statusRectInsideSpaceWidth, iconRect.y, hpLabelRectWidth, hpLabelRectHeight);

        // Initialize HP bar back rectangle
        float hpBarBackRectWidth = hpLabelRect.width;

        hpBarBackRect = new Rect(hpLabelRect.x, hpLabelRect.y + hpLabelRect.height, hpBarBackRectWidth, hpLabelRect.height);

        // Initialize HP front rectangle
        hpBarFrontRect = new Rect(hpBarBackRect.x, hpBarBackRect.y, hpBarBackRect.width, hpBarBackRect.height);

        // Initialize MP label rectangle
        float mpLabelRectWidth = hpLabelRect.width;

        mpLabelRect = new Rect(hpBarBackRect.x, hpBarBackRect.y + hpBarBackRect.height, mpLabelRectWidth, hpBarBackRect.height);

        // Initialize MP bar back rectangle
        mpBarBackRect = new Rect(mpLabelRect.x, mpLabelRect.y + mpLabelRect.height, hpBarBackRect.width, mpLabelRect.height);

        // Initialize MP bar front rectangle
        mpBarFrontRect = new Rect(mpBarBackRect.x, mpBarBackRect.y, mpBarBackRect.width, mpBarBackRect.height);

        // Initialize active border rectangle
        activeBorderRect = new Rect(statusRect.x - 10.0f, statusRect.y - 10.0f, statusRect.width + 20.0f, statusRect.height + 20.0f);

        // Initialize attack button rectangle
        float attackButtonRectWidth = activeBorderRect.width / 2.0f;
        float attackButtonRectHeight = GuiSkin.button.CalcSize(new GUIContent("Attack")).y;

        attackButtonRect = new Rect(activeBorderRect.x, activeBorderRect.y - attackButtonRectHeight, attackButtonRectWidth, attackButtonRectHeight);

        // Initialize defense button rectangle
        float defenseButtonRectHeight = GuiSkin.button.CalcSize(new GUIContent("Defense")).y;

        defenseButtonRect = new Rect(attackButtonRect.x + attackButtonRect.width, attackButtonRect.y, activeBorderRect.width - attackButtonRect.width, defenseButtonRectHeight);
        
        // Initialize crosshair rectangles
        crosshairRects = new Rect[PartyMemberController.EnemyControllers.Length];

        for (int i = 0; i < crosshairRects.Length; i++)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(PartyMemberController.EnemyControllers[i].ModelController.gameObject.transform.position);
            Vector2 guiPos = GUIUtility.ScreenToGUIPoint(new Vector2(screenPos.x, screenPos.y));
            crosshairRects[i] = new Rect(guiPos.x - crosshairTexture.width / 10.0f, guiPos.y - crosshairTexture.height / 2.0f, crosshairTexture.width / 5.0f, crosshairTexture.height / 5.0f);
        }
    }

    protected void InitTexture()
    {
        statusBackgroundTexture = new Texture2D(1, 1);
        statusBackgroundTexture.SetPixel(0, 0, new Color(0.5f, 0.5f, 1.0f, 0.75f));
        statusBackgroundTexture.Apply();

        hpBarFrontNormalTexture = new Texture2D(1, 1);
        hpBarFrontNormalTexture.SetPixel(0, 0, new Color(0.0f, 1.0f, 0.0f, 1.0f));
        hpBarFrontNormalTexture.Apply();

        hpBarFrontDangerTexture = new Texture2D(1, 1);
        hpBarFrontDangerTexture.SetPixel(0, 0, new Color(1.0f, 0.0f, 0.0f, 1.0f));
        hpBarFrontDangerTexture.Apply();

        hpBarBackTexture = new Texture2D(1, 1);
        hpBarBackTexture.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, 1.0f));
        hpBarBackTexture.Apply();

        mpBarFrontTexture = new Texture2D(1, 1);
        mpBarFrontTexture.SetPixel(0, 0, new Color(0.0f, 0.0f, 1.0f, 1.0f));
        mpBarFrontTexture.Apply();

        mpBarBackTexture = new Texture2D(1, 1);
        mpBarBackTexture.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, 1.0f));
        mpBarBackTexture.Apply();

        activeBorderTexture = new Texture2D((int)activeBorderRect.width, (int)activeBorderRect.height);
        for (int i = 0; i < activeBorderRect.width; i++)
        {
            for (int j = 0; j < activeBorderRect.height; j++)
            {
                if (i > activeBorderRect.x + 10.0f && i < activeBorderRect.width - 10.0f && j > activeBorderRect.y + 10.0f && j < activeBorderRect.height - 10.0f)
                {
                    activeBorderTexture.SetPixel(i, j, new Color(0.0f, 0.0f, 0.0f, 0.0f));
                }
                else
                {
                    activeBorderTexture.SetPixel(i, j, new Color(1.0f, 1.0f, 0.3f, 1.0f));
                }
            }
        }
        activeBorderTexture.Apply();

        downIconTexture = new Texture2D(IconTexture.width, IconTexture.height);
        for (int i = 0; i < downIconTexture.width; i++)
        {
            for (int j = 0; j < downIconTexture.height; j++)
            {
                Color pixel = IconTexture.GetPixel(i, j);
                float color = 0.2126f * pixel.r + 0.7152f * pixel.g + 0.0722f * pixel.b;
                downIconTexture.SetPixel(i, j, new Color(color, color, color, pixel.a));
            }
        }
        downIconTexture.Apply();
    }

    protected void InitGuiStyle()
    {
        damageLabelStyle = new GUIStyle();
        damageLabelStyle.normal.textColor = Color.red;
        damageLabelStyle.fontSize = 30;
        damageLabelStyle.fontStyle = FontStyle.Bold;
    }

    // Use this for initialization
    void Start()
    {
        InitGuiStyle();
        InitRect();
        InitTexture();

        IsSelectingEnemy = false;
        IsAttacked = false;
        IsDamageDisplayed = false;
        IsDown = false;
	}
	
	// Update is called once per frame
	void Update()
    {
	    hpBarFrontRect.width = hpBarBackRect.width * ((float)Hp / MaxHp);
        mpBarFrontRect.width = mpBarBackRect.width * ((float)Mp / MaxMp);

        float damageRectWidth = damageLabelStyle.CalcSize(new GUIContent((Damage == -1) ? "Miss" : Damage.ToString())).x;
        float damageRectHeight = damageLabelStyle.CalcSize(new GUIContent((Damage == -1) ? "Miss" : Damage.ToString())).y;

        damageRect = new Rect(hpLabelRect.x + hpBarBackRect.width - damageRectWidth, hpLabelRect.y - damageRectHeight, damageRectWidth, damageRectHeight);
	}

    void OnGUI()
    {
        GUI.skin = guiSkin;

        if (IsActive)
        {
            GUI.DrawTexture(activeBorderRect, activeBorderTexture);

            if (!IsSelectingEnemy)
            {
                if (!IsAttacked)
                {
                    if (GUI.Button(attackButtonRect, "Attack", GuiSkin.button))
                    {
                        IsSelectingEnemy = true;
                    }

                    if (GUI.Button(defenseButtonRect, "Defense", GuiSkin.button))
                    {
                        PartyMemberController.Defense();
                    }
                }
            }
            else
            {
                for (int i = 0; i < PartyMemberController.EnemyControllers.Length; i++)
                {
                    if (PartyMemberController.EnemyControllers[i].IsSelected)
                    {
                        GUI.DrawTexture(crosshairRects[i], CrosshairTexture);
                    }

                    if (PartyMemberController.EnemyControllers[i].IsTarget)
                    {
                        IsAttacked = true;
                        PartyMemberController.Attack(i);
                    }
                }
            }
        }

        GUI.DrawTexture(statusRect, statusBackgroundTexture);

        if (!IsDown)
        {
            GUI.DrawTexture(iconRect, IconTexture);
        }
        else
        {
            GUI.DrawTexture(iconRect, downIconTexture);
        }

        GUI.DrawTexture(hpBarBackRect, hpBarBackTexture);

        if ((float)Hp / MaxHp >= 0.2f)
        {
            GUI.DrawTexture(hpBarFrontRect, hpBarFrontNormalTexture);
        }
        else
        {
            GUI.DrawTexture(hpBarFrontRect, hpBarFrontDangerTexture);
        }

        GUI.DrawTexture(mpBarBackRect, mpBarBackTexture);
        GUI.DrawTexture(mpBarFrontRect, mpBarFrontTexture);
        GUI.Label(hpLabelRect, "HP: " + Hp + "/" + MaxHp, GuiSkin.label);
        GUI.Label(mpLabelRect, "MP: " + Mp + "/" + MaxMp, GuiSkin.label);

        if (IsDamageDisplayed)
        {
            GUI.Label(damageRect, (Damage == -1) ? "Miss" : Damage.ToString(), damageLabelStyle);
        }
    }
}