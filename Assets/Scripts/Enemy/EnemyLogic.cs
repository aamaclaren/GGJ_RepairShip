using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyType{
    Charger,
    Shooter,
    Fort,
};

enum EnemyState { 
    Idle,
    Aiming,
    Patrol,
    Shooting,

    //only for charger
    Charging,

    //only for shooter
    Moving
};


public class EnemyLogic : MonoBehaviour
{
    //decide which kind of enemy
    [SerializeField]
    private EnemyType enemyType;
    [SerializeField]
    private float m_health;
    //the min radius that the player can be detected by the enemies
    [SerializeField]
    private float attackRadius;
    //moving speed of the enemy
    [SerializeField]
    private float m_speed;
    private float m_originSpeed;
    [SerializeField]
    private float m_acceleration;
    [SerializeField]
    private float m_angularSpeed;
    [SerializeField]
    private float m_Aimingtime;
    private float m_AimingtimeCounter;




    private ShootingLogic m_shooting;

    //variables for charger
    private Vector3 m_target;
    private Vector3 m_movingDir;
    private bool m_slowdown;

    //variables for shooter
    [SerializeField]
    private float m_MovingTimeBtwShots;
    private float m_nextMovingTimeBtwShots;
    private float m_MovingTimeCounter;

    [SerializeField]
    private float m_ShootingTimeBtwMoving;
    private float m_nextShootingTimeBtwMoving;
    private float m_ShootingTimeCounter;


    //state of enemy for the FSM
    private EnemyState m_state;

    //navmesh agent
    private NavMeshAgent m_navAgent;

    //reference to the player object
    private GameObject player;
    //rigidbody
    private Rigidbody m_rb;
    //distance to the player
    private float dist_to_player;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        m_state = EnemyState.Idle;
        m_rb = GetComponent<Rigidbody>();
        m_shooting = GetComponent<ShootingLogic>();

        if (enemyType == EnemyType.Shooter)
        {
            m_navAgent = GetComponent<NavMeshAgent>();
            m_navAgent.speed = m_speed;
            m_navAgent.isStopped = true;
        }

        m_AimingtimeCounter = 0;
        m_originSpeed = m_speed;
        m_slowdown = false;

