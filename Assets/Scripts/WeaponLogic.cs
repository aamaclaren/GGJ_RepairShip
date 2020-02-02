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
    private int damage;

    [SerializeField]
    List<Transform> Fire_pos;
    [SerializeField]
    private float shooting_interval;
    private float shooting_counter;
    [SerializeField]
    private float TimeBtwShots;
    private float ShotTimeCounter;

    int mask = 1 << 11;

    [Header("MachineGun variables")]
    //variables for machine gun
    [SerializeField]
    private float BulletSpeed;

    [SerializeField]
    private int fireCountMax;
    private int fireCount;
    public bulletLogic bullet;




    [Header("Turrents variable")]
    [SerializeField]
    private float razerLength = 15;
    LineRenderer line;


    // Start is called before the first frame update
    void Start()
    {
        fireCount = 0;
        shooting_counter = shooting_interval;
        ShotTimeCounter = 0;
        bullet.fromEnemy = false;
        bullet.damage = damage;

        if (m_weaponType == WeaponType.Turrent) {
            line = GetComponent<LineRenderer>();
            line.positionCount = 2;
            line.widthMultiplier = 0.5f;
            line.enabled = false;
        }
        else {
            GetComponent<LineRenderer>().enabled = false;
        }
        mask = ~mask;
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
                useTurret();
                break;
            default:
                break;
        }

    }

    public void stopfiring()
    {
        fireCount = 0;
        shooting_counter = 0;
        ShotTimeCounter = 0;
        if(m_weaponType==WeaponType.Turrent)line.enabled = false;
    }

    public void useMachineGun()
    {

        //check time between attacks
        if (shooting_counter <= shooting_interval)
        {
            shooting_counter += Time.deltaTime;
        }
        else
        {

            //check time between two shots
            if (ShotTimeCounter <= TimeBtwShots)
            {
                ShotTimeCounter += Time.deltaTime;
            }
            else
            {
                //Debug.Log(ShotTimeCounter);
                ShotTimeCounter = 0;
                fireCount++;

                foreach (Transform t in Fire_pos)
                {
                    bulletLogic new_bullet = Instantiate(bullet, t.position, Quaternion.Euler(90, 0, 0));
                    new_bullet.fromEnemy = false;
                    new_bullet.setSpeed(BulletSpeed);
                    new_bullet.setDir(t.forward);
                }
                if (fireCount >= fireCountMax)
                {
                    fireCount = 0;
                    shooting_counter = 0;
                    //need to deactivate game objects and put them back into the pool
                }
            }

        }

    }

    public void useTurret()
    {
        if (shooting_counter < shooting_interval) {
            shooting_counter += Time.deltaTime;
        }
        else {
            if (ShotTimeCounter <= TimeBtwShots)
            {
                ShotTimeCounter += Time.deltaTime;

                line.enabled = true;
                Ray ray = new Ray(Fire_pos[0].position, Fire_pos[0].forward);
                line.SetPosition(0, ray.origin);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, mask))
                {
                    
                    line.SetPosition(1, hit.point);
                    //Damage health here

                }
                else
                {
                    line.SetPosition(1, ray.GetPoint(razerLength));
                }

            }
            else
            {
                ShotTimeCounter = 0;
                shooting_counter = 0;
                line.enabled = false;
            }

        }





    }
}
