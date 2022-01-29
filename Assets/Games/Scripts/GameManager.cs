using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Preparation Properties")]
    [SerializeField] private UnityEvent preparationEvent;

    [Header("Restart Properties")]
    [SerializeField] private RestartType restartYpe;
    [SerializeField] private UnityEvent whiteSideRestartEvent;
    [SerializeField] private UnityEvent blackSideRestartEvent;

    [SerializeField, ReadOnly] private GameState currentState;
    private StateMachine<GameState> state;

    private List<IGameState> gameStateListeners = new List<IGameState>();

    public void RegisterGameStateListener(IGameState igs)
    {
        gameStateListeners.Add(igs);
    }

    public void UnregisterGameStateListener(IGameState igs)
    {
        gameStateListeners.Remove(igs);
    }

    public void RestartGame()
    {
        switch (restartYpe)
        {
            case RestartType.WhiteSide:
                RestartFromWhiteSide();
                break;
            case RestartType.BlackSide:
                RestartFromBlackSide();
                break;
        }
    }

    private void RestartFromWhiteSide()
    {
        whiteSideRestartEvent?.Invoke();
    }

    private void RestartFromBlackSide()
    {
        blackSideRestartEvent?.Invoke();
    }

    private void Awake()
    {
        state = StateMachine<GameState>.Initialize(this);
        state.Changed += st => currentState = st;
        state.ChangeState(GameState.Initialization);
    }

    #region States
    private void Initialization_Enter()
    {
        preparationEvent?.Invoke();
        state.ChangeState(GameState.Gameplay);
    }

    private void Gameplay_Update()
    {
        if (InputInfo.UIPause.OnDown)
        {
            PauseListener();
            state.ChangeState(GameState.Pause);
        }
    }

    private void Pause_Update()
    {
        if (InputInfo.UIPause.OnDown)
        {
            ResumeListener();
            state.ChangeState(GameState.Gameplay);
        }
    }
    #endregion

    private void PauseListener()
    {
        foreach (IGameState igs in gameStateListeners)
        {
            igs.Pause();
        }
    }

    private void ResumeListener()
    {
        foreach (IGameState igs in gameStateListeners)
        {
            igs.Resume();
        }
    }

    public enum GameState { None, Initialization, Gameplay, Pause }
    public enum RestartType { WhiteSide, BlackSide }
}
