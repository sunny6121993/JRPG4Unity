using UnityEngine;
using System.Collections;

public class CombatEnemyGui : MonoBehaviour
{
    protected CombatEnemyController enemyController;
    protected int no;
    protected Rect hpBarFrontRect;
    protected Rect hpBarBackRect;
    protected Texture2D hpBarFrontTexture;
    protected Texture2D hpBarBackTexture;
    protected int hp;
    protected int maxHp;
    protected bool isDamageDisplayed;
    protected Rect damageRect;
    protected int damage;
    protected GUIStyle damageLabelStyle;

    public CombatEnemyController EnemyController
    {
        get { return enemyController; }
        set { enemyController = value; }
    }

    public int No
    {
        get { return no; }
        set { no = value; }
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

    public bool IsDamageDisplayed
    {
        get { return isDamageDisplayed; }
        set { isDamageDisplayed = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = Mathf.Max(value, -1); }
    }

    protected void InitGuiStyle()
    {
        damageLabelStyle = new GUIStyle();
        damageLabelStyle.normal.textColor = Color.red;
        damageLabelStyle.fontSize = 30;
        damageLabelStyle.fontStyle = FontStyle.Bold;
    }

    protected void InitRect()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(EnemyController.ModelController.gameObject.transform.position);
        Vector2 guiPos = GUIUtility.ScreenToGUIPoint(new Vector2(screenPos.x, screenPos.y));

        // Initialize HP bar back rectangle
        float hpBarBackRectWidth = Screen.width / 6.0f;
        float hpBarBackRectHeight = Screen.height / 40.0f;

        hpBarBackRect = new Rect(guiPos.x - hpBarBackRectWidth / 2.0f, guiPos.y / 6.0f, hpBarBackRectWidth, hpBarBackRectHeight);

        // Initialize HP bar front rectangle
        hpBarFrontRect = new Rect(hpBarBackRect.x, hpBarBackRect.y, hpBarBackRect.width, hpBarBackRect.height);
    }

    protected void InitTexture()
    {
        hpBarBackTexture = new Texture2D(1, 1);
        hpBarBackTexture.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, 1.0f));
        hpBarBackTexture.Apply();

        hpBarFrontTexture = new Texture2D(1, 1);
        hpBarFrontTexture.SetPixel(0, 0, new Color(1.0f, 0.2f, 0.1f, 1.0f));
        hpBarFrontTexture.Apply();
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
        hpBarFrontRect.width = hpBarBackRect.width * ((float)Hp / MaxHp);

        float damageRectWidth = damageLabelStyle.CalcSize(new GUIContent((Damage == -1) ? "Miss" : Damage.ToString())).x;
        float damageRectHeight = damageLabelStyle.CalcSize(new GUIContent((Damage == -1) ? "Miss" : Damage.ToString())).y;

        damageRect = new Rect(hpBarBackRect.x + hpBarBackRect.width - damageRectWidth, hpBarBackRect.y + hpBarBackRect.height, damageRectWidth, damageRectHeight);
	}

    void OnGUI()
    {
        GUI.DrawTexture(hpBarBackRect, hpBarBackTexture);
        GUI.DrawTexture(hpBarFrontRect, hpBarFrontTexture);

        if (IsDamageDisplayed)
        {
            GUI.Label(damageRect, (Damage == -1) ? "Miss" : Damage.ToString(), damageLabelStyle);
        }
    }
}