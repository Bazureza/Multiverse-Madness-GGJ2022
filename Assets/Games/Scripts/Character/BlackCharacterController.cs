using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCharacterController : CharacterController
{
    public void TeleportTo(Transform target)
    {
        transform.position = target.position;
    }

    protected override void ReadInput()
    {
        physics.OnPreUpdate();
        physics.OnGravityUpdate(CharacterPhysic.PhysicsState.OnNormal);
        inputAxisHorizontal = InputInfo.PlayerHorizontal.Raw * characterInfo.characterSpeed * -1f;

        physics.velocity.x = inputAxisHorizontal;

        if (InputInfo.PlayerJump.OnDown && physics.collisions.isGrounded)
        {
            physics.velocity.y = characterInfo.characterJumpForce;
            print("Jump??");
        }
    }
}
