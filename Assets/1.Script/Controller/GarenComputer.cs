using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class GarenComputer : BaseController
{
    public GameObject hitEffect;

    private GameObject nexusTarget;

    [SerializeField]
    Collider[] cols;

    float scanRange = 7.0f;
    float skillTime = 3.0f;
    
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        Init();
        
        base.Start();
    }
    private void Init()
    {

        statData = Managers.Data.GetStatData("Garen");

        stat.SetStat(statData);

        if (type.team == Define.Team.BLUE)
        {
            layer = 1 << (int)Define.Layer.RED_TURRET | 1 << (int)Define.Layer.PLAYER | 1 << (int)Define.Layer.RED_MINION;
            nexusTarget = GameObject.FindGameObjectWithTag("RED_NEXUS");
            
        }
        else if (type.team == Define.Team.RED)
        {
            layer = 1 << (int)Define.Layer.BLUE_TURRET | 1 << (int)Define.Layer.PLAYER | 1 << (int)Define.Layer.BLUE_MINION;
            nexusTarget = GameObject.FindGameObjectWithTag("BLUE_NEXUS");
        }

        Target = nexusTarget;
        agent.destination = Target.transform.position;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdateIdle()
    {
        if (state == Define.State.DIE) return;

        if (isAirbone) return;

        if (skill.IsSpell_E)
            animator.Play("ATTACK_SPELL_3");
        
        else animator.Play("IDLE");

        if(Target !=null)
        {
            state = Define.State.MOVE;
        }
        if (Target == null)
        {
            ScanTarget();
        }
    }

    private void ScanTarget()
    {
        if (state == Define.State.DIE) return;

        cols = Physics.OverlapSphere(transform.position, scanRange,layer);

        if(cols.Length <=0)
        {
            Target = nexusTarget;
            agent.destination = Target.transform.position;
            agent.isStopped = false;
            return;
        }
        switch (type.team)
        {
            case Define.Team.RED:
                foreach (Collider col in cols)
                {
                    if (col.gameObject.GetComponent<IsType>().team == Define.Team.BLUE)
                    {   
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
            case Define.Team.BLUE:
                foreach (Collider col in cols)
                {
                    if (col.gameObject.GetComponent<IsType>().team == Define.Team.RED)
                    {
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
    private void RandomSkillActive()
    {
        int rand = Random.Range(0, 4);
        switch(rand)
        {
            case 0:
                skill.Active_q();
                break;
            case 1:
                skill.Active_w();
                break;
            case 2:
                skill.Active_e();
                break;
            case 3:
                skill.Active_r();
                break;
        }
    }
    protected override void UpdateMoving()
    {
        if (state == Define.State.DIE) return;
        if (isAirbone) return;
        ScanTarget();

        if (skill.IsSpell_E)
        {
            animator.Play("ATTACK_SPELL_3");
        }
        else if (skill.IsSpell_Q && !skill.IsSpell_E)
        {
            animator.Play("RUN_SPELL_1");
        }
        else if (!skill.IsSpell_Q && !skill.IsSpell_E)
        {
            animator.Play("RUN");
        }

        if (Target != null)
        {
            Vector3 dir = Target.transform.position - transform.position;
            if (dir.magnitude <= stat.attackRange)
            {
                agent.isStopped = true;
                state = Define.State.ATTACK;
                return;
            }
            else
            {
                Vector3 distance = Target.transform.position - transform.position;
                if(distance.magnitude > scanRange)
                {
                    ScanTarget();
                }
            }
        }
    }
    protected override void UpdateAttack()
    {
        Vector3 dir = Target.transform.position - transform.position;
        if (state == Define.State.DIE) return;
        if (isAirbone) return;

        if(Target.GetComponent<Stat>().curHp <=0)
        {
            Target = null;
            state = Define.State.IDLE;
            return;

        }
        if (Target.gameObject.tag == "PLAYER")
        {
            
            skillTime -= Time.deltaTime;
            if(skillTime<=0)
            {
                RandomSkillActive();
                skillTime = 3.0f;
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("IDLE") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("RUN"))
            animator.Play("ATTACK");

        transform.LookAt(Target.transform);

        
        if (dir.magnitude >=stat.attackRange)
        {
            
            state = Define.State.MOVE;
            Target = null;
            return;
        }

        if (skill.IsSpell_E)
        {
            animator.Play("ATTACK_SPELL_3");
            return;
        }
        else if (skill.IsSpell_Q && !skill.IsSpell_E)
        {
            //audio.PlayOneShot(voices[5]);
            //audio.PlayOneShot(spellSounds[1]);
            animator.Play("ATTACK_SPELL_1");
        }
    }
    protected override void UpdateDie()
    {
        animator.Play("DIE");
        if (!dieSoundPlaying)
        {
            Managers.Audio.PlayClip("GarenSound/GarenDie", _audio);
            dieSoundPlaying = true;
        }
        base.UpdateDie();
    }
    public override void OnDamaged(float attackPower, GameObject hitObject = null)
    {
        if (hitObject!= null && hitObject.tag == "PLAYER")
            Target = hitObject;

        base.OnDamaged(attackPower, hitObject);
    }
    void OnHitEvent()
    {
        if (target == null) return;

        if (Target.GetComponent<Stat>().curHp > 0)
        {
            Target.GetComponent<BaseController>().OnDamaged(stat.attack, this.gameObject);
            Managers.Audio.PlayClip("GarenSound/가렌피격소리", _audio,3.0f);
            GameObject go = Instantiate(hitEffect, target.transform.position, Camera.main.transform.rotation);
            Destroy(go, hitEffect.GetComponent<ParticleSystem>().main.duration);
        }
        if (skill.IsSpell_Q)
        {
            skill.IsSpell_Q = false;
            Managers.Audio.PlayClip("GarenSound/GarenQHit", _audio);
            state = Define.State.IDLE;
            SetStat();
        }
    }
    void SetStat()
    {
        stat.attack = statData.attack;
        stat.moveSpeed = statData.moveSpeed;
    }
}
