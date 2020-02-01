using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    GameObject[] hideObjects;

    void Start()
    {
        hideObjects = GameObject.FindGameObjectsWithTag("Title");
    }

    public void Playg()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        hide();
    }

    public void hide()
    {
        foreach (GameObject g in hideObjects)
        {
            g.SetActive(false);
        }

    }
}
