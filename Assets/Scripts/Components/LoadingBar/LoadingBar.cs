using UnityEngine;
using UnityEngine.UI;

abstract public class LoadingBar : MonoBehaviour
{
    /// <summary>
    /// Slider that will act as the loading bar
    /// </summary>
    [SerializeField] protected Slider sliderBar;

    /// <summary>
    /// UI canvas to disable and unable visibility
    /// </summary>
    [SerializeField] protected GameObject canvas;

    private void Start()
    {
        SetVisibility(false);
    }

    #region Clamp & Setters

    /// <summary>
    /// Sets the slider max value
    /// </summary>
    /// <param name="newMax">New max value of the slider</param>
    public void SetSliderMax(float newMax)
    {
        sliderBar.maxValue = newMax;
        sliderBar.value = newMax;
    }

    /// <summary>
    /// Sets the canvas as active or disable
    /// </summary>
    /// <param name="isVisible">Is canvas/UI active</param>
    public void SetVisibility(bool isVisible)
    {
        canvas.gameObject.SetActive(isVisible);
    }

    /// <summary>
    /// Clamps 'alpha' to avoid it going over the duration and exceeding the target.
    /// </summary>
    /// <param name="alpha">Value to clamp</param>
    /// <param name="duration">Max value</param>
    /// <returns>Value within range</returns>
    protected float AlphaClamp(float alpha, float duration)
    {
        if (alpha >= duration)
        {
            return duration;
        }
        else
        {
            return alpha;
        }
    }

    #endregion

    #region Lerping

    /// <summary>
    /// Interpolates between point A and B within the given duration.
    /// </summary>
    /// <param name="InitialValue">Sets the starting value or point A</param>
    /// <param name="targetValue">Sets the target to achieve or point B</param>
    /// <param name="duration">Sets the total duration of the lerp</param>
    /// <param name="alpha">Sets at which point in between A and B it currently is</param>
    /// <returns></returns>
    protected float Lerp(float InitialValue, float targetValue, float duration, float alpha)
    {
        return (InitialValue + (targetValue - InitialValue) * FractionAlphaNormalized(alpha, duration));
    }

    /// <summary>
    /// Normalizes 'alpha' with the total duration.
    /// </summary>
    /// <param name="alpha">Current value to normalize</param>
    /// <param name="duration">Max value of the normalize formula</param>
    /// <returns>A value in between 0 and 1, the duration being 1</returns>
    protected float FractionAlphaNormalized(float alpha, float duration)
    {
        // 1 is added to everything to avoid dividing under 0 and getting unexpected values
        int minValue = 1;
        alpha += minValue;
        duration += minValue;
        return (alpha - minValue) / (duration - minValue);
    }

    #endregion

    #region Outdated

    // Won't currently need a load bar that just loads, want to avoid tasks for now as I need to look further into cancellationTokens
    //public async Task StartLoading(float duration, float currentValue, float targetValue)
    //{
    //    float timerDuration = Time.time + duration;
    //    float alpha = 0;
    //    while (Time.time < timerDuration)
    //    {
    //        alpha += Time.deltaTime;
    //        loadingBar.value = Lerp(currentValue, targetValue, duration, alpha);
    //        await Task.Yield();
    //    }
    //}

    // '0' would be the min value of the normalized set, but in this case it is always from 0 seconds to duration, so no need.
    // NOTE: It produces problems with fractional values but in this class it works, StoredLoadingBar fixed it, replaced by FractionalAlphaNormalized
    //private float AlphaNormalized(float alpha, float duration)
    //{
    //    float normalized = (alpha - 0) / (duration - 0);
    //    Debug.Log($"{normalized} result, {alpha} alpha, {duration} duration");
    //    return normalized;
    //}

    #endregion
}
