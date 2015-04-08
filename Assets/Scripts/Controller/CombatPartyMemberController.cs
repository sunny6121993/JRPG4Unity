using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatPartyMemberController : MonoBehaviour
{
    protected const float DAMAGE_TIME = 1.0f;

    protected string partyMemberName;
    protected int no;
    protected string dataFolderPath;
    protected string iconFolderPath;
    protected CombatBehaviours.DataFileType dataFileType;
    protected TextAsset dataFile;
    protected PartyMemberData data;
    protected CombatPartyMemberGui gui;
    protected int totalPartyMemberNum;
    protected Texture2D iconTexture;
    protected int maxPartyMemberNum;
    protected CombatBehaviours.Alignment guiAlign;
    protected GUISkin guiSkin;
    protected CombatBehaviours.CharacterState currentState;
    protected CombatBehaviours.CharacterState previousState;
    protected Texture2D crosshairTexture;
    protected CombatEnemyController[] enemyControllers;
    protected CombatController masterController;
    protected List<int> affectedEnemiesNo;
    protected float damageStartTime;

    public string PartyMemberName
    {
        get { return partyMemberName; }
        set { partyMemberName = value; }
    }

    public int No
    {
        get { return no; }
        set { no = Mathf.Max(value, 0); }
    }

    public string DataFolderPath
    {
        get { return dataFolderPath; }
        set { dataFolderPath = value; }
    }

    public string IconFolderPath
    {
        get { return iconFolderPath; }
        set { iconFolderPath = value; }
    }

    public CombatBehaviours.DataFileType DataFileType
    {
        get { return dataFileType; }
        set { dataFileType = CombatBehaviours.CheckDataFileType(value) ? value : CombatBehaviours.DataFileType.TextFile; }
    }

    public int TotalPartyMemberNum
    {
        get { return totalPartyMemberNum; }
        set { totalPartyMemberNum = Mathf.Max(value, 1); }
    }

    public int MaxPartyMemberNum
    {
        get { return maxPartyMemberNum; }
        set { maxPartyMemberNum = Mathf.Max(value, 1); }
    }

    public CombatBehaviours.Alignment GuiAlign
    {
        get { return guiAlign; }
        set { guiAlign = CombatBehaviours.CheckAlignment(value) ? value : CombatBehaviours.Alignment.Left; }
    }

    public GUISkin GuiSkin
    {
        get { return guiSkin; }
        set { guiSkin = value; }
    }

    public PartyMemberData Data
    {
        get { return data; }
        set { data = value; }
    }

    public CombatPartyMemberGui Gui
    {
        get { return gui; }
        set { gui = value; }
    }

    public CombatBehaviours.CharacterState CurrentState
    {
        get { return currentState; }
        set { currentState = CombatBehaviours.CheckPartyMemberState(value) ? value : CombatBehaviours.CharacterState.Idle; }
    }

    public CombatBehaviours.CharacterState PreviousState
    {
        get { return previousState; }
        set { previousState = CombatBehaviours.CheckPartyMemberState(value) ? value : CombatBehaviours.CharacterState.Idle; }
    }

    public Texture2D CrosshairTexture
    {
        get { return crosshairTexture; }
        set { crosshairTexture = value; }
    }

    public CombatEnemyController[] EnemyControllers
    {
        get { return enemyControllers; }
        set { enemyControllers = value; }
    }

    public CombatController MasterController
    {
        get { return masterController; }
        set { masterController = value; }
    }
    
    protected void LoadDataFile()
    {
        if (dataFile == null)
        {
            if (PartyMemberName != null)
            {
                dataFile = Resources.Load<TextAsset>(DataFolderPath + PartyMemberName);

                if (dataFile == null)
                {
                    throw new UnityException("Cannot load data from \"" + DataFolderPath + PartyMemberName + "\"");
                }
            }
            else
            {
                throw new UnityException("Party member is not defined.");
            }
        }
        else
        {
            PartyMemberName = dataFile.name;
        }
    }

    protected void LoadPartyMemberDataFromTextFile()
    {
        string text = dataFile.text;

        string name = "Default";
        int lv = 1;
        int nextExp = 1;
        int hp = 10;
        int mp = 10;
        int atk = 1;
        int def = 1;
        int spd = 1;

        int startPos = 0;
        int endPos = 0;
        string field = null;
        string value = null;
        while (endPos < text.Length)
        {
            endPos = text.IndexOf(" ", startPos);
            field = text.Substring(startPos, endPos - startPos);
            startPos = endPos + 1;

            endPos = text.IndexOf("\n", startPos);
            if (endPos == -1)
            {
                endPos = text.Length;
            }

            value = text.Substring(startPos, endPos - startPos);
            value = value.Trim();

            switch (field)
            {
                case "name":
                    name = value;
                    break;
                case "icon":
                    iconTexture = Resources.Load<Texture2D>(IconFolderPath + value);
                    if (iconTexture == null)
                    {
                        throw new UnityException("Failed to load icon from \"" + IconFolderPath + value + "\"");
                    }
                    break;
                case "nextExp":
                    if (!int.TryParse(value, out nextExp))
                    {
                        throw new UnityException("Fail to read \"nextExp\" from \"" + DataFolderPath + partyMemberName + "\"");
                    }
                    break;
                case "lv":
                    if (!int.TryParse(value, out lv))
                    {
                        throw new UnityException("Fail to read \"lv\" from \"" + DataFolderPath + PartyMemberName + "\"");
                    }
                    break;
                case "hp":
                    if (!int.TryParse(value, out hp))
                    {
                        throw new UnityException("Fail to read \"hp\" from \"" + DataFolderPath + PartyMemberName + "\"");
                    }
                    break;
                case "mp":
                    if (!int.TryParse(value, out mp))
                    {
                        throw new UnityException("Fail to read \"mp\" from \"" + DataFolderPath + PartyMemberName + "\"");
                    }
                    break;
                case "atk":
                    if (!int.TryParse(value, out atk))
                    {
                        throw new UnityException("Fail to read \"atk\" from \"" + DataFolderPath + PartyMemberName + "\"");
                    }
                    break;
                case "def":
                    if (!int.TryParse(value, out def))
                    {
                        throw new UnityException("Fail to read \"def\" from \"" + DataFolderPath + PartyMemberName + "\"");
                    }
                    break;
                case "spd":
                    if (!int.TryParse(value, out spd))
                    {
                        throw new UnityException("Fail to read \"spd\" from \"" + DataFolderPath + PartyMemberName + "\"");
                    }
                    break;
            }

            startPos = endPos + 1;
        }

        data = new PartyMemberData(name, lv, nextExp, hp, mp, atk, def, spd);
    }

    protected void LoadPartyMemberDataFromXml()
    {

    }

    protected void LoadPartyMemberDataFromDatabase()
    {

    }

    protected void InitGui()
    {
        Gui = gameObject.AddComponent<CombatPartyMemberGui>();
        Gui.No = No;
        Gui.TotalPartyMemberNum = TotalPartyMemberNum;
        Gui.Hp = Data.Hp;
        Gui.MaxHp = Data.MaxHp;
        Gui.Mp = Data.Mp;
        Gui.MaxMp = Data.MaxMp;
        Gui.IconTexture = iconTexture;
        Gui.MaxPartyMemberNum = MaxPartyMemberNum;
        Gui.Align = GuiAlign;
        Gui.GuiSkin = GuiSkin;
        Gui.CrosshairTexture = CrosshairTexture;
        Gui.PartyMemberController = this;
    }
    
    // Use this for initialization
	void Start()
    {
        LoadDataFile();

        switch (DataFileType)
        {
            case CombatBehaviours.DataFileType.TextFile:
                LoadPartyMemberDataFromTextFile();
                break;
            case CombatBehaviours.DataFileType.XML:
                LoadPartyMemberDataFromXml();
                break;
            case CombatBehaviours.DataFileType.Database:
                LoadPartyMemberDataFromDatabase();
                break;
        }

        InitGui();

        CurrentState = CombatBehaviours.CharacterState.Idle;
        affectedEnemiesNo = new List<int>(enemyControllers.Length);
	}

    protected void ChangeState(CombatBehaviours.CharacterState state)
    {
        PreviousState = CurrentState;
        CurrentState = state;
    }

    protected void DoAttack()
    {
        List<int> indicesDeleted = new List<int>(affectedEnemiesNo.Count);

        for (int i = 0; i < affectedEnemiesNo.Count; i++)
        {
            if (EnemyControllers[affectedEnemiesNo[i]].CurrentState != CombatBehaviours.CharacterState.Damage)
            {
                indicesDeleted.Add(i);
            }
        }

        for (int i = 0; i < indicesDeleted.Count; i++)
        {
            affectedEnemiesNo.RemoveAt(indicesDeleted[i]);
        }

        if (affectedEnemiesNo.Count <= 0)
        {
            indicesDeleted.Clear();
            ChangeState(CombatBehaviours.CharacterState.Idle);
            Gui.IsDamageDisplayed = false;
            Gui.IsAttacked = false;
            Gui.IsSelectingEnemy = false;
            Gui.IsActive = false;

            bool combatEnd = true;
            for (int i = 0; i < EnemyControllers.Length; i++)
            {
                if (EnemyControllers[i].CurrentState != CombatBehaviours.CharacterState.Die)
                {
                    combatEnd = false;
                    break;
                }
            }

            if (combatEnd)
            {
                MasterController.CombatEnd(CombatBehaviours.Force.PlayerParty);
            }
            else
            {
                MasterController.TurnEnd();
            }
        }
    }

    protected void DoDamage()
    {
        if (Time.time - damageStartTime > DAMAGE_TIME)
        {
            if (Data.Hp <= 0)
            {
                ChangeState(CombatBehaviours.CharacterState.Down);
                Gui.IsDown = true;
            }
            else
            {
                if (PreviousState == CombatBehaviours.CharacterState.Defense)
                {
                    ChangeState(CombatBehaviours.CharacterState.Defense);
                }
                else
                {
                    ChangeState(CombatBehaviours.CharacterState.Idle);
                }
            }

            Gui.IsDamageDisplayed = false;
        }
    }

    protected void UpdateGui()
    {
        Gui.Hp = Data.Hp;
        Gui.Mp = Data.Mp;
    }

	// Update is called once per frame
	void Update()
    {
        switch (CurrentState)
        {
            case CombatBehaviours.CharacterState.Idle:
                break;
            case CombatBehaviours.CharacterState.Attack:
                DoAttack();
                break;
            case CombatBehaviours.CharacterState.Defense:
                break;
            case CombatBehaviours.CharacterState.Damage:
                DoDamage();
                break;
            case CombatBehaviours.CharacterState.Down:
                break;
        }

        UpdateGui();
	}

    public void TurnStart()
    {
        Gui.IsActive = true;
        ChangeState(CombatBehaviours.CharacterState.Idle);
    }

    public void Attack(int enemyNo)
    {
        ChangeState(CombatBehaviours.CharacterState.Attack);

        enemyControllers[enemyNo].Damage(CombatCalculator.GetPartyMemberAttackDamage(this, enemyControllers[enemyNo]));

        enemyControllers[enemyNo].IsTarget = false;
        enemyControllers[enemyNo].IsSelected = false;

        affectedEnemiesNo.Add(enemyNo);
    }

    public void Defense()
    {
        ChangeState(CombatBehaviours.CharacterState.Defense);
        Gui.IsDamageDisplayed = false;
        Gui.IsAttacked = false;
        Gui.IsSelectingEnemy = false;
        Gui.IsActive = false;
        MasterController.TurnEnd();
    }

    public void Damage(int damage)
    {
        ChangeState(CombatBehaviours.CharacterState.Damage);

        if (damage > -1)
        {
            Data.Hp -= damage;
        }
        
        Gui.Damage = damage;
        Gui.IsDamageDisplayed = true;

        damageStartTime = Time.time;
    }
}