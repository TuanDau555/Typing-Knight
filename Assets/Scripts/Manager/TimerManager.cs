using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class TimerManager : MonoBehaviour
{
    #region Parameters
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float targetTime = 180f;
    [SerializeField] private bool isCountdownMode = true;
    public float CurrentTime { get; private set; }
    public float ElapsedTime { get; private set; }
    public UnityEvent OnTimeUp;
    public UnityEvent OnTimeReachedTarget;
    private bool isRuning = false;
    #endregion

    public void Initialize(float target, bool countdown)
    {
        targetTime = target;
        isCountdownMode = countdown;
        Reset();
    }

    public void Reset()
    {
        ElapsedTime = 0f;
        CurrentTime = isCountdownMode ? targetTime : 0f;
        UpdateUI();
        isRuning = false;
    }

    public void StartTimer()
    {
        isRuning=true;
    }
    public void StopTimer()
    {
        isRuning=false;
    }

    private void Update()
    {
        if (!isRuning) { return; }
        ElapsedTime += Time.deltaTime;
        if (isCountdownMode)
        {
            CurrentTime = Mathf.Max(0, targetTime - ElapsedTime);
            UpdateUI();
            if (CurrentTime <= 0f)
            {
                CurrentTime = 0f;
                StopTimer();
                OnTimeUp?.Invoke();
            }
        }
        else 
        {
            CurrentTime = ElapsedTime;
            UpdateUI();
            if (CurrentTime >= targetTime) { OnTimeReachedTarget?.Invoke(); }
        }
    }
    private void UpdateUI()
    {
        if (timerText == null) return;
        float t = Mathf.Max(0, CurrentTime);
        int min = Mathf.FloorToInt(t/60);
        int sec = Mathf.FloorToInt(t%60);
        timerText.text = $"{min:00}:{sec:00}";
    }

    public void SetMode(bool countdown, bool reset = true)
    {
        isCountdownMode = countdown;
        if (reset) Reset();
    }

}
