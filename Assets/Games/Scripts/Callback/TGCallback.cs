using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

public class TGCallback : MonoBehaviour
{
    [SerializeField] private bool freezePlayerOnPlay;
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
        if (freezePlayerOnPlay) ServiceLocator.Resolve<GameManager>()?.FreezeGame(true);
        foreach (TGCallbackComponent tgcc in tgccs)
        {
            yield return StartCoroutine(tgcc.PlayComponent());
        }
        if (freezePlayerOnPlay) ServiceLocator.Resolve<GameManager>()?.FreezeGame(false);
        isPlaying = false;
    }
}
