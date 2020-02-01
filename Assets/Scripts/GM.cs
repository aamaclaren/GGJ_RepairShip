﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public static GM gm;
    public GameObject player;
    public int playerHealth = 100;
    public Material hurtMaterial;
    public ConnectionSystem playerCS;

    private bool isInvincible = false;
    private Material defaultPlayerMaterial;

    // Start is called before the first frame update
    void Start()
    {
        if (gm != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gm = this;
        }

        playerCS = player.gameObject.GetComponent<ConnectionSystem>();
        defaultPlayerMaterial = player.gameObject.GetComponent<Renderer>().material;
    }

    public IEnumerator PlayerTookDamage(int damage = 100)
    {
        isInvincible = true;
        playerCS.isConnectable = false;
        for (int i = 0; i < 5; i++)
        {
            player.gameObject.GetComponent<Renderer>().material = hurtMaterial;
            yield return new WaitForSeconds(.12f);
            player.gameObject.GetComponent<Renderer>().material = defaultPlayerMaterial;
            if (i < 4) {
                yield return new WaitForSeconds(.12f);
            }
        }
        isInvincible = false;
        playerCS.isConnectable = true;

    }
}
