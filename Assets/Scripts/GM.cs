using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GM : MonoBehaviour
{
    public static GM gm;
    public GameObject player;
    public AudioSource[] sounds;
    public int playerHealth = 100;
    public int playerMass = 1;

    private Material defaultPlayerMaterial;
    private PlayerController playerController;
    public HiddenMsgs msg;

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

        defaultPlayerMaterial = player.gameObject.GetComponent<Renderer>().material;
        playerController = player.GetComponent<PlayerController>();
    }

    public void DamagePlayer(int damage = 100)
    {
        playerHealth = Mathf.Clamp(playerHealth - damage, 0, 100);
        //HiddenMsgs.setHealth(playerHealth);
        GM.gm.msg.setHealth(playerHealth);
        if (playerHealth <= 0)
        {
            GM.gm.msg.showLose();
        }

        // flashing
        playerController.StartFlashing();
        if (damage > 0) {
            playerController.StartSpinning();
        }
    }

    public void SetMass(int newMass = 1) {
        playerMass = newMass;
        GM.gm.msg.setMass(playerMass);
        if (newMass >= 100)
        {
            GM.gm.msg.showWin();
        }
    }

    public bool PlayerIsInvincible() {
        return playerController.playerIsInvincible;
    }

    public ConnectionSystem GetPlayerCS() {
        return playerController.cs;
    }
}