        m_nextMovingTimeBtwShots = m_MovingTimeBtwShots;
        m_MovingTimeCounter = 0;
        m_nextShootingTimeBtwMoving = m_ShootingTimeBtwMoving;
        m_ShootingTimeCounter = 0;




    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_health <= 0) Destroy(gameObject);
        dist_to_player = (player.transform.position - transform.position).magnitude;

        if (enemyType == EnemyType.Charger) charger_behavior();
        else if (enemyType == EnemyType.Shooter) shooter_behavior();
        else if (enemyType == EnemyType.Fort) fort_behavior();


    }

    private void charger_behavior()
    {
        //Debug.Log(m_state);
        //Debug.Log(m_slowdown);
        switch (m_state) {
            case EnemyState.Idle:
                if (dist_to_player <= attackRadius)
                {
                    m_state = EnemyState.Aiming;
                }
                break;
            case EnemyState.Patrol:
                if (dist_to_player <= attackRadius) {
                    m_state = EnemyState.Charging;
                }
                break;
            case EnemyState.Aiming: 
                Quaternion q = Quaternion.LookRotation((getTarget() - transform.position).normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q, m_angularSpeed);
                if (m_AimingtimeCounter >= m_Aimingtime)
                {
                    m_AimingtimeCounter = 0;
                    m_state = EnemyState.Charging;
                }
                else m_AimingtimeCounter += Time.fixedDeltaTime;

                break;
            case EnemyState.Charging:

                if (m_slowdown)
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + m_movingDir*100, m_speed * Time.fixedDeltaTime);
                    m_speed -= m_acceleration * Time.fixedDeltaTime;
                    if (m_speed <= m_originSpeed/2) {
                        m_state = EnemyState.Aiming;
                        m_speed = m_originSpeed;
                        m_slowdown = false;
                    }
                }
                else
                {

                    transform.position = Vector3.MoveTowards(transform.position, m_target, m_speed * Time.fixedDeltaTime);
                    m_speed += m_acceleration * Time.fixedDeltaTime;
                    //Debug.Log(m_speed);
                    if ((transform.position - m_target).magnitude < 0.01f)
                    {
                        m_slowdown = true;
                        //m_state = EnemyState.Aiming;
                        //m_speed = m_originSpeed;
                    }
                }
                //Quaternion q = Quaternion.LookRotation((getTarget() - transform.position).normalized);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, q, m_angularSpeed);

                //Debug.Log(q);
                break;
            default:
                break;
        }

    }

    private void shooter_behavior() {
        //Debug.Log(m_state);
        switch (m_state)
        {

            case EnemyState.Idle:
                if (dist_to_player <= attackRadius)
                {
                    m_state = EnemyState.Aiming;
                }
                break;
            case EnemyState.Patrol:
                if (dist_to_player <= attackRadius)
                {
                    m_state = EnemyState.Charging;
                }
                break;
            case EnemyState.Aiming:
                if (dist_to_player > attackRadius*2) {
                    m_state = EnemyState.Idle;
                    break;
                }

                Quaternion q = Quaternion.LookRotation((player.transform.position - transform.position));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q, m_angularSpeed);
                //Debug.Log(transform.rotation.eulerAngles.y - q.eulerAngles.y);
                if (m_AimingtimeCounter >= m_Aimingtime) 
                {
                    m_AimingtimeCounter = 0;
                    //transform.rotation = q;
                    m_state = EnemyState.Shooting;
                    break;
                }
                else m_AimingtimeCounter += Time.fixedDeltaTime;

                break;

            case EnemyState.Moving:
                m_MovingTimeCounter += Time.deltaTime;
                if (m_MovingTimeCounter >= m_nextMovingTimeBtwShots) {
                    m_state = EnemyState.Aiming;
                    m_MovingTimeCounter = 0;
                    m_nextMovingTimeBtwShots = Random.Range(m_MovingTimeBtwShots * 10 - 5, m_MovingTimeBtwShots * 10 + 5) / 10;
                    m_navAgent.isStopped = true;
                }
                else {

                    m_navAgent.SetDestination(m_target);
                
                }



                break;


            case EnemyState.Shooting:
                transform.LookAt(player.transform.position);
                m_ShootingTimeCounter += Time.deltaTime;
                if (m_ShootingTimeCounter >= m_nextShootingTimeBtwMoving) {
                    m_state = EnemyState.Moving;
                    m_ShootingTimeCounter = 0;
                    m_nextShootingTimeBtwMoving = Random.Range(m_ShootingTimeBtwMoving*10 - 5, m_ShootingTimeBtwMoving*10 + 5)/10;
                    m_navAgent.isStopped = false;
                    m_target = getNextMovingPos();
                    m_shooting.activate_fire(false);
                    break;
                }

                //if ((transform.position - player.transform.position).magnitude >= attackRadius)
                //{
                //    m_shooting.activate_fire(false);
                //    m_state = EnemyState.Idle;
                //    break;

                //}
                if (!m_shooting.isfiring()) {
                    m_shooting.activate_fire(true);
                }
                //else {
                //    Quaternion q = Quaternion.LookRotation((getTarget() - transform.position).normalized);
                //    transform.rotation = Quaternion.RotateTowards(transform.rotation, q, m_angularSpeed);
                //}
                break;
            default:
                break;
        }

    }

    private void fort_behavior()
    {
        //Debug.Log(m_state);
        //Debug.Log(m_shooting.isfiring());
        //Debug.Log(m_health);
        m_shooting.bullet.GetComponent<bulletLogic>().fromEnemy = true;
        //Debug.Log(m_shooting.bullet.GetComponent<bulletLogic>().fromEnemy);
        switch (m_state)
        {

            case EnemyState.Idle:
                if (dist_to_player <= attackRadius)
                {
                    m_state = EnemyState.Shooting;
                }
                break;
            case EnemyState.Patrol:
                if (dist_to_player <= attackRadius)
                {
                    m_state = EnemyState.Charging;
                }
                break;
            case EnemyState.Shooting:
                if ((transform.position - player.transform.position).magnitude >= attackRadius)
                {
                    m_shooting.activate_fire(false);
                    m_state = EnemyState.Idle;
                    break;

                }
                if (!m_shooting.isfiring())
                {
                    m_shooting.activate_fire(true);
                }
                else
                {
                    Quaternion q = Quaternion.LookRotation((getTarget() - transform.position).normalized);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, q, m_angularSpeed);
                }
                break;
            default:
                break;
        }

    }

    private Vector3 getNextMovingPos() {
        //Debug.Log(m_target)

        Vector3 dir = (player.transform.position - transform.position).normalized;

        dir = Quaternion.Euler(0,Mathf.Pow(-1, Random.Range(1,3)) * Random.Range(30, 55), 0) * dir;

        return transform.position + dir*20;

    }

    private Vector3 getTarget() {
        //might need to rewrite this funciton
        m_target = player.transform.position;
        m_movingDir = (m_target - transform.position).normalized;
        m_movingDir.y = 0;
        return player.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GM.gm.player.GetComponent<PlayerController>().StartSpinning();

            // GM.gm.player.GetComponent<Rigidbody>().AddTorque(Vector3.up * 1000);
            if (enemyType == EnemyType.Charger) Destroy(gameObject);
            //else GM.gm.player.GetComponent<Rigidbody>().AddForce(GM.gm.player.transform.position-transform.position);


        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.5f, 0.5f, 0.5f);
        //Gizmos.DrawSphere(transform.position, attackRadius);
        Gizmos.DrawRay(transform.position,m_target-transform.position);
    }


    public void takeDamage(float d) {
        m_health -= d;
    }

}
