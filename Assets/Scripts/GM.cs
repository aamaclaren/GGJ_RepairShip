using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GM : MonoBehaviour
{
    public static GM gm;
    public GameObject player;
    public ConnectionSystem playerCS;
    public AudioSource[] sounds;
    public int playerHealth = 100;
    // public Material hurtMaterial;
    public bool playerIsInvincible = false;
    public float invincibilityTime = 1.5f;
    public int numFlashes = 5;

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
        playerIsInvincible = true;
        playerCS.isConnectable = false;
        playerHealth -= damage;
        float iTimePerFlash = invincibilityTime / (numFlashes * 2 - 1);
        List<GameObject> meshChildren = new List<GameObject>();
        Color meshColor = player.gameObject.GetComponent<Renderer>().material.color;
        List<Color> meshChildrenColors = new List<Color>();
        foreach (Transform child in player.gameObject.GetComponentsInChildren<Transform>()) {
            if (child.gameObject.GetComponent<MeshRenderer>() != null) { // || child.gameObject.GetComponent<Renderer>() != null
                meshChildren.Add(child.gameObject);
                meshChildrenColors.Add(child.gameObject.GetComponent<Renderer>().material.color);
            }
        }
        for (int i = 0; i < numFlashes; i++)
        {
            // player.gameObject.GetComponent<Renderer>().material = hurtMaterial;
            // yield return new WaitForSeconds(iTimePerFlash);
            // player.gameObject.GetComponent<Renderer>().material = defaultPlayerMaterial;
            // if (i < numFlashes - 1) {
            //     yield return new WaitForSeconds(iTimePerFlash);
            // }
            player.gameObject.GetComponent<Renderer>().material.color = Color.red;
            for (int j = 0; j < meshChildren.Count; j++) {
                meshChildren[j].GetComponent<Renderer>().material.color = Color.red;
            }
            yield return new WaitForSeconds(iTimePerFlash);
            player.gameObject.GetComponent<Renderer>().material.color = meshColor;
            for (int j = 0; j < meshChildren.Count; j++) {
                meshChildren[j].GetComponent<Renderer>().material.color = meshChildrenColors[j];
            }
            if (i < numFlashes - 1) {
                yield return new WaitForSeconds(iTimePerFlash);
            }
        }
        playerIsInvincible = false;
        playerCS.isConnectable = true;
    }
}
