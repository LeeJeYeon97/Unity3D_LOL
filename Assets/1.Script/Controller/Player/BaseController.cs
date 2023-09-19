using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseController : MonoBehaviour
{
    protected Vector3 destPos;
    protected int layer;
    public int MyLayer
    { get { return layer; } set { layer = value; } }

    protected Ray ray;
    protected RaycastHit hit;
    protected GameObject hitObject;


    [SerializeField]
    protected Define.State state = Define.State.IDLE;
    public virtual Define.State State
    {
        get { return state; }
        set { state = value; }
    }
    
    [SerializeField] 
    protected GameObject target;
    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }
    
    protected Rigidbody _rigidbody;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Collider _collider;

    protected Stat stat;
    protected StatData statData;
    protected IsType type;
    protected BaseSkill skill;

    private GameObject hpBarPrefab;
    protected GameObject hpBar;

    protected bool isAirbone = false;
    protected bool isSnare = false;

    public Transform spawnPos;
    public GameObject dieEffectPrefab;
    private GameObject dieEffect;

    float respawnTime = 3.0f;
    public GameObject spawnPrefab;
    private bool isRange = false;

    public AudioSource _audio;
    public List<AudioClip> voices;
    public List<AudioClip> spellSounds;

    protected bool dieSoundPlaying = false;
    public GameObject rangeCircle;
    protected virtual void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        _collider = gameObject.GetComponent<Collider>();
        type = gameObject.GetOrAddComponent<IsType>();
        stat = gameObject.GetOrAddComponent<Stat>();
        skill = gameObject.GetComponent<BaseSkill>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        _audio = gameObject.GetComponent<AudioSource>();
    }
    protected virtual void Start()
    {
        if(gameObject.tag == "BLUE_NEXUS" || gameObject.tag == "RED_NEXUS")
        {
            statData = Managers.Data.GetStatData("Nexus");
            stat.SetStat(statData);
            hpBarPrefab = Resources.Load("Prefabs/UI/HpBar_Turret") as GameObject;

        }
        if (gameObject.tag == "MINION")
        {
            hpBarPrefab = Resources.Load("Prefabs/UI/HpBar_Minion") as GameObject;
        }
        else if (gameObject.tag == "PLAYER" || gameObject.tag =="BOT")
        {
            hpBarPrefab = Resources.Load("Prefabs/UI/UI_HpBar") as GameObject;
        }
        else if (gameObject.tag == "TURRET")
        {
            hpBarPrefab = Resources.Load("Prefabs/UI/HpBar_Turret") as GameObject;
        }
        hpBar = Instantiate(hpBarPrefab);
        hpBar.transform.SetParent(transform, false);
        hpBar.isStatic = true;

        if(type.team == Define.Team.BLUE)
        {
            GameObject go = GameObject.Find("GameManager");
            spawnPos = go.GetComponent<GameManager>().blue_ChampSpawnPos;
        }
        else if (type.team == Define.Team.RED)
        {
            GameObject go = GameObject.Find("GameManager");
            spawnPos = go.GetComponent<GameManager>().red_ChampSpawnPos;
        }
        
    }
    protected virtual void Update()
    {
        if (state != Define.State.AIRBONE)
        {
            _rigidbody.velocity = Vector3.zero;
        }
        
        _rigidbody.angularVelocity = Vector3.zero;

        switch (State)
        {
            case Define.State.IDLE:
                if (isAirbone || isSnare) return;
                UpdateIdle();
                break;
            case Define.State.MOVE:
                if (isAirbone || isSnare) return;
                UpdateMoving();
                break;
            case Define.State.ATTACK:
                if (isAirbone || isSnare) return;
                UpdateAttack();
                break;
            case Define.State.DIE:
                UpdateDie();
                break;
            case Define.State.AIRBONE:
                agent.enabled = false;
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
                if (!isAirbone && transform.position.y >=0)
                {
                    _rigidbody.AddForce(Vector3.up * 400.0f);
                    isAirbone = true;
                }
                else if(isAirbone && transform.position.y <=0)
                {
                    isAirbone = false;
                    agent.enabled = true;
                    state = Define.State.IDLE;
                }
                break;
            case Define.State.SNARE:
                if(!isSnare)
                    StartCoroutine(Snare());
                break;
            default:
                break;
        }
    }
    IEnumerator Snare()
    {
        agent.isStopped = true;
        isSnare = true;
        yield return new WaitForSeconds(1.5f);
        agent.isStopped = false;
        isSnare = false;
        state = Define.State.IDLE;
    }
    protected virtual void OnMouseClicked()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rangeCircle.gameObject.SetActive(false);
            // 물체의 Layer를 맞춰 Raycast
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
                    switch (type.team)
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
                    if (Target != null)
                    {
                        destPos = Target.transform.position;
                        state = Define.State.MOVE;
                    }
                }
            }
        }
    }
    
    protected virtual void OnKeyboard()
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            skill.Active_r();
        }
        if (Input.GetKeyDown(KeyCode.A)) // 공격 범위 보여주기
        {
            isRange = !isRange;
        }
        if (Input.GetKeyDown(KeyCode.D)) // 점멸
        {
            skill.Active_d();
        }
        if (Input.GetKeyDown(KeyCode.F)) // 점멸
        {
            skill.Active_f();
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            rangeCircle.gameObject.SetActive(true);
        }
    }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateDie()
    {
        if(dieEffect == null)
        {
            dieEffect = Instantiate(dieEffectPrefab, transform);
        }
        
        agent.enabled = false;
        hpBar.SetActive(false);
        _collider.enabled = false;
        Target = null;

        respawnTime -= Time.deltaTime;
        if(respawnTime < 0)
        {
            SetAlive();
            respawnTime = 3.0f;
        }
    }

    protected void SetAlive()
    {
        agent.enabled = true;
        Destroy(dieEffect);
        _collider.enabled = true;
        stat.curHp = statData.maxHp;
        stat.curMp = statData.maxMp;

        hpBar.SetActive(true);
        transform.position = spawnPos.localPosition;
        
        GameObject go = Instantiate(spawnPrefab, transform);
        go.transform.position += Vector3.up;
        dieSoundPlaying = false;
        Destroy(go, 2.0f);
        state = Define.State.IDLE;
    }

    public virtual void OnDamaged(float attackPower, GameObject hitObject = null)
    {
        if (hitObject != null)
        {
            this.hitObject = hitObject;
        }
        float damage = Mathf.Max(0, attackPower - stat.defense);
        stat.curHp -= damage;

        if (stat.curHp <= 0 && State != Define.State.DIE)
        {
            State = Define.State.DIE;
            
            if (hitObject == null) return;

            hitObject.GetComponent<Stat>().gold += stat.dieGold;
            hitObject.GetComponent<Stat>().exp += stat.exp;
            
            foreach(GameObject goldUI in Managers.Pool.goldUIs)
            {
                if (!goldUI.activeSelf)
                {
                    goldUI.GetComponent<GoldUIController>().Target = transform.gameObject;
                    goldUI.SetActive(true);
                    break;
                }
                else
                    continue;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!isRange) return;

        float radius = stat.attackRange;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
