using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TGCallbackComponent : MonoBehaviour
{
    [SerializeField] protected float delay;
    [SerializeField] protected float duration;
    [SerializeField] protected UnityEvent callbackEvent;

    public virtual IEnumerator PlayComponent()
    {
        yield return new WaitForSeconds(delay);
        callbackEvent?.Invoke();
        yield return new WaitForSeconds(duration);
    }
}
