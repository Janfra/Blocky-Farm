using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Interact_LoadingBar : LoadingBar
{
    /// <summary>
    /// Used to determine if the interaction was cancelled
    /// </summary>
    [HideInInspector]
    public bool isCanceled;
    public Action<bool, Transform> isLoadingSuccessful;

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
    /// <param name="initialValue">Starting value of the slider bar</param>
    /// <param name="targetValue">Target value of the slider bar</param>
    /// <param name="objPos">Object transform to check if it is within interaction range</param>
    /// <returns>If the interaction was completed succesfully</returns>
    public async Task<bool> LoadInteraction(float duration, float initialValue, float targetValue, Transform objPos)
    {
        isCanceled = false;
        float timerDuration = Time.time + duration;
        float alpha = 0f;
        while (Time.time < timerDuration && IsInteracting(objPos))
        {
            SetVisibility(true);
            alpha += Time.deltaTime;
            sliderBar.value = Lerp(initialValue, targetValue, duration, alpha);
            await Task.Yield();
        }
        if (!IsInteracting(objPos))
        {
            alpha = 0f;
            float value = sliderBar.value;
            float tempValue = value;
            while (value > initialValue)
            {
                SetVisibility(true);
                alpha += Time.deltaTime;
                value = Lerp(tempValue, initialValue, 0.5f, AlphaClamp(alpha, 0.5f));
                sliderBar.value = value;
                await Task.Yield();
            }
            Debug.Log(value);
            SetVisibility(false);
            return false;
        }
        Debug.Log(sliderBar.value);
        return true;
    }

    public IEnumerator StartInteraction(float duration, float initialValue, float targetValue, Transform objPos)
    {
        isCanceled = false;
        float value = sliderBar.value;
        float alpha = 0f;
        while (value != targetValue && IsInteracting(objPos))
        {
            SetVisibility(true);
            alpha += Time.deltaTime;
            value = Lerp(initialValue, targetValue, duration, AlphaClamp(alpha, duration));
            sliderBar.value = value;
            yield return null;
        }
        // If the interaction fails, start loading back to initial value.
        if (!IsInteracting(objPos))
        {
            alpha = 0f;
            value = sliderBar.value;
            float newInitialValue = value;
            while (value > initialValue)
            {
                SetVisibility(true);
                alpha += Time.deltaTime;
                value = Lerp(newInitialValue, initialValue, 0.5f, AlphaClamp(alpha, 0.5f));
                sliderBar.value = value;
                yield return null;
            }
            SetVisibility(false);
            isLoadingSuccessful.Invoke(false, objPos);
            yield break;
        }
        isLoadingSuccessful.Invoke(true, objPos);
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
