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
    private float largestRadius = 0f;
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
        RefreshLargestRadius();
    }

    public void DamagePlayer(int damage = 100)
    {
        //playerHealth = Mathf.Clamp(playerHealth - damage, 0, 100);
        //HiddenMsgs.setHealth(playerHealth);
        playerHealth -= damage;
        msg.UpdateHealth();
        if (playerHealth <= 0)
        {
            msg.showLose();
            return;
        }

        // flashing
        playerController.StartFlashing();
        //if (damage > 0) {
          //  playerController.StartSpinning();
        //}
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

    // returns the combined mass of the ship and all of its parts
    public int GetMassWithChildren() {
        int totalMass = 0;
        foreach (ConnectionSystem child in player.gameObject.GetComponentsInChildren<ConnectionSystem>()) {
            totalMass += child.mass;
        }
        // foreach (Transform child in gameObject.GetComponentsInChildren<Transform>()) {
        //  Debug.Log(child.name);
        // }
        return totalMass;
    }

    public void RefreshLargestRadius() {
        float rad = 0f;
        foreach (Transform child in player.gameObject.GetComponentsInChildren<Transform>()) {
            if (child.gameObject.GetComponent<Camera>() == null) {
                float newRadius = Mathf.Abs((child.position - player.transform.position).magnitude);
                rad = Mathf.Max(rad, newRadius);
            }
        }
        largestRadius = rad;
    }

    public float GetLargestRadius() {
        return largestRadius;
    }
}
