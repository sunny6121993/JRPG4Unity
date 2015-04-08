using UnityEngine;
using System.Collections;

public class PartyMemberData
{
    protected string name;
    protected int lv;
    protected int nextExp;
    protected int hp;
    protected int mp;
    protected int atk;
    protected int def;
    protected int spd;

    protected int maxHp;
    protected int maxMp;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Lv
    {
        get { return lv; }
        set { lv = Mathf.Max(value, 1); }
    }

    public int NextExp
    {
        get { return nextExp; }
        set { nextExp = Mathf.Max(value, 1); }
    }
    
    public int Hp
    {
        get { return hp; }
        set { hp = Mathf.Max(value, 0); }
    }

    public int Mp
    {
        get { return mp; }
        set { mp = Mathf.Max(value, 0); }
    }

    public int Atk
    {
        get { return atk; }
        set { atk = Mathf.Max(value, 1); }
    }

    public int Def
    {
        get { return def; }
        set { def = Mathf.Max(value, 1); }
    }

    public int Spd
    {
        get { return spd; }
        set { spd = Mathf.Max(value, 1); }
    }

    public int MaxHp
    {
        get { return maxHp; }
        set { maxHp = Mathf.Max(value, Hp); }
    }

    public int MaxMp
    {
        get { return maxMp; }
        set { maxMp = Mathf.Max(value, Mp); }
    }

    public PartyMemberData()
    {
        Name = "Default";
        Lv = 1;
        NextExp = 1;
        MaxHp = Hp = 10;
        MaxMp = Mp = 10;
        Atk = 1;
        Def = 1;
        Spd = 1;
    }

    public PartyMemberData(string name, int lv, int nextExp, int hp, int mp, int atk, int def, int spd)
    {
        Name = name;
        Lv = lv;
        NextExp = nextExp;
        MaxHp = Hp = hp;
        MaxMp = Mp = mp;
        Atk = atk;
        Def = def;
        Spd = spd;
    }
}