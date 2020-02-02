using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletLogic : MonoBehaviour
{
    [SerializeField]
    private float m_speed;

    [SerializeField]
    private float m_lifetime;

    private float m_startTime;

    public bool fromEnemy = true;
    public int damage;



    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        transform.forward = dir;
        m_startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (dir * m_speed * Time.deltaTime);
        if (Time.time - m_startTime >= m_lifetime) destorySelf();
    }

    private void OnEnable()
    {
        m_startTime = Time.time;
    }

    public void setDir(Vector3 thedir)
    {
        dir = thedir;
    }

    public void setSpeed(float spd) {
        m_speed = spd;
    }

    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log(other.tag);
        if (fromEnemy)
        {
            if (other.gameObject.GetComponent<ConnectionSystem>() != null) {
                // GM.gm.DamagePlayer(damage);
                other.gameObject.GetComponent<ConnectionSystem>().TakeDamage(damage);
                destorySelf();
            }
        }
        if(!fromEnemy && other.tag == "Enemy") {
            other.gameObject.GetComponent<EnemyLogic>().takeDamage(damage);
            destorySelf();
        }
    }

    private void destorySelf() {

        ResourcePoolMember rpm = gameObject.GetComponent<ResourcePoolMember>();
        if(rpm != null) {
            rpm.Despawn();
        }
        else {
            Destroy(gameObject);
        }
    }


}
