using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGCallback : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<TGCallbackComponent> tgccs = new List<TGCallbackComponent>();

    private bool isPlaying;

    private void OnEnable()
    {
        tgccs.AddRange(GetComponentsInChildren<TGCallbackComponent>());
    }

    private void OnDisable()
    {
        tgccs.Clear();
    }

    public void PlayCallback()
    {
        if (!isPlaying) StartCoroutine(PlayComponents());
    }

    private IEnumerator PlayComponents()
    {
        isPlaying = true;
        foreach (TGCallbackComponent tgcc in tgccs)
        {
            yield return StartCoroutine(tgcc.PlayComponent());
        }
        isPlaying = false;
    }
}
