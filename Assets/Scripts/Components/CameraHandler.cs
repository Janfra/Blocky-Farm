using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    private const float yOffset = 25f;
    private const float zOffset = 20f;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    private void LateUpdate()
    {
        FollowPlayer();
        transform.LookAt(playerPos);
    }

    private void FollowPlayer()
    {
        transform.position = new Vector3(playerPos.position.x, playerPos.position.y + yOffset, playerPos.position.z - zOffset);
    }
}
