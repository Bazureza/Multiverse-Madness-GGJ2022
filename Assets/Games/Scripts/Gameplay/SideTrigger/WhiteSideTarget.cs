﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteSideTarget : SideTrigger
{
    protected override void TriggerOnEnter(CharacterController characterController)
    {
        triggerEnterEvent?.Invoke();
        if (disableAfterInteract) gameObject.GetComponent<Collider2D>().enabled = false;
    }

    protected override void TriggerOnExit(CharacterController characterController)
    {
        base.TriggerOnExit(characterController);
    }
}
