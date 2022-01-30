using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCharacterController : CharacterController
{
    private Vector2 firstSpawnedLocation;

    public void TeleportTo(Transform target)
    {
        firstSpawnedLocation = target.position;
        transform.position = target.position;
    }

    public void TeleportToFirstSpawn()
    {
        transform.position = firstSpawnedLocation;
    }

    protected override void ReadInput()
    {
        animator.ResetTrigger("jump");
        animator.ResetTrigger("fall");
        physics.OnPreUpdate();
        physics.OnGravityUpdate(CharacterPhysic.PhysicsState.OnNormal);
        inputAxisHorizontal = InputInfo.PlayerHorizontal.Raw * characterInfo.characterSpeed * -1f;
        //UpdateAnimation();
        physics.velocity.x = inputAxisHorizontal;

        if (InputInfo.PlayerJump.OnDown && physics.collisions.isGrounded)
        {
            physics.velocity.y = characterInfo.characterJumpForce;
            animator.SetTrigger("jump");
            SimpleAudio.Instance?.PlaySFXJump();
        }
    }
}
