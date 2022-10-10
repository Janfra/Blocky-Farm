using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    /// <summary>
    /// Movement information/stats
    /// </summary>
    [SerializeField] private MoveStats stats;

    /// <summary>
    /// Players movement direction.
    /// </summary>
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

    /// <summary>
    /// Sets the player new position depending on direction and speed.
    /// </summary>
    private void Move()
    {
        transform.position += direction * stats.speed * Time.fixedDeltaTime;
        // Debug.Log($"Current move should be: {direction * stats.speed * Time.fixedDeltaTime}");
    }

    /// <summary>
    /// Gets the players input and normalizes it to set the direction.
    /// </summary>
    private void GetPlayersInput()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");
        direction = new Vector3(x: xInput, y: 0, z: zInput).normalized;
    }
}
