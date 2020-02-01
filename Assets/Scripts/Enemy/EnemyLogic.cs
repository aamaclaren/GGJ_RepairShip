using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyType{
    Charger,
    Shooter
};

enum EnemyState { 
    Idle,
    Patrol,
    Charging,
    Shooting
};


public class EnemyLogic : MonoBehaviour
{
    //decide which kind of enemy
    [SerializeField]
    private EnemyType enemyType;
    //the min radius that the player can be detected by the enemies
    [SerializeField]
    private float attackRadius;
    //moving speed of the enemy
    [SerializeField]
    private float m_speed;
    //the moving distance at one time step
    private float m_dis;

    private ShootingLogic m_shooting;


    //state of enemy for the FSM
    private EnemyState m_state;

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
    }

    private void FixedUpdate()
    {
        m_dis = m_speed * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        dist_to_player = (player.transform.position - transform.position).magnitude;

        if (enemyType == EnemyType.Charger) charger_behavior();
        else if (enemyType == EnemyType.Shooter) shooter_behavior();


    }

    private void charger_behavior()
    {

        switch (m_state) {
            case EnemyState.Idle:
                if (dist_to_player <= attackRadius)
                {
                    m_state = EnemyState.Charging;
                }
                break;
            case EnemyState.Patrol:
                if (dist_to_player <= attackRadius) {
                    m_state = EnemyState.Charging;
                }
                break;
            case EnemyState.Charging:
                transform.position = Vector3.MoveTowards(transform.position,getTarget(),m_dis);
                //Quaternion q = Quaternion.LookRotation((getTarget() - transform.position).normalized);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 100);

                //Debug.Log(q);
                break;
            default:
                break;
        }

    }

    private void shooter_behavior() {
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
                if (!m_shooting.isfiring()) {
                    m_shooting.activate_fire(true);
                }
                break;
            default:
                break;
        }

    }

    private Vector3 getTarget() {
        //might need to rewrite this funciton

        return player.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enemyType==EnemyType.Charger&&collision.gameObject.tag == "Player") Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.5f, 0.5f, 0.5f);
        Gizmos.DrawSphere(transform.position, attackRadius);
    }

}
