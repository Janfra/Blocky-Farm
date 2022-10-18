using System.Threading.Tasks;
using UnityEngine;

public class Interact_LoadingBar : LoadingBar
{
    /// <summary>
    /// Used to determine if the interaction was cancelled
    /// </summary>
    [HideInInspector]
    public bool isCanceled;

    private void Start()
    {
        isCanceled = false;
        SetVisibility(false);
    }

    #region Loading Handling

    /// <summary>
    /// Starts loading towards the target during an interaction while checking for the conditions to make it valid.
    /// </summary>
    /// <param name="duration">Duration until completed</param>
    /// <param name="InitialValue">Starting value of the slider bar</param>
    /// <param name="targetValue">Target value of the slider bar</param>
    /// <param name="objPos">Object transform to check if it is within interaction range</param>
    /// <returns>If the interaction was completed succesfully</returns>
    public async Task<bool> LoadInteraction(float duration, float InitialValue, float targetValue, Transform objPos)
    {
        isCanceled = false;
        float timerDuration = Time.time + duration;
        float alpha = 0f;
        while (Time.time < timerDuration && IsInteracting(objPos))
        {
            SetVisibility(true);
            alpha += Time.deltaTime;
            sliderBar.value = Lerp(InitialValue, targetValue, duration, alpha);
            await Task.Yield();
        }
        if (!IsInteracting(objPos))
        {
            alpha = 0f;
            float value = sliderBar.value;
            float tempValue = value;
            while (value > InitialValue)
            {
                SetVisibility(true);
                alpha += Time.deltaTime;
                value = Lerp(tempValue, InitialValue, 0.5f, AlphaClamp(alpha, 0.5f));
                sliderBar.value = value;
                await Task.Yield();
            }
            SetVisibility(false);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Check if the interaction is still valid
    /// </summary>
    /// <param name="objPos">Object transform to check distance</param>
    /// <returns>If the interaction is still active and valid</returns>
    private bool IsInteracting(Transform objPos)
    {
        return IsInRange(objPos) && Input.GetKey(KeyCode.E) && !isCanceled;
    }

    /// <summary>
    /// Checks if the given object is still within interaction range
    /// </summary>
    /// <param name="objPos">Object transform to check distance</param>
    /// <returns>If the object is within range</returns>
    private bool IsInRange(Transform objPos)
    {
        return Vector3.Distance(transform.position, objPos.position) < InteractSystem.maxRange;
    }

    #endregion
}
