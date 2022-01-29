using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using TomGustin.GameDesignPattern;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private DataHolder dataHolder;

    [Header("Preparation Properties")]
    [SerializeField] private UnityEvent preparationEvent;

    [Header("Restart Properties")]
    [SerializeField] private RestartType restartYpe;
    [SerializeField] private UnityEvent whiteSideRestartEvent;
    [SerializeField] private UnityEvent blackSideRestartEvent;

    [SerializeField] private GameObject pausePanel;

    [SerializeField, ReadOnly] private GameState currentState;
    private StateMachine<GameState> state;

    private LevelManager levelManager;

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

    public void SaveProgress()
    {
        dataHolder.saveData.AddLevel(levelManager.GetLevelID());
        DataManager.SaveGameData(dataHolder.saveData);
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            pausePanel.SetActive(true);
            PauseListener();
            state.ChangeState(GameState.Pause);
        } else
        {
            pausePanel.SetActive(false);
            ResumeListener();
            state.ChangeState(GameState.Gameplay);
        }
    }

    public void FreezeGame(bool pause)
    {
        if (pause)
        {
            PauseListener();
            state.ChangeState(GameState.None);
        }
        else
        {
            ResumeListener();
            state.ChangeState(GameState.Gameplay);
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

        levelManager = ServiceLocator.Resolve<LevelManager>();
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
            PauseGame(true);
        }
    }

    private void Pause_Update()
    {
        if (InputInfo.UIPause.OnDown)
        {
            PauseGame(false);
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
