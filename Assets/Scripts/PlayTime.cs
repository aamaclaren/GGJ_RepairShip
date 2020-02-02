using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayAni : MonoBehaviour
{
    PlayableDirector play;
    void Start()
    {
        play = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        
    }
    
    public void onClick()
    {
        play.Play();
    }
}
