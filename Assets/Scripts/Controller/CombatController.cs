using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
    protected const float ENEMY_DISTANCE = 1.75f;

    public Camera MainCamera;
    public GameObject Ground;
    public GameObject FieldLight;
    public Material SkyboxMaterial;
    public string IconFolderPath = "Images/Icons/";
    public CombatBehaviours.DataFileType DataFileType;
    public Texture2D CrosshairTexture;
    public GameObject PartyMemberController;
    public int MaxPartyMemberNumber = 4;
    public string[] PartyMembersName;
    public string PartyMemberDataFolderPath = "Data/PartyMemberData/";
    public CombatBehaviours.Alignment PartyMemberGuiAlign;
    public GUISkin PartyMemberGuiSkin;
    public GameObject EnemyController;
    public string[] EnemiesName;
    public string EnemyDataFolderPath = "Data/EnemyData/";
    public string EnemyModelFolderPath = "Characters/";

    protected CombatPartyMemberController[] partyMemberControllers;
    protected CombatEnemyController[] enemyControllers;
    protected string[] moveOrder;
    protected int activeCharacter;
    protected bool isFirstUpdate;
    protected CombatGui gui;
    protected int totalExpGain;
    protected int totalMoneyGain;

    protected bool IsObjectExists(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        
        return objects.Length > 0;
    }

    protected void CheckInputValues()
    {
        // Check common inputs
        if (MainCamera == null)
        {
            throw new UnityException("No main camera set.");
        }

        if (IconFolderPath == null)
        {
            throw new UnityException("No icon folder path set.");
        }

        if (CrosshairTexture == null)
        {
            throw new UnityException("No crosshair texture set.");
        }

        // Check inputs for party members
        if (PartyMemberController == null)
        {
            throw new UnityException("No party member controller set.");
        }

        if (MaxPartyMemberNumber < 1)
        {
            throw new UnityException("There should be at least one party member.");
        }

        if (PartyMembersName.Length < 1)
        {
            throw new UnityException("No party member set.");
        }
        else if (PartyMembersName.Length > MaxPartyMemberNumber)
        {
            throw new UnityException("Too many party members.");
        }

        for (int i = 0; i < PartyMembersName.Length; i++)
        {
            if (PartyMembersName[i] == null)
            {
                throw new UnityException("No party member name set in PartyMembersName[" + i + "].");
            }
        }

        if (PartyMemberDataFolderPath == null)
        {
            throw new UnityException("No party member data folder path set.");
        }

        if (PartyMemberGuiSkin == null)
        {
            throw new UnityException("No GUISkin for party members' GUI set.");
        }

        // Check inputs for enemies
        if (EnemyController == null)
        {
            throw new UnityException("No enemy controller set.");
        }

        if (EnemiesName.Length < 1)
        {
            throw new UnityException("No enemy set.");
        }

        for (int i = 0; i < EnemiesName.Length; i++)
        {
            if (EnemiesName[i] == null)
            {
                throw new UnityException("No enemy name set in EnemiesName[" + i + "].");
            }
        }

        if (EnemyDataFolderPath == null)
        {
            throw new UnityException("No enemy data folder path set.");
        }

        if (EnemyModelFolderPath == null)
        {
            throw new UnityException("No enemy model folder path set.");
        }
    }

    protected void InitEnvironment()
    {
        Instantiate(MainCamera);

        if (!IsObjectExists("Ground") && Ground != null)
        {
            Instantiate(Ground);
        }

        if (!IsObjectExists("FieldLight") && FieldLight != null)
        {
            Instantiate(FieldLight);
        }

        if (SkyboxMaterial != null)
        {
            RenderSettings.skybox = SkyboxMaterial;
        }
    }

    protected void InitPartyMembers()
    {
        partyMemberControllers = new CombatPartyMemberController[PartyMembersName.Length];

        GameObject controller;
        for (int i = 0; i < partyMemberControllers.Length; i++)
        {
            controller = (GameObject)Instantiate(PartyMemberController);
            partyMemberControllers[i] = controller.GetComponent<CombatPartyMemberController>();
            partyMemberControllers[i].PartyMemberName = PartyMembersName[i];
            partyMemberControllers[i].No = i;
            partyMemberControllers[i].DataFolderPath = PartyMemberDataFolderPath;
            partyMemberControllers[i].IconFolderPath = IconFolderPath;
            partyMemberControllers[i].DataFileType = DataFileType;
            partyMemberControllers[i].TotalPartyMemberNum = partyMemberControllers.Length;
            partyMemberControllers[i].MaxPartyMemberNum = MaxPartyMemberNumber;
            partyMemberControllers[i].GuiAlign = PartyMemberGuiAlign;
            partyMemberControllers[i].GuiSkin = PartyMemberGuiSkin;
            partyMemberControllers[i].CrosshairTexture = CrosshairTexture;
            partyMemberControllers[i].MasterController = this;
        }
    }

    protected void InitEnemies()
    {
        enemyControllers = new CombatEnemyController[EnemiesName.Length];
        float totalDistance = ENEMY_DISTANCE * (enemyControllers.Length - 1);
        float startX = -totalDistance / 2.0f;

        GameObject controller;
        for (int i = 0; i < enemyControllers.Length; i++)
        {
            controller = (GameObject)Instantiate(EnemyController);
            enemyControllers[i] = controller.GetComponent<CombatEnemyController>();
            enemyControllers[i].EnemyName = EnemiesName[i];
            enemyControllers[i].No = i;
            enemyControllers[i].DataFolderPath = EnemyDataFolderPath;
            enemyControllers[i].DataFileType = DataFileType;
            enemyControllers[i].ModelFolderPath = EnemyModelFolderPath;
            enemyControllers[i].MasterController = this;
            enemyControllers[i].ModelPosition = new Vector3(startX + ENEMY_DISTANCE * i, 1.5f, 0.0f);
        }
    }

    protected void SetControllers()
    {
        for (int i = 0; i < partyMemberControllers.Length; i++)
        {
            partyMemberControllers[i].EnemyControllers = enemyControllers;
        }

        for (int i = 0; i < enemyControllers.Length; i++)
        {
            enemyControllers[i].PartyMemberControllers = partyMemberControllers;
        }
    }

    protected void InitGui()
    {
        gui = gameObject.AddComponent<CombatGui>();
        gui.IsCombatEnd = false;
    }

    // Use this for initialization
    void Start()
    {
        CheckInputValues();

        InitEnvironment();
        InitPartyMembers();
        InitEnemies();

        SetControllers();

        InitGui();

        isFirstUpdate = true;
	}

    protected void CalculateMoveOrder()
    {
        moveOrder = CombatCalculator.GetMoveOrder(partyMemberControllers, enemyControllers);
        activeCharacter = 0;
    }

    protected bool TurnStart()
    {
        int no = int.Parse(moveOrder[activeCharacter].Substring(1, moveOrder[activeCharacter].Length - 1));
        switch (moveOrder[activeCharacter].Substring(0, 1))
        {
            case "P":
                if (partyMemberControllers[no].CurrentState == CombatBehaviours.CharacterState.Down)
                {
                    return false;
                }
                else
                {
                    partyMemberControllers[no].TurnStart();
                    return true;
                }
            case "E":
                if (enemyControllers[no].CurrentState == CombatBehaviours.CharacterState.Die)
                {
                    return false;
                }
                else
                {
                    enemyControllers[no].TurnStart();
                    return true;
                }
        }

        return false;
    }

    protected void FirstUpdate()
    {
        CalculateMoveOrder();
        TurnStart();

        isFirstUpdate = false;
    }

    protected void CheckInput()
    {
        if (moveOrder[activeCharacter].Substring(0, 1) == "P")
        {
            int no = int.Parse(moveOrder[activeCharacter].Substring(1, 1));
            if (!partyMemberControllers[no].Gui.IsAttacked && partyMemberControllers[no].Gui.IsSelectingEnemy)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100.0f))
                    {
                        for (int i = 0; i < enemyControllers.Length; i++)
                        {
                            if (enemyControllers[i].ModelController.gameObject.name == hit.collider.gameObject.name)
                            {
                                if (!enemyControllers[i].IsSelected)
                                {
                                    enemyControllers[i].IsSelected = true;
                                }
                                else
                                {
                                    enemyControllers[i].IsTarget = true;
                                }
                            }
                            else
                            {
                                enemyControllers[i].IsSelected = false;
                                enemyControllers[i].IsTarget = false;
                            }
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    for (int i = 0; i < enemyControllers.Length; i++)
                    {
                        enemyControllers[i].IsSelected = false;
                        enemyControllers[i].IsTarget = false;
                        partyMemberControllers[no].Gui.IsSelectingEnemy = false;
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update()
    {
        if (isFirstUpdate)
        {
            FirstUpdate();
        }
        else
        {
            CheckInput();
        }
	}

    public void TurnEnd()
    {
        bool result = false;
        while (!result)
        {
            activeCharacter++;
            if (activeCharacter >= moveOrder.Length)
            {
                CalculateMoveOrder();
            }

            result = TurnStart();
        }
    }

    protected void CalculateResult()
    {
        totalExpGain = 0;
        totalMoneyGain = 0;
        for (int i = 0; i < enemyControllers.Length; i++)
        {
            totalExpGain += enemyControllers[i].Data.Exp;
            totalMoneyGain += enemyControllers[i].Data.Money;
        }

        List<string> levelUpPartyMembers = new List<string>(partyMemberControllers.Length);
        List<int> levelUpPartyMembersOldLv = new List<int>(partyMemberControllers.Length);
        List<int> levelUpPartyMembersNewLv = new List<int>(partyMemberControllers.Length);
        for (int i = 0; i < partyMemberControllers.Length; i++)
        {
            if (partyMemberControllers[i].CurrentState != CombatBehaviours.CharacterState.Down && partyMemberControllers[i].Data.NextExp - totalExpGain <= 0)
            {
                levelUpPartyMembers.Add(partyMemberControllers[i].PartyMemberName);
                levelUpPartyMembersOldLv.Add(partyMemberControllers[i].Data.Lv);

                int nextExp = partyMemberControllers[i].Data.NextExp - totalExpGain;
                while (nextExp <= 0)
                {
                    nextExp += (int)(100 * (partyMemberControllers[i].Data.Lv / 2.0f));
                    partyMemberControllers[i].Data.Lv++;
                }
                partyMemberControllers[i].Data.NextExp = nextExp;

                levelUpPartyMembersNewLv.Add(partyMemberControllers[i].Data.Lv);
            }
        }

        gui.GenerateResult(totalExpGain, totalMoneyGain, levelUpPartyMembers.ToArray(), levelUpPartyMembersOldLv.ToArray(), levelUpPartyMembersNewLv.ToArray());
    }

    public void CombatEnd(CombatBehaviours.Force winForce)
    {
        gui.IsCombatEnd = true;

        switch (winForce)
        {
            case CombatBehaviours.Force.PlayerParty:
                CalculateResult();
                gui.IsPlayerPartyWin = true;
                break;
            case CombatBehaviours.Force.Enemy:
                gui.IsEnemyWin = true;
                break;
        }
    }
}