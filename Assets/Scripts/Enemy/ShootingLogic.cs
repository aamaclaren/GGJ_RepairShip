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

    public bulletLogic bullet;


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

        coolDownCounter = coolDownTime / 2;
        ShotTimeCounter = 0;


        m_player = GameObject.FindGameObjectWithTag("Player");
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
                        bulletLogic new_bullet = Instantiate(bullet, t.position, Quaternion.Euler(90, 0, 0));
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

    public void activate_fire(bool val)
    {

        isfire = val;
        firecount = 0;
        coolDownCounter = coolDownTime / 2;
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
