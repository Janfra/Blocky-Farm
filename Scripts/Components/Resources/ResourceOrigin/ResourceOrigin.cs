using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interact_LoadingBar))]
public class ResourceOrigin : MonoBehaviour, IInteractable, IGridUpdatable
{
    #region Variables

    /// <summary>
    /// Prefab information.
    /// </summary>
    [SerializeField] protected ItemData resourcePrefab;

    /// <summary>
    /// Changes the mesh to match the visual.
    /// </summary>
    [SerializeField] protected MeshFilter meshFilter;

    /// <summary>
    /// Visual of each stage.
    /// </summary>
    [SerializeField] protected List<Mesh> stageMesh;

    /// <summary>
    /// Duration of the transition to next stage.
    /// </summary>
    [SerializeField] protected float durationPerStage;

    /// <summary>
    /// Timer to be used in between stages and UI handling.
    /// </summary>
    [SerializeField] protected Interact_LoadingBar interactLoadBar;

    /// <summary>
    /// Index for current state of the mesh.
    /// </summary>
    protected int currentStageIndex;

    /// <summary>
    /// Sets if it is possible to interact with the object.
    /// </summary>
    protected bool isInteractable;

    #endregion

    #region IGridUpdatable Index

    public int GridUpdateIndex
    {
        get
        {
            return gridUpdateIndex;
        }
        set
        {
            
        }
    }
    private int gridUpdateIndex;

    #endregion

    #region Constants

    /// <summary>
    /// Max stage for the index.
    /// </summary>
    private const int maxStageIndex = 2;

    /// <summary>
    ///  Added to the currentStage index to match loading bar values.
    /// </summary>
    private const int stageOffset = 1;

    #endregion

    void Start()
    {
        OnStart();
    }

    protected void OnStart()
    {
        currentStageIndex = 0;
        isInteractable = true;
        if (durationPerStage == 0f)
        {
            durationPerStage = 1f;
            Debug.LogError($"Duration Per Stage in {gameObject.name} has not been set");
        }
        
        // Loading
        interactLoadBar.Init(maxStageIndex + stageOffset, meshFilter.mesh, transform);
        interactLoadBar.isLoadingSuccessful = LoadResult;

        // Grid Updating
        gridUpdateIndex = GridUpdateManager.Instance.GetIndex(gameObject);
    }

    #region Interact

    /// <summary>
    /// Starts interaction
    /// </summary>
    /// <param name="playerPos">Players position to check if its in range</param>
    public void OnInteract(Transform playerPos)
    {
        if (!isInteractable)
            return;

        isInteractable = false;
        StartLoading(playerPos);
    }

    /// <summary>
    /// Starts the handling of the loading and the result.
    /// </summary>
    /// <param name="playerPos"></param>
    protected void StartLoading(Transform playerPos)
    {
        StartCoroutine(interactLoadBar.StartInteraction(durationPerStage, currentStageIndex, currentStageIndex + stageOffset, playerPos));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isLoadSuccessful"></param>
    /// <param name="playerPos"></param>
    protected void LoadResult(bool isLoadSuccessful, Transform playerPos)
    {
        if (!isLoadSuccessful)
        {
            isInteractable = true;
            return;
        }
        if (IsNotLastStage())
        {
            UpdateMeshStage();
        }
        else
        {
            GenerateResource();
            return;
        }
        StartLoading(playerPos);
    }

    /// <summary>
    /// Updates the mesh filter to the next stage, while updating relevant scripts
    /// </summary>
    protected void UpdateMeshStage()
    {
        currentStageIndex++;
        meshFilter.mesh = stageMesh[currentStageIndex];

        // Updates load bar position and mesh size for grid updates
        interactLoadBar.SetOnTop(meshFilter.mesh, transform);
        Static_GridUpdating.MeshUpdated(meshFilter.mesh, this);
    }

    /// <summary>
    /// Generates a resource and disables game object 
    /// </summary>
    protected void GenerateResource()
    {
        ObjectPooler.Instance.SpawnFromPool(resourcePrefab.PoolIndex, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Checks if the resource is in its last stage
    /// </summary>
    /// <returns>Is the resource not at its last stage</returns>
    protected bool IsNotLastStage()
    {
        return currentStageIndex != maxStageIndex;
    }



    #endregion
}