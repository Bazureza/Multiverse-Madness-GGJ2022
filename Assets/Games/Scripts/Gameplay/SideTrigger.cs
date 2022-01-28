using UnityEngine;
using UnityEngine.Events;

public class SideTrigger : MonoBehaviour, ITriggerWithPlayer
{
    [SerializeField] protected bool disableAfterInteract;
    [SerializeField] protected UnityEvent triggerEnterEvent;

    protected virtual void TriggerOnEnter(CharacterController characterController)
    {

    }

    protected virtual void TriggerOnExit(CharacterController characterController)
    {

    }

    void ITriggerWithPlayer.OnEnter(CharacterController characterController)
    {
        TriggerOnEnter(characterController);
    }

    void ITriggerWithPlayer.OnExit(CharacterController characterController)
    {
        TriggerOnExit(characterController);
    }

    void ITriggerWithPlayer.OnStay(CharacterController characterController)
    {
        
    }

    public enum SideType { White, Black }
}
