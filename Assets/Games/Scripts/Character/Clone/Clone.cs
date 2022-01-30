using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

public class Clone : MonoBehaviour, ITriggerWithPlayer
{
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private Animator anim;

    private Vector2 lastKnownPosition;

    private GameManager gameManager
    {
        get
        {
            if (_gameManager == null) _gameManager = ServiceLocator.Resolve<GameManager>();
            return _gameManager;
        }
    }
    private GameManager _gameManager;


    public void TrackTransform(Vector2 lastKnown)
    {
        if (lastKnown.x > lastKnownPosition.x)
        {
            anim.SetFloat("movement", 1f);
            render.flipX = false;
        } else if (lastKnown.x < lastKnownPosition.x)
        {
            anim.SetFloat("movement", 1f);
            render.flipX = true;
        } else
        {
            anim.SetFloat("movement", 0f);
        }
        lastKnownPosition = lastKnown;
    }

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
