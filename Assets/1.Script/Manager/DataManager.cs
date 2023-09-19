using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StatData
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
}

public struct SkillData
{
    public string key;
    public float qSkillCoolDown;
    public float wSkillCoolDown;
    public float eSkillCoolDown;
    public float rSkillCoolDown;
    public float qSkillDuration;
    public float wSkillDuration;
    public float eSkillDuration;
    public float rSkillDuration;
    public float qMp;
    public float wMp;
    public float eMp;
    public float rMp;
}

public struct SoundData
{
    public string key;

    public string dieSound;
    public string moveSound_1;
    public string moveSound_2;
    public string moveSound_3;
    
}
public class DataManager
{
    private Dictionary<string, StatData> stats = new Dictionary<string, StatData>();
    private Dictionary<string, SkillData> skillStats = new Dictionary<string, SkillData>();

    public StatData GetStatData(string key)
    {
        return stats[key];
    }
    public SkillData GetSkillData(string key)
    {
        return skillStats[key];
    }

    public void LoadData()
    {
        LoadStatData();
        LoadSkillData();
    }
    private void LoadStatData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/Stats");

        // \r 지워주기
        string temp = textAsset.text.Replace("\r\n", "\n");

        // 줄바꿈으로 나누어준다
        string[] row = temp.Split('\n');

        // 첫번째행은 무시
        for(int i = 1; i<row.Length; i++)
        {
            if (row[i].Length == 0)
                break;

            // csv기 때문에 쉼표로 구분
            string[] data = row[i].Split(',');

            // new해서 초기화를 안해줄때는 모든 변수를 초기화 해줄때만
            StatData statData;

            statData.key = data[0];
            statData.type = data[1];
            statData.maxHp = float.Parse(data[2]);
            statData.curHp = float.Parse(data[3]);
            statData.maxMp = float.Parse(data[4]);
            statData.curMp = float.Parse(data[5]);
            statData.attack = float.Parse(data[6]);
            statData.defense = float.Parse(data[7]);
            statData.moveSpeed = float.Parse(data[8]);
            statData.attackRange = float.Parse(data[9]);
            statData.attackSpeed = float.Parse(data[10]);
            statData.level = int.Parse(data[11]);
            statData.exp = int.Parse(data[12]);
            statData.gold = int.Parse(data[13]);
            statData.dieGold = int.Parse(data[14]);
            if(!stats.ContainsKey(statData.key))
                stats.Add(statData.key, statData);

        }
    }
    private void LoadSkillData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/SkillStat");

        // \r 지워주기
        string temp = textAsset.text.Replace("\r\n", "\n");

        // 줄바꿈으로 나누어준다
        string[] row = temp.Split('\n');

        // 첫번째행은 무시
        for (int i = 1; i < row.Length; i++)
        {
            if (row[i].Length == 0)
                break;

            // csv기 때문에 쉼표로 구분
            string[] data = row[i].Split(',');

            // new해서 초기화를 안해줄때는 모든 변수를 초기화 해줄때만
            SkillData skillData;

            skillData.key = data[0];
            skillData.qSkillCoolDown = float.Parse(data[1]);
            skillData.wSkillCoolDown = float.Parse(data[2]);
            skillData.eSkillCoolDown = float.Parse(data[3]);
            skillData.rSkillCoolDown = float.Parse(data[4]);
            skillData.qSkillDuration = float.Parse(data[5]);
            skillData.wSkillDuration = float.Parse(data[6]);
            skillData.eSkillDuration = float.Parse(data[7]);
            skillData.rSkillDuration = float.Parse(data[8]);
            skillData.qMp= float.Parse(data[9]);
            skillData.wMp= float.Parse(data[10]);
            skillData.eMp= float.Parse(data[11]);
            skillData.rMp= float.Parse(data[12]);
            if (!skillStats.ContainsKey(skillData.key))
                skillStats.Add(skillData.key, skillData);
            
        }
    }
}
