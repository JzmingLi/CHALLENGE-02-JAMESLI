using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionBroadcaster : MonoBehaviour
{
    public bool PLAYER_SOUND;
    public bool PLAYER_INVISIBLE;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PLAYER_SOUND = !PLAYER_SOUND;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PLAYER_INVISIBLE = !PLAYER_INVISIBLE;
        }
    }
}
