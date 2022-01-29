using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCharacterController : CharacterController
{
    [SerializeField] private CharacterCloneController cloneController;
    private Vector2 firstSpawnedLocation;

    protected override void Awake()
    {
        base.Awake();

        firstSpawnedLocation = transform.position;
    }

    public void TeleportToFirstSpawn()
    {
        transform.position = firstSpawnedLocation;
    }

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
