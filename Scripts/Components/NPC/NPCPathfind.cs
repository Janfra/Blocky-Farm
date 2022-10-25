using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPathfind : Movement
{
    #region Variables

    public Transform pathTarget;

    /// <summary>
    /// List of surfaces that this unit has a penalty on. NOTE: List and not array in case I change them during runtime
    /// </summary>
    [SerializeField] private List<A_Pathfind.SurfaceType> affectedBySurface;

    private Vector3[] path;
    private int targetIndex;
    private bool newRequest;

    #endregion

    private const float pathUpdateMoveThreshold = 0.5f;
    private const float minPathUpdateTime = 0.3f;

    /// <summary>
    /// Always have at least 1 item in the list to avoid errors.
    /// </summary>
    private void OnValidate()
    {
        if (affectedBySurface.Count <= 0)
        {
            affectedBySurface.Add(A_Pathfind.SurfaceType.None);
        }
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }


    #region Pathfinding

    /// <summary>
    /// NPC request a path to follow to the target
    /// </summary>
    /// <param name="newTarget">Target position to achieve</param>
    private void RequestPath(Transform newTarget)
    {
        newRequest = true;
        targetIndex = 0;
        pathTarget = newTarget;
        PathRequestManager.RequestPath(transform.position, pathTarget.position, affectedBySurface.ToArray(), OnPathFound);
        // Debug.Log($"Path requested from {transform.position} to {pathTarget.position}");
    }

    /// <summary>
    /// Once a path is follow, run this method
    /// </summary>
    /// <param name="newPath">Path to follow</param>
    /// <param name="pathSuccess">If the path was found</param>
    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            // Debug.Log("PathFound");
            path = newPath;
            Move();
        }
    }
    protected override void Move()
    {
        StartCoroutine(FollowPath());
    }

    /// <summary>
    /// Starts moving toward the target position
    /// </summary>
    private IEnumerator FollowPath()
    {
        newRequest = false;
        Vector3 currentWaypoint = path[0];

        while (!newRequest)
        {
            if (transform.position == currentWaypoint)
            {
                // Debug.Log($"index: {targetIndex}, target {currentWaypoint}, current: {transform.position}, maxIndex: {path.Length}");
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, GetCurrentSpeed() * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(0.3f);
        }
        RequestPath(pathTarget);

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = pathTarget.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((pathTarget.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                RequestPath(pathTarget);
                targetPosOld = pathTarget.position;
            }
        }
    }

    #endregion

    // Debug
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}