using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingLogic : MonoBehaviour
{
    [SerializeField]
    private float coolDownTime;
    private float coolDownCounter;

    [SerializeField]
    private float TimeBtwShots;
    private float ShotTimeCounter;

    [SerializeField]
    private float BulletSpeed;

    [SerializeField]
    private List<Transform> fire_pos;

    public GameObject bullet;
    
    [SerializeField]
    private int poolSize = 100;
    private ResourcePool pool;
    private GameObject bulletObjContainer;


    private bool isfire;

    [SerializeField]
    private int max_firecount;
    private int firecount;

    private GameObject m_player;

    // Start is called before the first frame update
    void Start()
    {
        isfire = false;
        firecount = 0;
        bullet.GetComponent<bulletLogic>().fromEnemy = true;
        coolDownCounter = coolDownTime;
        ShotTimeCounter = 0;

        m_player = GameObject.FindGameObjectWithTag("Player");

        InitializeBulletResourcePool();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(coolDownCounter);
        if (isfire)
        {
            //check time between attacks
            if (coolDownCounter > 0)
            {
                coolDownCounter -= Time.deltaTime;
            }
            else
            {

                //check time between two shots
                if (ShotTimeCounter > 0)
                {
                    ShotTimeCounter -= Time.deltaTime;
                }
                else
                {
                    //Debug.Log(ShotTimeCounter);
                    ShotTimeCounter = TimeBtwShots;
                    firecount++;

                    foreach (Transform t in fire_pos)
                    {
                        bulletLogic new_bullet = pool.Spawn(t.position, Quaternion.Euler(90, 0, 0)).GetComponent<bulletLogic>();
                        //Debug.Log(new_bullet.name);
                        new_bullet.fromEnemy = true;
                        new_bullet.setSpeed(BulletSpeed);
                        new_bullet.setDir(t.forward);
                        //new_CannoBall.dir = m_player.transform.position - fire_pos.transform.position;
                        //Debug.Log(new_CannoBall.dir);
                    }
                    if (firecount >= max_firecount)
                    {
                        firecount = 0;
                        coolDownCounter = coolDownTime;

                        //need to deactivate game objects and put them back into the pool
                    }



                }

            }
        }
    }

    private void InitializeBulletResourcePool() {
        pool = new ResourcePool(bullet, poolSize);

        string bulletsContainerName = "Bullets";
        bulletObjContainer = GameObject.Find(bulletsContainerName);

        if(bulletObjContainer == null)
        {
            bulletObjContainer = new GameObject(bulletsContainerName);
        }
    }

    public void activate_fire(bool val)
    {

        isfire = val;
        firecount = 0;
        coolDownCounter = coolDownTime;
        ShotTimeCounter = 0;
    }

    public bool isfiring()
    {
        return isfire;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1, 0, 0, 0.5f);
    //    Gizmos.DrawLine(fire_pos.position, m_player.transform.position);
    //}
}