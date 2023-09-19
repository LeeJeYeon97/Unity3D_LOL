using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public string key;
    public string type;
    public float maxHp;
    public float curHp;
    public float maxMp;
    public float curMp;
    public float attack;
    public float defense;
    public float moveSpeed;
    public float attackRange;
    public float attackSpeed;
    public int level;
    public int exp;
    public int gold;
    public int dieGold;
    public void SetStat(StatData statData)
    {

        key = statData.key;
        type = statData.type;
        maxHp = statData.maxHp;
        curHp = statData.curHp;
        maxMp = statData.maxMp;
        curMp = statData.curMp;
        attack = statData.attack;
        defense = statData.defense;
        moveSpeed = statData.moveSpeed;
        attackRange = statData.attackRange;
        attackSpeed = statData.attackSpeed;
        level = statData.level;
        exp = statData.exp;
        gold = statData.gold;
        dieGold = statData.dieGold;
    }
}
