using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class TurretController : BaseController
{
    public Transform firePos;

    Collider[] cols;
    string bulletKey;

    public GameObject bigExpPrefab;
    public GameObject smokePrefab;
    private GameObject smoke;
    public GameObject firePrefab;

    GameObject bigExp;


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
        
        if(type.team == Define.Team.BLUE)
            layer = 1 << (int)Define.Layer.RED_MINION | 1 << (int)Define.Layer.PLAYER | 1 << (int)Define.Layer.BOT;
        if (type.team == Define.Team.RED)
            layer = 1 << (int)Define.Layer.BLUE_MINION | 1 << (int)Define.Layer.PLAYER | 1 << (int)Define.Layer.BOT;
        switch (type.turretType)
        {
            case Define.TurretType.OUTER:
                statData = Managers.Data.GetStatData("OuterTurret");
                break;
            case Define.TurretType.INNER:
                statData = Managers.Data.GetStatData("InnerTurret");
                break;
            case Define.TurretType.INHIBITOR:
                statData = Managers.Data.GetStatData("InhiBitorTurret");
                break;
            case Define.TurretType.NEXUS:
                statData = Managers.Data.GetStatData("NexusTurret");
                break;
        }
        stat.SetStat(statData);

        state = Define.State.IDLE;

        bulletKey = "Bullet_3";
        
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdateIdle()
    {
        ScanTarget();

        if (Target != null && stat.curHp > 0)
        {
            state = Define.State.ATTACK;    
        }
    }

    protected override void UpdateAttack()
    {
        
        Vector3 dir = Target.transform.position - transform.position;

        if (dir.magnitude > stat.attackRange)
        {
            Target = null;
            state = Define.State.IDLE;
            return;
        }

        if (Target.gameObject.GetComponent<Stat>().curHp <= 0)
        {
            state = Define.State.IDLE;
            return;
        }

        stat.attackSpeed -= Time.deltaTime;
        if (stat.attackSpeed <= 0)
        {    
            if (Target != null)
            {
                Managers.Bullet.Fire(bulletKey, Target, stat.attack, firePos,this.gameObject);
                Managers.Audio.PlayClip("Turret/포탑 공격소리", _audio);
                stat.attackSpeed = statData.attackSpeed;
                return;
            }
        }
    }
    protected override void UpdateDie()
    {
        _collider.isTrigger = true;
        
        gameObject.isStatic = false;
        if(!dieSoundPlaying)
        {
            Managers.Audio.PlayClip("Announcer/포탑을 파괴했습니다");
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.SetText("포탑을 파괴했습니다.");
            Managers.Audio.PlayClip("Turret/포탑터지는소리",_audio);
            dieSoundPlaying = true;
        }
        hpBar.SetActive(false);
        _collider.enabled = false;
        Target = null;
        
        if (transform.position.y > -3.0f)
        {
            transform.position += Vector3.down * Time.deltaTime;

            if(bigExp == null)
            {
                bigExp = Instantiate(bigExpPrefab, transform.position, Camera.main.transform.rotation);
                float time = bigExp.GetComponent<ParticleSystem>().main.duration;
                bigExp.transform.position += Vector3.up * 2.0f;
                Destroy(bigExp, time);
            }
            
        }
        if(transform.position.y <= -3.0f && smoke == null)
        {
            smoke = Instantiate(smokePrefab, transform);
            smoke.transform.localPosition = new Vector3(1, 4, 0);
            GameObject go = Instantiate(firePrefab, transform);
            go.transform.localPosition = new Vector3(0, 4, 0);
            
        }
    }

    void ScanTarget()
    {
        cols = Physics.OverlapSphere(transform.position, 8.0f, layer);
        if (cols.Length <= 0)
        {
            Target = null;
            return;
        }
        else
        {
            switch(type.team)
            {
                case Define.Team.BLUE:
                    foreach (Collider col in cols)
                    {
                        if (col.gameObject.GetComponent<IsType>().team == Define.Team.RED && 
                            col.gameObject.GetComponent<Stat>().curHp > 0)
                            Target = col.gameObject;
                    }
                    break;
                case Define.Team.RED:
                    foreach (Collider col in cols)
                    {
                        if (col.gameObject.GetComponent<IsType>().team == Define.Team.BLUE &&
                            col.gameObject.GetComponent<Stat>().curHp > 0)
                            Target = col.gameObject;
                    }
                    break;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Color color = new Color(1.0f, 0, 0, 0.2f);
        Gizmos.color = color;
        
        Gizmos.DrawSphere(transform.position, 8.0f);
    }

}
