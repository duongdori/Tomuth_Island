using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWolf
{
    private string privateKey;
    private AnimalPositions animalPositions;
    string keyTag;
    private int level;
    // hit points
    private float HP;
    // experience point
    private int EXP;
    // strength
    private int STR;
    // intelligence
    private int INT;
    // vitality
    private int VIT;
    // agility
    private int AGI;
    // dexterity
    private int DEX;

    public void SetModel(string privateKey, string keyTag, int level, int STR, int INT, int VIT, int AGI, int DEX)
    {
        this.privateKey = privateKey;
        this.keyTag = keyTag;
        this.level = level;
        this.STR += STR + 1;
        this.INT += INT + 1;
        this.VIT += VIT + 1;
        this.AGI += AGI + 1;
        this.DEX += DEX + 1;
        EXP = (this.level ^ 3) * 10;
        HP = 100 + (this.VIT * 10);
    }

    public string GetPrivateKey()
    {
        return privateKey;
    }

    public AnimalPositions GetAnimalPositions()
    {
        return animalPositions;
    }

    public string GetTag()
    {
        return keyTag;
    }

    public void SetAnimalPositions(AnimalPositions animalPositions)
    {
        this.animalPositions = animalPositions;
    }

    public int GetLevel()
    {
        return level;
    }

    public void LevelUp()
    {
        level++;
        EXP = (level ^ 3) * 10;
        HP = 100 + (this.VIT * 10);
    }

    public float GetHP()
    {
        return HP;
    }

    public void SetHealing(int healing)
    {
        if (HP + healing <= 100 + (VIT * 10))
        {
            HP += healing;
        }
        else
        {
            HP = 100 + (VIT * 10);
        }
    }

    public bool CheckHealth()
    {
        if (HP == 100 + (VIT * 10))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TakeDamge(float damge)
    {
        if (HP - damge <= 0)
        {
            return false;
        }
        else
        {
            HP -= damge;
            return true;
        }
    }

    public int GetEXP()
    {
        return EXP;
    }

    public int GetSTR()
    {
        return STR * 10;
    }

    public void SetSTR(int STR)
    {
        this.STR += STR;
    }

    public int GetINT()
    {
        return INT;
    }

    public void SetINT(int INT)
    {
        this.INT += INT;
    }

    public int GetVIT()
    {
        return VIT;
    }

    public void SetVIT(int VIT)
    {
        this.VIT += VIT;
        HP += (this.VIT * 10);
    }

    public int GetAGI()
    {
        return AGI;
    }

    public void SetAGI(int AGI)
    {
        this.AGI += AGI;
    }

    public int GetDEX()
    {
        return DEX;
    }

    public void SetDEX(int DEX)
    {
        this.DEX += DEX;
    }
}
