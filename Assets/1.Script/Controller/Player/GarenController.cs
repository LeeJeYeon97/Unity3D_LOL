using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class GarenController : BaseController
{
    
    public GameObject hitEffect;

    bool isSpell_4_ready = false;
    float spell_4_range = 5.0f;
    
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        statData = Managers.Data.GetStatData("Garen");

        stat.SetStat(statData);

        if (type.team == Define.Team.BLUE)
            layer = 1 << (int)Define.Layer.RED_TURRET | 1 << (int)Define.Layer.BOT | 1 << (int)Define.Layer.GROUND
                | 1 << (int)Define.Layer.RED_MINION;
        else if (type.team == Define.Team.RED)
            layer = 1 << (int)Define.Layer.BLUE_TURRET | 1 << (int)Define.Layer.BOT | 1 << (int)Define.Layer.GROUND
                | 1 << (int)Define.Layer.BLUE_MINION;

        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!skill.IsSpell_R)
        {
            OnMouseClicked();
            OnKeyboard();
        }
    }

    protected override void OnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            skill.Active_q();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            skill.Active_w();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            skill.Active_e();    
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (skill.IsSpell_E) return;
            // ±Ã±Ø±â Ãß°¡
            isSpell_4_ready = true;
        }
    }
    protected override void OnMouseClicked()
    {

        if (isSpell_4_ready && Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layer))
            {
                if (hit.transform.gameObject.layer != (int)Define.Layer.BOT) return;

                float distance = (hit.transform.position - transform.position).magnitude;

                if (distance > spell_4_range) return;

                Target = hit.transform.gameObject;
                state = Define.State.IDLE;
                skill.Active_r();
                isSpell_4_ready = false;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, 100.0f, layer))
            {
                destPos = hit.point;

                if (hit.transform.gameObject.layer == (int)Define.Layer.GROUND)
                {
                    Target = null;
                    state = Define.State.MOVE;
                }
                else
                {
                    switch(type.team)
                    {
                        case Define.Team.BLUE:
                            if (hit.transform.gameObject.GetComponent<IsType>().team == Define.Team.RED)
                                Target = hit.transform.gameObject;
                            break;
                        case Define.Team.RED:
                            if (hit.transform.gameObject.GetComponent<IsType>().team == Define.Team.BLUE)
                                Target = hit.transform.gameObject;
                            break;
                    }
                    if(Target != null)
                    {
                        destPos = Target.transform.position;
                        state = Define.State.MOVE;
                    }
                }
            }
        }
    }
    protected override void UpdateIdle()
    {
        if (skill.IsSpell_R)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_SPELL_4") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {

                skill.IsSpell_R = false;
            }
            else return;
        }
        if (skill.IsSpell_E)
        {
            animator.Play("ATTACK_SPELL_3");
        }
        
        else
            animator.Play("IDLE");
    }
    protected override void UpdateMoving()
    {
        if (skill.IsSpell_R) return;
        Vector3 dir = destPos - transform.position;

        // Anim
        if (skill.IsSpell_E)
        {
            animator.Play("ATTACK_SPELL_3");
        }
        else if(skill.IsSpell_Q && !skill.IsSpell_E)
        {
            animator.Play("RUN_SPELL_1");
        }
        else if(!skill.IsSpell_Q && !skill.IsSpell_E)
        {
            animator.Play("RUN");
        }
        // Attack È£Ãâ
        if(target != null)
        {
            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir,stat.attackRange,layer))
            {
                state = Define.State.ATTACK;
                return;
            }
        }
        // ¸ñÀûÁö µµÂø
        if(dir.magnitude <= 0.1f)
        {
            state = Define.State.IDLE;
        }
        agent.Move(dir.normalized * stat.moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 30 *Time.deltaTime);

    }
    protected override void UpdateAttack()
    {
        if (skill.IsSpell_R)
            return;
        
        if (target == null || target.GetComponent<Stat>().curHp <= 0)
        {
            state = Define.State.IDLE;
            return;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("IDLE") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("RUN"))
            animator.Play("ATTACK");

        transform.LookAt(target.transform);

        if (skill.IsSpell_E)
        {
            animator.Play("ATTACK_SPELL_3");
            return;
        }
        else if (skill.IsSpell_Q && !skill.IsSpell_E)
        {
            animator.Play("ATTACK_SPELL_1");
        }
       
    }
    void OnHitEvent()
    {
        if (target == null) return;

        if(Target.GetComponent<Stat>().curHp > 0)
        {
            Target.GetComponent<BaseController>().OnDamaged(stat.attack, this.gameObject);
            GameObject go = Instantiate(hitEffect, target.transform.position, Camera.main.transform.rotation);
            Destroy(go, hitEffect.GetComponent<ParticleSystem>().main.duration);
        }
        
        if(skill.IsSpell_Q)
        {
            skill.IsSpell_Q = false;
            SetStat();
        }
    }

    protected override void UpdateDie()
    {
        animator.Play("DIE");

        agent.SetDestination(transform.position);
        
    }

    void SetStat()
    {
        stat.attack = statData.attack;
        stat.moveSpeed = statData.moveSpeed;
    }

}
