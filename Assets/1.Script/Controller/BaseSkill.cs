using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    protected bool q_spell_cool = false;
    protected bool w_spell_cool = false;
    protected bool e_spell_cool = false;
    protected bool r_spell_cool = false;
    protected bool d_spell_cool = false;
    protected bool f_spell_cool = false;

    private float dSkillCoolTime = 90.0f;
    private float fSkillCoolTime = 120.0f;

    public GameObject dSkillEffectPrefab;
    private GameObject dSkillEffect;
    public GameObject fSkillEffectPrefab;

    private bool isSpell_q = false;
    public bool IsSpell_Q
    {
        get { return isSpell_q; }
        set { isSpell_q=value; }
    }
    private bool isSpell_w = false;
    public bool IsSpell_W
    {
        get { return isSpell_w; }
        set { isSpell_w=value; }
    }

    private bool isSpell_e = false;
    public bool IsSpell_E
    {
        get { return isSpell_e; }
        set{ isSpell_e = value;}
    }
    private bool isSpell_r = false;
    public bool IsSpell_R
    {
        get { return isSpell_r; }
        set { isSpell_r = value;}
    }
    private bool isSpell_d = false;
    public bool IsSpell_D
    {
        get { return isSpell_d; }
        set { isSpell_d = value; }
    }
    private bool isSpell_f = false;
    public bool IsSpell_F
    {
        get { return isSpell_f; }
        set { isSpell_f = value; }
    }

    protected SkillData skillData;
    protected StatData statData;
    protected Stat stat;

    protected Animator animator;

    protected GameObject target;
    private HudController hud;
    protected BaseController controller;
    private void Update()
    {
        //if(dSkillEffect != null)
        //{
        //    dSkillEffect.transform.position = transform.position;
        //}
    }
    protected void GetSkill(string key)
    {
        skillData = Managers.Data.GetSkillData(key);
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HudController>();
    }
    public virtual void Active_q(){}
    public virtual void Active_w(){}
    public virtual void Active_e(){}
    public virtual void Active_r(){}
    public virtual void Active_d() 
    {
        if (d_spell_cool) return;
        stat.moveSpeed = stat.moveSpeed * 1.5f;
        controller._audio.PlayOneShot(controller.spellSounds[0]);
        StartCoroutine(Ghost());
        StartCoroutine(D_Spell_CoolDown());
    }

    IEnumerator Ghost()
    {
        dSkillEffect = Instantiate(dSkillEffectPrefab,transform);
        yield return new WaitForSeconds(10.0f);
        stat.moveSpeed = statData.moveSpeed;
        Destroy(dSkillEffect);
    }

    public virtual void Active_f() 
    {
        if (f_spell_cool) return;

        GameObject go = Instantiate(fSkillEffectPrefab, transform);
        go.transform.position += Vector3.up * 0.5f;
        //float duration = go.GetComponent<ParticleSystem>().main.duration;
        Destroy(go, 2.0f);

        StartCoroutine(F_Spell_CoolDown());
        controller._audio.PlayOneShot(controller.spellSounds[1]);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100.0f);

        transform.LookAt(hit.point);

        
        Vector3 dir = hit.point - transform.position;
        transform.position += dir.normalized * 7.0f;
    }
    protected IEnumerator Q_Spell_CoolDown()
    {
        q_spell_cool = true;
        if(gameObject.tag == "PLAYER")
            hud.SetCoolDownImage(0, skillData.qSkillCoolDown);
        yield return new WaitForSeconds(skillData.qSkillCoolDown);
        q_spell_cool = false;
    }
    protected IEnumerator W_Spell_CoolDown()
    {
        w_spell_cool = true;
        if (gameObject.tag == "PLAYER")
            hud.SetCoolDownImage(1, skillData.wSkillCoolDown);
        yield return new WaitForSeconds(skillData.wSkillCoolDown);
        w_spell_cool = false;
    }
    protected IEnumerator E_Spell_CoolDown()
    {
        e_spell_cool = true;
        if (gameObject.tag == "PLAYER")
            hud.SetCoolDownImage(2, skillData.eSkillCoolDown);
        yield return new WaitForSeconds(skillData.eSkillCoolDown);
        e_spell_cool = false;
    }
    protected IEnumerator R_Spell_CoolDown()
    {
        r_spell_cool = true;
        if (gameObject.tag == "PLAYER")
            hud.SetCoolDownImage(3, skillData.rSkillCoolDown);
        yield return new WaitForSeconds(skillData.rSkillCoolDown);
        r_spell_cool = false;
    }

    protected IEnumerator D_Spell_CoolDown()
    {
        d_spell_cool = true;
        if (gameObject.tag == "PLAYER")
            hud.SetCoolDownImage(4, dSkillCoolTime);
        yield return new WaitForSeconds(dSkillCoolTime);
        d_spell_cool = false;
    }
    protected IEnumerator F_Spell_CoolDown()
    {
        f_spell_cool = true;
        if (gameObject.tag == "PLAYER")
            hud.SetCoolDownImage(5, fSkillCoolTime);
        yield return new WaitForSeconds(fSkillCoolTime);
        f_spell_cool = false;
    }

}
