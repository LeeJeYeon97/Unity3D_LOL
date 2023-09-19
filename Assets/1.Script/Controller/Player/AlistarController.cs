using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class AlistarController : BaseController
{

    public GameObject hitEffect;

    bool wSpellReady = false;
    float wSpellRange = 5.0f;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        statData = Managers.Data.GetStatData("Alistar");

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

        OnMouseClicked();
        OnKeyboard();
    }
    protected override void OnKeyboard()
    {
        if (skill.IsSpell_Q) return;
        if (skill.IsSpell_W) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            skill.Active_q();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            wSpellReady = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            skill.Active_e();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            skill.Active_r();
        }
    }
    protected override void OnMouseClicked()
    {
        if (skill.IsSpell_Q) return;
        if (skill.IsSpell_W) return;

        if (wSpellReady && Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layer))
            {
                if (hit.transform.gameObject.layer != (int)Define.Layer.BOT) return;

                float distance = (hit.transform.position - transform.position).magnitude;

                if (distance > wSpellRange) return;

                Target = hit.transform.gameObject;
                state = Define.State.IDLE;
                skill.Active_w();
                wSpellReady = false;
            }
        }
        base.OnMouseClicked();
    }
    protected override void UpdateIdle()
    {
        if (skill.IsSpell_Q) return;
        if (skill.IsSpell_W) return;
        if (skill.IsSpell_E) return;
        if (skill.IsSpell_R)
            animator.Play("IDLE_ANGRY");
        else
            animator.Play("IDLE");
    }
    protected override void UpdateMoving()
    {
        if (skill.IsSpell_Q) return;
        if (skill.IsSpell_E) return;
        if (skill.IsSpell_W) return;

        Vector3 dir = destPos - transform.position;

        if (skill.IsSpell_R) animator.Play("RUN_ANGRY");
        else animator.Play("RUN");

        // Attack 호출
        if (target != null)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, stat.attackRange, layer))
            {
                state = Define.State.ATTACK;
                return;
            }
        }
        // 목적지 도착
        if (dir.magnitude <= 0.1f)
        {
            state = Define.State.IDLE;
        }
        agent.Move(dir.normalized * stat.moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 30 * Time.deltaTime);
    }
    protected override void UpdateAttack()
    {
        if (target == null || target.GetComponent<Stat>().curHp <= 0)
        {
            state = Define.State.IDLE;
            return;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("IDLE") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("RUN"))
            animator.Play("ATTACK");

        transform.LookAt(target.transform);

    }
    void OnHitEvent()
    {
        if (target == null) return;

        if (Target.GetComponent<Stat>().curHp > 0)
        {
            Target.GetComponent<BaseController>().OnDamaged(stat.attack, this.gameObject);
            GameObject go = Instantiate(hitEffect, target.transform.position, Camera.main.transform.rotation);
            Destroy(go, hitEffect.GetComponent<ParticleSystem>().main.duration);
        }
    }

    protected override void UpdateDie()
    {
        animator.Play("DIE");

        agent.SetDestination(transform.position);

    }
    public override void OnDamaged(float attackPower, GameObject hitObject = null)
    {
        // 받는 피해 감소 시키기
        base.OnDamaged(attackPower, hitObject);
    }
}
