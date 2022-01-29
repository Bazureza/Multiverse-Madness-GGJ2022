using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TomGustin.GameDesignPattern;
using UnityEngine;
using UnityEngine.Events;

public class WhiteSideTimer : MonoBehaviour, IGameState
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float time;
    [SerializeField, ReadOnly] private float timerElapsed;
    [SerializeField] private UnityEvent timerEndEvent;

    private bool timeActive;

    private Tween tweenTimer;

    private void OnEnable()
    {
        ServiceLocator.Resolve<GameManager>().RegisterGameStateListener(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Resolve<GameManager>().UnregisterGameStateListener(this);
    }

    [Button]
    public void StartTimer()
    {
        if (timeActive) return;
        timeActive = true;
        tweenTimer = DOVirtual.Float(time, 0f, time, x =>
            {
                timerElapsed = x;
                UpdateTimerText();
            })
            .SetEase(Ease.Linear)
            .OnComplete(()=>
            {
                timerEndEvent?.Invoke();
                timeActive = false;
            });
    }

    public void StopTimer()
    {
        tweenTimer.Kill();
        timeActive = false;
        if (timerText) timerText.text = "-";
    }

    private void UpdateTimerText()
    {
        if (timerText) timerText.text = timerElapsed.ToString("F2");
    }

    void IGameState.Pause()
    {
        tweenTimer.Pause();
    }

    void IGameState.Resume()
    {
        tweenTimer.Play();
    }
}
