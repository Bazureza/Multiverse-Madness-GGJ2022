using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventCaller : MonoBehaviour
{
    public UnityEvent eventInvoked;

    public void InvokeEvent()
    {
        eventInvoked?.Invoke();
    }
}
