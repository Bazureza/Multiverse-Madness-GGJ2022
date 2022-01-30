using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

public class SimpleAudio : Singleton<SimpleAudio>
{
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip move;

    private void Awake()
    {
        OnInitialize();
    }

    public void PlaySFXJump()
    {
        sfx.PlayOneShot(jump);
    }

    public void PlaySFXMove()
    {
        sfx.PlayOneShot(move);
    }
}
