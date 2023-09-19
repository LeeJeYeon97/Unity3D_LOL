using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;

public class MinionController : BaseController
{
    [SerializeField]
    GameObject nexusTarget;
    

    public Transform firePos;

    string bulletKey;

    [SerializeField]
    Collider[] cols;

    Vector3 minionSpawnPos;
    
    protected override void Awake()
    {
        base.Awake();
    }
    private void OnEnable()
    {
        if (hpBar != null)
            hpBar.gameObject.SetActive(true);

        State = Define.State.MOVE;
        _collider.enabled = true;
        gameObject.SetActive(true);

        switch(type.team)
        {
            case Define.Team.BLUE:
                nexusTarget = GameObject.FindGameObjectWithTag("RED_NEXUS");
                minionSpawnPos = GameObject.FindGameObjectWithTag("BLUE_SPAWN").transform.position;
                break;
            case Define.Team.RED:
                nexusTarget = GameObject.FindGameObjectWithTag("BLUE_NEXUS");
                minionSpawnPos = GameObject.FindGameObjectWithTag("RED_SPAWN").transform.position;
                break;
        }
        Target = nexusTarget;
        transform.position = minionSpawnPos;
        agent.enabled = true;
        agent.autoBraking = false;
        agent.SetDestination(Target.transform.position);

        stat.curHp = stat.maxHp;
    }
    protected override void Start()
    {
        if (type.team == Define.Team.RED)
        {
            layer = (1 << (int)Define.Layer.BLUE_MINION | 1 << (int)Define.Layer.PLAYER | 1 << (int)Define.Layer.BLUE_TURRET);
        }
        else if (type.team == Define.Team.BLUE)
        {   
            layer = (1 << (int)Define.Layer.RED_MINION | 1 << (int)Define.Layer.PLAYER | 1 << (int)Define.Layer.RED_TURRET | 1<<(int)Define.Layer.BOT);
        }

        switch (type.minionType)
        {
            case Define.MinionType.MELEE:
                statData = Managers.Data.GetStatData("Melee");
                break;
            case Define.MinionType.RANGED:
                statData = Managers.Data.GetStatData("Ranged");
                bulletKey = "Bullet_1";
                break;
            case Define.MinionType.SEIGE:
                statData = Managers.Data.GetStatData("Seige");
                bulletKey = "Bullet_2";
                break;
            case Define.MinionType.NONE:
                Debug.Log("MinionType is None!");
                break;
        }
        stat.SetStat(statData);

        base.Start();
        
    }
    protected override void Update()
    {
        base.Update();
        if (Target == null && state != Define.State.DIE)
            ScanTarget();
    }
    protected override void UpdateMoving()
    {
        animator.Play("RUN");
        ScanTarget();

        if(Target !=null)
        {
            if (agent.remainingDistance <= stat.attackRange)
            {
                agent.isStopped = true;
                state = Define.State.ATTACK;
                return;
            }
        }
    }
    protected override void UpdateAttack()
    {
        Stat targetStat = Target.GetComponent<Stat>();
        float dir = (Target.transform.position - transform.position).magnitude;
        if (dir > stat.attackRange || targetStat.curHp <= 0)
        {
            state = Define.State.MOVE;
            Target = null;
            return;       
        }
        transform.LookAt(Target.transform);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RUN"))
        {
            animator.Play("ATTACK");
        }
    }
    void OnHitEvent()
    {
        if (Target == null) return;
        
        Stat targetStat = Target.GetComponent<Stat>();
        
        switch(type.minionType)
        {
            case Define.MinionType.MELEE:
                Target.GetComponent<BaseController>().OnDamaged(stat.attack);
                break;
            case Define.MinionType.RANGED:
                Managers.Bullet.Fire(bulletKey, Target, stat.attack, firePos);
                break;
            case Define.MinionType.SEIGE:
                Managers.Bullet.Fire(bulletKey,Target,stat.attack,firePos);
                break;
        }
        
    }
    protected override void UpdateDie()
    {
        animator.Play("DIE");
        
        agent.enabled = false;
        hpBar.SetActive(false);
        _collider.enabled = false;
        Target = null;
        

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DIE") 
            &&animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            gameObject.SetActive(false);
        }

    }
    public override void OnDamaged(float attackPower, GameObject hitObject = null)
    {
        base.OnDamaged(attackPower, hitObject);
    }


    void ScanTarget()
    {
        cols = Physics.OverlapSphere(transform.position, 5.0f,layer);

        if(cols.Length <= 1 && Target == null) 
        {
            if (cols.Length == 0 || cols[0].GetComponent<IsType>().team == GetComponent<IsType>().team)
            {
                Target = nexusTarget;
                agent.destination = Target.transform.position;
                agent.isStopped = false;
                return;
            }
            
        }
        switch (type.team)
        {
            case Define.Team.RED:
                foreach(Collider col in cols)
                {
                    if (col.gameObject.GetComponent<IsType>().team == Define.Team.BLUE)
                    {
                        float distance = (col.transform.position - transform.position).magnitude;
                        if(col.GetComponent<Stat>().curHp > 0)
                        {
                            Target = col.gameObject;
                            agent.destination = Target.transform.position;
                            agent.isStopped = false;
                            return;
                        }
                    }
                }
                break;
            case Define.Team.BLUE:
                foreach (Collider col in cols)
                {
                    if (col.gameObject.GetComponent<IsType>().team == Define.Team.RED)
                    {
                        float distance = (col.transform.position - transform.position).magnitude;
                        if (col.GetComponent<Stat>().curHp > 0)
                        {
                            Target = col.gameObject;
                            agent.destination = Target.transform.position;
                            agent.isStopped = false;
                            return;
                        }
                    }
                }
                break;
        }
    }
}
