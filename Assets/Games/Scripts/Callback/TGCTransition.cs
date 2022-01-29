using System.Collections;
using TomWill;
using UnityEngine;

public class TGCTransition : TGCallbackComponent
{
    [SerializeField] private TWTransition.TransitionType transitionType;
    public override IEnumerator PlayComponent()
    {
        yield return new WaitForSeconds(delay);
        TWTransition.ScreenTransition(transitionType, duration);
        yield return new WaitForSeconds(duration);
    }
}
