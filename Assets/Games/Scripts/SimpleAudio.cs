using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

public class SimpleAudio : Singleton<SimpleAudio>
{
    private void Awake()
    {
        OnInitialize();
    }
}
