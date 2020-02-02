using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{

    [SerializeField]
    List<WeaponLogic> weapons;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1")) { 
            foreach(WeaponLogic w in weapons) {
                w.fire();
            }
        }
        if (Input.GetButtonUp("Fire1")) {
            foreach (WeaponLogic w in weapons)
            {
                w.stopfiring();
            }
        }


    }

    public void addnewWeapon(WeaponLogic w) {
        weapons.Add(w);
    }
}
