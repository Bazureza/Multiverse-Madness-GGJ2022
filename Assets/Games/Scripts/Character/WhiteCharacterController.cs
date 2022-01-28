using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCharacterController : CharacterController
{
    [SerializeField] private CharacterCloneController cloneController;

    public void StartRecordClone()
    {
        cloneController.StartRecording(transform);
    }

    public void PlayRecordedClone()
    {
        cloneController.StopRecording();
        cloneController.PlayingTape();
    }

    public void StopRecordedClone()
    {
        cloneController.StopTape();
    }
}
