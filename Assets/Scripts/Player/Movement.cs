using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private CharacterController playerController;
    [SerializeField] private float speed;
    private Vector3 direction;

    void Update()
    {
        GetPlayersInput();
    }

    private void FixedUpdate()
    {
        if(direction.magnitude > 0f)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
        Debug.Log($"Current move should be: {direction * speed * Time.fixedDeltaTime}");
    }

    private void GetPlayersInput()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");
        direction = new Vector3(x: xInput, y: 0, z: zInput).normalized;

        //Debug.Log($"Current input: {direction}");
    }
}
