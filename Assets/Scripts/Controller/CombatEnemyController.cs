using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatEnemyController : MonoBehaviour
{
    protected const float DAMAGE_TIME = 1.0f;

    protected string enemyName;
    protected int no;
    protected string dataFolderPath;
    protected CombatBehaviours.DataFileType dataFileType;
    protected string modelFolderPath;
    protected TextAsset dataFile;
    protected EnemyData data;
    protected CombatCharacterController modelController;
    protected CombatBehaviours.CharacterState currentState;
    protected CombatBehaviours.CharacterState previousState;
    protected CombatPartyMemberController[] partyMemberControllers;
    protected CombatController masterController;
    protected bool isSelected;
    protected bool isTarget;
    protected CombatEnemyGui gui;
    protected float damageStartTime;
    protected List<int> affectedPartyMembersNo;
    protected Vector3 modelPosition;

    public string EnemyName
    {
        get { return enemyName; }
        set { enemyName = value; }
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

    public CombatBehaviours.DataFileType DataFileType
    {
        get { return dataFileType; }
        set { dataFileType = CombatBehaviours.CheckDataFileType(value) ? value : CombatBehaviours.DataFileType.TextFile; }
    }

    public string ModelFolderPath
    {
        get { return modelFolderPath; }
        set { modelFolderPath = value; }
    }

    public EnemyData Data
    {
        get { return data; }
    }

    public CombatCharacterController ModelController
    {
        get { return modelController; }
        set { modelController = value; }
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

    public CombatPartyMemberController[] PartyMemberControllers
    {
        get { return partyMemberControllers; }
        set { partyMemberControllers = value; }
    }

    public CombatController MasterController
    {
        get { return masterController; }
        set { masterController = value; }
    }

    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }

    public bool IsTarget
    {
        get { return isTarget; }
        set { isTarget = value; }
    }

    public CombatEnemyGui Gui
    {
        get { return gui; }
        set { gui = value; }
    }

    public Vector3 ModelPosition
    {
        get { return modelPosition; }
        set { modelPosition = value; }
    }

    protected void LoadDataFile()
    {
        if (dataFile == null)
        {
            if (EnemyName != null)
            {
                dataFile = Resources.Load<TextAsset>(DataFolderPath + EnemyName);

                if (dataFile == null)
                {
                    throw new UnityException("Cannot load data from \"" + DataFolderPath + EnemyName + "\"");
                }
            }
            else
            {
                throw new UnityException("Party member is not defined.");
            }
        }
        else
        {
            EnemyName = dataFile.name;
        }
    }

    protected void LoadEnemyDataFromTextFile()
    {
        string text = dataFile.text;

        string name = "Default";
        int hp = 10;
        int mp = 10;
        int atk = 1;
        int def = 1;
        int spd = 1;
        int exp = 1;
        int money = 1;

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

            switch (field)
            {
                case "name":
                    name = value;
                    break;
                case "hp":
                    if (!int.TryParse(value, out hp))
                    {
                        throw new UnityException("Fail to read \"hp\" from \"" + DataFolderPath + EnemyName + "\"");
                    }
                    break;
                case "mp":
                    if (!int.TryParse(value, out mp))
                    {
                        throw new UnityException("Fail to read \"mp\" from \"" + DataFolderPath + EnemyName + "\"");
                    }
                    break;
                case "atk":
                    if (!int.TryParse(value, out atk))
                    {
                        throw new UnityException("Fail to read \"atk\" from \"" + DataFolderPath + EnemyName + "\"");
                    }
                    break;
                case "def":
                    if (!int.TryParse(value, out def))
                    {
                        throw new UnityException("Fail to read \"def\" from \"" + DataFolderPath + EnemyName + "\"");
                    }
                    break;
                case "spd":
                    if (!int.TryParse(value, out spd))
                    {
                        throw new UnityException("Fail to read \"spd\" from \"" + DataFolderPath + EnemyName + "\"");
                    }
                    break;
                case "exp":
                    if (!int.TryParse(value, out exp))
                    {
                        throw new UnityException("Fail to read \"exp\" from \"" + DataFolderPath + EnemyName + "\"");
                    }
                    break;
                case "money":
                    if (!int.TryParse(value, out money))
                    {
                        throw new UnityException("Fail to read \"money\" from \"" + DataFolderPath + EnemyName + "\"");
                    }
                    break;
            }

            startPos = endPos + 1;
        }

        data = new EnemyData(name, hp, mp, atk, def, spd, exp, money);
    }

    protected void LoadEnemyDataFromXml()
    {

    }

    protected void LoadEnemyDataFromDatabase()
    {

    }

    protected void LoadModel()
    {
        GameObject model = Resources.Load<GameObject>(ModelFolderPath + EnemyName);
        if (model == null)
        {
            throw new UnityException("Cannot load model from \"" + ModelFolderPath + EnemyName + "\"");
        }

        GameObject clone = (GameObject)Instantiate(model);

        ModelController = clone.GetComponent<CombatCharacterController>();
        if (ModelController == null)
        {
            throw new UnityException("Cannot get CombatCharacterController from \"" + EnemyName + "\"");
        }

        ModelController.Position = ModelController.OriginalPosition = ModelPosition;
        ModelController.EnemyController = this;
    }

    protected void InitGui()
    {
        Gui = gameObject.AddComponent<CombatEnemyGui>();
        Gui.No = No;
        Gui.Hp = Data.Hp;
        Gui.MaxHp = Data.MaxHp;
        Gui.EnemyController = this;
    }
    
    // Use this for initialization
	void Start()
    {
        LoadDataFile();

        switch (DataFileType)
        {
            case CombatBehaviours.DataFileType.TextFile:
                LoadEnemyDataFromTextFile();
                break;
            case CombatBehaviours.DataFileType.XML:
                LoadEnemyDataFromXml();
                break;
            case CombatBehaviours.DataFileType.Database:
                LoadEnemyDataFromDatabase();
                break;
        }

        LoadModel();

        InitGui();

        CurrentState = CombatBehaviours.CharacterState.Idle;
        IsSelected = false;
        IsTarget = false;

        affectedPartyMembersNo = new List<int>(PartyMemberControllers.Length);
	}

    protected void UpdateModel()
    {
        ModelController.CurrentState = CurrentState;
        ModelController.PreviousState = PreviousState;
    }

    protected void ChangeState(CombatBehaviours.CharacterState state)
    {
        PreviousState = CurrentState;
        CurrentState = state;
    }

    protected void DoAttack()
    {
        List<int> indicesDeleted = new List<int>(affectedPartyMembersNo.Count);

        for (int i = 0; i < affectedPartyMembersNo.Count; i++)
        {
            if (PartyMemberControllers[affectedPartyMembersNo[i]].CurrentState != CombatBehaviours.CharacterState.Damage)
            {
                indicesDeleted.Add(i);
            }
        }

        for (int i = 0; i < indicesDeleted.Count; i++)
        {
            affectedPartyMembersNo.RemoveAt(indicesDeleted[i]);
        }

        if (affectedPartyMembersNo.Count <= 0)
        {
            indicesDeleted.Clear();
            ChangeState(CombatBehaviours.CharacterState.Idle);
            ModelController.AttackMotionDone = false;
            ModelController.DamageMotionDone = false;

            bool combatEnd = true;
            for (int i = 0; i < PartyMemberControllers.Length; i++)
            {
                if (PartyMemberControllers[i].CurrentState != CombatBehaviours.CharacterState.Down)
                {
                    combatEnd = false;
                    break;
                }
            }

            if (combatEnd)
            {
                MasterController.CombatEnd(CombatBehaviours.Force.Enemy);
            }
            else
            {
                MasterController.TurnEnd();
            }
        }
    }

    protected void Die()
    {
        ModelController.gameObject.SetActive(false);
        Gui.enabled = false;
    }

    protected void DoDamage()
    {
        if (Time.time - damageStartTime > DAMAGE_TIME)
        {
            if (Data.Hp <= 0)
            {
                ChangeState(CombatBehaviours.CharacterState.Die);
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
            ModelController.AttackMotionDone = false;
            ModelController.DamageMotionDone = false;

            if (CurrentState == CombatBehaviours.CharacterState.Die)
            {
                Die();
            }
        }
    }

    protected void UpdateGui()
    {
        Gui.Hp = Data.Hp;
    }

	// Update is called once per frame
	void Update()
    {
        UpdateModel();

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
            case CombatBehaviours.CharacterState.Die:
                break;
        }

        UpdateGui();
	}

    protected void Attack(int partyMemberNo)
    {
        ChangeState(CombatBehaviours.CharacterState.Attack);
        PartyMemberControllers[partyMemberNo].Damage(CombatCalculator.GetEnemyAttackDamage(this, PartyMemberControllers[partyMemberNo]));

        affectedPartyMembersNo.Add(partyMemberNo);
    }

    protected void Defense()
    {
        ChangeState(CombatBehaviours.CharacterState.Defense);
        MasterController.TurnEnd();
    }

    protected void DecideAttackTarget()
    {
        float[] randomNums = new float[PartyMemberControllers.Length];
        for (int i = 0; i < randomNums.Length; i++)
        {
            if (PartyMemberControllers[i].CurrentState == CombatBehaviours.CharacterState.Down)
            {
                continue;
            }
            else
            {
                randomNums[i] = Random.value;
            }
        }

        float[] diff = new float[PartyMemberControllers.Length];
        for (int i = 0; i < diff.Length; i++)
        {
            if (PartyMemberControllers[i].CurrentState == CombatBehaviours.CharacterState.Down)
            {
                continue;
            }
            else
            {
                diff[i] = Mathf.Abs(randomNums[i] - (float)PartyMemberControllers[i].Data.Hp / PartyMemberControllers[i].Data.MaxHp);
            }
        }

        int index = 0;
        float temp = diff[0];
        for (int i = 1; i < diff.Length; i++)
        {
            if (PartyMemberControllers[i].CurrentState == CombatBehaviours.CharacterState.Down)
            {
                continue;
            }
            else
            {
                if (diff[i] < temp)
                {
                    temp = diff[i];
                    index = i;
                }
            }
        }

        Attack(index);
    }

    protected void NextCommand()
    {
        if (Random.value > Mathf.Max((float)Data.Hp / Data.MaxHp, 0.4f))
        {
            Defense();
        }
        else
        {
            if (PartyMemberControllers.Length > 1)
            {
                DecideAttackTarget();
            }
            else
            {
                Attack(0);
            }
        }
    }

    public void TurnStart()
    {
        ChangeState(CombatBehaviours.CharacterState.Idle);
        NextCommand();
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

        ModelController.Damage = damage;

        damageStartTime = Time.time;
    }
}