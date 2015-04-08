using UnityEngine;
using System.Collections;

public class EnemyData
{
	protected string name;
    protected int hp;
    protected int mp;
    protected int atk;
    protected int def;
    protected int spd;
    protected int exp;
    protected int money;

    protected int maxHp;
    protected int maxMp;

    public string Name
    {
        get { return name; }
        set { name = value; }
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

    public int Exp
    {
        get { return exp; }
        set { exp = Mathf.Max(value, 1); }
    }

    public int Money
    {
        get { return money; }
        set { money = Mathf.Max(value, 1); }
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

    public EnemyData()
    {
        Name = "Default";
        MaxHp = Hp = 10;
        MaxMp = Mp = 10;
        Atk = 1;
        Def = 1;
        Spd = 1;
        Exp = 1;
        Money = 1;
    }

    public EnemyData(string name, int hp, int mp, int atk, int def, int spd, int exp, int money)
    {
        Name = name;
        MaxHp = Hp = hp;
        MaxMp = Mp = mp;
        Atk = atk;
        Def = def;
        Spd = spd;
        Exp = exp;
        Money = money;
    }
}