using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlistarSkill : BaseSkill
{

    private float qSkillRange = 5.0f;
    public float qSkillPower = 10.0f;
    public GameObject qSkillPrefab;

    public float wSkillSpeed = 10.0f;
    public float wSkillPower= 100.0f;
    private bool wSkillHit = false;


    public GameObject eSkillPrefab;
    private GameObject eSkill;
    public float eSkillHeal = 50.0f;

    public GameObject rSkillPrefab;
    public GameObject rSkillEffectPrefab;
    private GameObject rSkill;
    private GameObject rSkillEffect;

    [SerializeField]
    Collider[] cols;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        GetSkill("Alistar");
        stat = gameObject.GetComponent<Stat>();
        statData = Managers.Data.GetStatData("Alistar");

    }

    void Update()
    {
        if(IsSpell_Q)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("SPELL_1")
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                IsSpell_Q = false;
            }
        }

        if(IsSpell_W)
        {
            Vector3 dir = target.transform.position - transform.position;

            transform.position += dir.normalized * Time.deltaTime * wSkillSpeed;
            if (dir.magnitude <= 1.5f)
            {
                wSkillSpeed = 0;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("SPELL_2")
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    IsSpell_W = false;
                }
                if(!wSkillHit)
                {
                    target.GetComponent<BaseController>().OnDamaged(wSkillPower, gameObject);
                    wSkillHit = true;
                }
                    
                
                // 타겟 날려보내기 구현.

            }
        }
        if(IsSpell_E)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("SPELL_3")
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                IsSpell_E = false;
            }
        }
        if(eSkill !=null)
        {
            eSkill.transform.position = transform.position;
            eSkill.transform.rotation = Camera.main.transform.rotation;
        }

        if(rSkill != null)
        {
            rSkillEffect.transform.position = transform.position;
            rSkill.transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
            
            rSkill.transform.Rotate(Vector3.forward, Time.deltaTime * 50);
        }
    }

    public override void Active_q() 
    {
        if (q_spell_cool) return;

        //StartCoroutine(Q_Spell_CoolDown());

        IsSpell_Q = true;
        animator.Play("SPELL_1");

        int layer = gameObject.GetComponent<BaseController>().MyLayer;
        IsType type = gameObject.GetComponent<IsType>();

        cols = Physics.OverlapSphere(transform.position, qSkillRange,layer);

        if (cols.Length <= 0) return;

        foreach(Collider col in cols)
        {
            if (col.gameObject.layer == (int)Define.Layer.GROUND ||
                col.gameObject.layer == (int)Define.Layer.RED_TURRET ||
                col.gameObject.layer == (int)Define.Layer.BLUE_TURRET)
                continue;

            col.gameObject.GetComponent<BaseController>().State = Define.State.AIRBONE;
        }
    }
    public void Q_Skill()
    {
        Vector3 pos = transform.position;
        Quaternion rot = Quaternion.Euler(-90,0, 0);
        GameObject go = Instantiate(qSkillPrefab,pos,rot);
        //go.transform.position = transform.position;
        
        Destroy(go, 3.0f);
    }
    public override void Active_w()
    {
        if (w_spell_cool) return;

        StartCoroutine(W_Spell_CoolDown());

        IsSpell_W = true;
        wSkillHit = false;
        wSkillSpeed = 10.0f;
        animator.Play("SPELL_2");
        target = gameObject.GetComponent<BaseController>().Target;
        transform.LookAt(target.transform);

    }
    public override void Active_e() 
    {
        if (e_spell_cool) return;

        IsSpell_E = true;
        StartCoroutine(E_Spell_CoolDown());
        animator.Play("SPELL_3");

        eSkill = Instantiate(eSkillPrefab, transform.position, Quaternion.identity);
        float pstime = eSkill.GetComponent<ParticleSystem>().main.duration;
        Destroy(eSkill, pstime);
        stat.curHp += eSkillHeal;
    }
    
    
    public override void Active_r() 
    {
        if(r_spell_cool) return;
        StartCoroutine(rDuration());
    }
    IEnumerator rDuration()
    {
        IsSpell_R = true;
        rSkill = Instantiate(rSkillPrefab);
        rSkillEffect = Instantiate(rSkillEffectPrefab);
        StartCoroutine(R_Spell_CoolDown());
        animator.Play("SPELL_4");

        yield return new WaitForSeconds(skillData.rSkillDuration);
        IsSpell_R = false;
        Destroy(rSkill);
        Destroy(rSkillEffect);
    }
}
