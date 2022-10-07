using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceOrigin : MonoBehaviour, IInteractable
{
    #region Variables

    /// <summary>
    /// Prefab information.
    /// </summary>
    [SerializeField] private ItemData resourcePrefab;

    /// <summary>
    /// Changes the mesh to match the visual.
    /// </summary>
    [SerializeField] private MeshFilter meshFilter;

    /// <summary>
    /// Visual of each stage.
    /// </summary>
    [SerializeField] private List<Mesh> stageMesh;

    /// <summary>
    /// Duration of the transition to next stage.
    /// </summary>
    [SerializeField] private float durationPerStage;

    /// <summary>
    /// Timer to be used in between stages and UI handling.
    /// </summary>
    [SerializeField] private Interact_LoadingBar interactLoadBar;

    /// <summary>
    /// Index for current state of the mesh.
    /// </summary>
    private int currentStageIndex;

    /// <summary>
    /// Sets if it is possible to interact with the object.
    /// </summary>
    private bool isInteractable;

    /// <summary>
    /// Checks if the player is holding the interact button
    /// </summary>
    private bool isKeyHold => Input.GetKey(KeyCode.E);

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
        currentStageIndex = 0;
        isInteractable = true;
        if(durationPerStage == 0f)
        {
            durationPerStage = 1f;
            Debug.LogError($"Duration Per Stage in {gameObject.name} has not been set");
        }
        interactLoadBar.SetSliderMax(maxStageIndex + stageOffset);
    }

    #region Interact

    /// <summary>
    /// Starts interaction, is successful change stage until completed to generate resource.
    /// </summary>
    /// <param name="playerPos">Players position to check if its in range</param>
    public async void OnInteract(Transform playerPos)
    {
        if (!isInteractable)
            return;
 
        isInteractable = false;
        bool isLoading;
        while (IsUpdatable())
        {
            isLoading = await interactLoadBar.LoadInteraction(durationPerStage, currentStageIndex, currentStageIndex + stageOffset, playerPos);
            UpdateMeshStage(isLoading);
        }
        isLoading = await interactLoadBar.LoadInteraction(durationPerStage, currentStageIndex, currentStageIndex + stageOffset, playerPos);
        GenerateResource(isLoading);
    }

    /// <summary>
    /// Updates the mesh filter to the next stage if successful.
    /// </summary>
    /// <param name="isLoading">Was the interaction loading successful</param>
    private void UpdateMeshStage(bool isLoading)
    {
        if (!isLoading)
            return;

        currentStageIndex++;
        meshFilter.mesh = stageMesh[currentStageIndex];
    }

    /// <summary>
    /// Generates a resource and disables game object if interaction was successful
    /// </summary>
    /// <param name="isLoading">Was the loading successful</param>
    private void GenerateResource(bool isLoading)
    {
        if (!isKeyHold || !isLoading)
        {
            isInteractable = true;
            return;
        }
        
        ObjectPooler.Instance.SpawnFromPool(resourcePrefab.PoolIndex, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Checks that the conditions to be able to keep running the while loop are still true
    /// </summary>
    /// <returns>Is the interaction still valid or at the last stage</returns>
    private bool IsUpdatable()
    {
        return currentStageIndex != maxStageIndex && isKeyHold;
    }

    #endregion
}
