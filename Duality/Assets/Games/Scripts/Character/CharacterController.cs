using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] protected CharacterInfo characterInfo;
    protected Rigidbody2D rigid;
    protected Collider2D col;
    protected Animator animator;
    protected CharacterPhysic physics;

    protected float inputAxisHorizontal;

    protected virtual void Init()
    {
        col = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        physics = GetComponent<CharacterPhysic>();

        physics.OnInit(transform);
        physics.ChangeGravityInfo(29, 0.15f);
    }

    protected virtual void ReadInput()
    {
        physics.OnPreUpdate();
        physics.OnGravityUpdate(CharacterPhysic.PhysicsState.OnNormal);
        inputAxisHorizontal = InputInfo.PlayerHorizontal.Raw * characterInfo.characterSpeed;
        
        physics.velocity.x = inputAxisHorizontal;

        if (InputInfo.PlayerJump.OnDown && physics.collisions.isGrounded)
        {
            physics.velocity.y = characterInfo.characterJumpForce;
            print("Jump??");
        }
    }

    protected virtual void Move()
    {
        physics.OnUpdate();
    }

    protected virtual void OnTriggerCollisionEnter(Collider2D collision)
    {
        if (collision.TryGetComponent(out ITriggerWithPlayer itwp))
        {
            itwp.OnEnter(this);
        }
    }

    protected virtual void OnTriggerCollisionExit(Collider2D collision)
    {
        if (collision.TryGetComponent(out ITriggerWithPlayer itwp))
        {
            itwp.OnEnter(null);
        }
    }

    #region Unity Function
    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Update()
    {
        ReadInput();
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerCollisionEnter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerCollisionExit(collision);
    }
    #endregion
}
