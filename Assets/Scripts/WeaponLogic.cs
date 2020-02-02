using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WeaponType { 
    MachineGun,
    Turrent
}

public class WeaponLogic : MonoBehaviour
{
    [SerializeField]
    private WeaponType m_weaponType;

    [SerializeField]
    private float BulletSpeed;

    [SerializeField]
    private float shooting_interval;
    private float shooting_counter;

    [SerializeField]
    private float TimeBtwShots;
    private float ShotTimeCounter;

    [SerializeField]
    private int fireCountMax;
    private int fireCount;

    [SerializeField]
    private float damage;

    [SerializeField]
    List<Transform> Fire_pos;

    public bulletLogic bullet;



    // Start is called before the first frame update
    void Start()
    {
        fireCount = 0;
        shooting_counter = shooting_interval;
        ShotTimeCounter = 0;
        bullet.fromEnemy = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void fire()
    {

        switch (m_weaponType)
        {

            case WeaponType.MachineGun:
                useMachineGun();
                break;

            case WeaponType.Turrent:
                break;
            default:
                break;
        }

    }

    public void stopfiring()
    {
        fireCount = 0;
        shooting_counter = shooting_interval;
        ShotTimeCounter = 0;
    }

    public void useMachineGun()
    {

        //check time between attacks
        if (shooting_counter > 0)
        {
            shooting_counter -= Time.deltaTime;
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
                fireCount++;

                foreach (Transform t in Fire_pos)
                {
                    bulletLogic new_bullet = Instantiate(bullet, t.position, Quaternion.Euler(90, 0, 0));

                    new_bullet.setSpeed(BulletSpeed);
                    new_bullet.setDir(t.forward);
                }
                if (fireCount >= fireCountMax)
                {
                    fireCount = 0;
                    shooting_counter = shooting_interval;
                    //need to deactivate game objects and put them back into the pool
                }
            }

        }

    }
}
