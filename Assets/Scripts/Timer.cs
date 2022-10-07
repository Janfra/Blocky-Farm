using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Timer
{
    // For now not used.
    public async Task StartTimer(float duration)
    {
        float timerDuration = Time.time + duration;
        while (Time.time < timerDuration)
        {
            await Task.Yield();
        }
    }
}
