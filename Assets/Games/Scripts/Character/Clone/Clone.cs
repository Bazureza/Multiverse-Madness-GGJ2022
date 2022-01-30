using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

public class Clone : MonoBehaviour, ITriggerWithPlayer
{
    [SerializeField] private Animator anim;

    private GameManager gameManager
    {
        get
        {
            if (_gameManager == null) _gameManager = ServiceLocator.Resolve<GameManager>();
            return _gameManager;
        }
    }
    private GameManager _gameManager;

    void ITriggerWithPlayer.OnEnter(CharacterController characterController)
    {
        gameManager.RestartGame();
    }

    void ITriggerWithPlayer.OnExit(CharacterController characterController)
    {
        
    }

    void ITriggerWithPlayer.OnStay(CharacterController characterController)
    {
        
    }
}
