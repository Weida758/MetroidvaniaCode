using UnityEngine;

public class TimerDash : MonoBehaviour
{
    public float stateTimer;
    public float stateTimerDuration = 3;

    void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public void SetTimer()
    {
        stateTimer = stateTimerDuration;
    }
}
