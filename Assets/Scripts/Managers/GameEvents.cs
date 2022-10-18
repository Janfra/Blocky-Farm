using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    // Testing
    public Transform playerPos;
    public static event Action<Transform> FollowPlayer;

    private void Awake()                 
    {

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            FollowPlayer?.Invoke(playerPos);
        }
    }

}
