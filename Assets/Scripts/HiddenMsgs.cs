using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HiddenMsgs : MonoBehaviour
{
    public AnimationCurve blip;                                         //block for blip
    public float blipduration;
    public float blipscale;
    private Coroutine blipCoroutine;
    private float start = 0;

    public Text hull;
    private int health = 100;
    public Text massx;
    private int count = 1;

    private IEnumerator blipanimation()
    {

        float perc = 0;
        start = Time.timeSinceLevelLoad;
        do
        {
            float time = Time.timeSinceLevelLoad - start;
            perc = time / blipduration;
            transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(blipscale, blipscale, blipscale), blip.Evaluate(perc));
            yield return null;
        } while (perc < 1);
        blipCoroutine = null;
        transform.localScale = Vector3.one;
    }

    GameObject[] pauseObjects;
    GameObject[] loseObjects;
    GameObject[] winObjects;
    private int hideCase = 0;

    void Start()
    {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        loseObjects = GameObject.FindGameObjectsWithTag("ShowOnLose");
        winObjects = GameObject.FindGameObjectsWithTag("ShowOnWin");
        hidePause();
        hideLose();
        hideWin();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))                                            //blip if
        {
            count++;
            health -= 10;
            hull.text = "Hull Integrity: " + health.ToString() + "%";
            massx.text = "Mass: " + count.ToString();
            if (blipCoroutine == null)
            {
                blipCoroutine = StartCoroutine(blipanimation());
            }
            else
            {
                start = Time.timeSinceLevelLoad;
            }

        }
        if (health <= 0)
        {
            showLose();
        }

        if (Input.GetKeyDown(KeyCode.Escape))                   //options
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                showPause();
            }
            else if (Time.timeScale == 0)
            {
                Debug.Log("high");
                Time.timeScale = 1;
                hidePause();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))                    //win
        {
            showWin();
        }
    }

    
    // Hide commands
    public void hidePause()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
        hideCase = 0;
    }
    public void hideLose()
    {
        foreach (GameObject g in loseObjects)
        {
            g.SetActive(false);
        }
        hideCase = 0;
    }
    public void hideWin()
    {
        foreach (GameObject g in winObjects)
        {
            g.SetActive(false);
        }
        hideCase = 0;
    }

    
    //show commands
    public void showPause()
    {
        if (hideCase == 0)
        {
            foreach (GameObject g in pauseObjects)
            {
                g.SetActive(true);
            }
            hideCase = 1;
        }
    }
    public void showLose()
    {
        if (hideCase == 0)
        {
            Time.timeScale = 0;
            foreach (GameObject g in loseObjects)
            {
                g.SetActive(true);
            }
            hideCase = 1;
        }
    }
    public void showWin()
    {
        if (hideCase == 0)
        {
            Time.timeScale = 0;
            foreach (GameObject g in winObjects)
            {
                g.SetActive(true);
            }
            hideCase = 1;
        }
    }


    //Buttons
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void Resume()
    {
        Time.timeScale = 1;
        hidePause();
    }
    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



}
