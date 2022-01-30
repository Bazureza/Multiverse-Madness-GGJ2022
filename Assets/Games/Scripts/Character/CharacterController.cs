using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

public class CharacterController : MonoBehaviour, IGameState
{
    [SerializeField] protected GameObject visual;
    [SerializeField] protected SpriteRenderer visualImage;
    [SerializeField] protected CharacterInfo characterInfo;
    [SerializeField] private bool facingRight;
    [SerializeField, ReadOnly] protected CharacterState currentState;
    protected Rigidbody2D rigid;
    protected Collider2D col;
    protected Animator animator;
    protected CharacterPhysic physics;

    protected float inputAxisHorizontal;

    private bool freeze;

    protected GameManager gameManager;
    protected StateMachine<CharacterState> state;

    public enum CharacterState { None, Normal, Pause }

    protected virtual void Init()
    {
        state = StateMachine<CharacterState>.Initialize(this);
        state.Changed += st => currentState = st;

        col = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = visual.GetComponent<Animator>();
        physics = GetComponent<CharacterPhysic>();
        gameManager = ServiceLocator.Resolve<GameManager>();

        physics.OnInit(transform);
        physics.ChangeGravityInfo(characterInfo.gravity, characterInfo.gravityMultiplier);

        state.ChangeState(CharacterState.Normal);
    }

    public void Unhide()
    {
        visual.SetActive(true);
        freeze = false;
        col.enabled = true;
    }

    public void Hide()
    {
        visual.SetActive(false);
        freeze = true;
        col.enabled = false;
    }

    protected virtual void ReadInput()
    {
        animator.ResetTrigger("jump");
        animator.ResetTrigger("fall");

        physics.OnPreUpdate();
        physics.OnGravityUpdate(CharacterPhysic.PhysicsState.OnNormal);
        inputAxisHorizontal = InputInfo.PlayerHorizontal.Raw * characterInfo.characterSpeed;
        
        physics.velocity.x = inputAxisHorizontal;

        if (InputInfo.PlayerJump.OnDown && physics.collisions.isGrounded)
        {
            physics.velocity.y = characterInfo.characterJumpForce;
            animator.SetTrigger("jump");
        }
    }

    protected virtual void UpdateAnimation()
    {
        if (inputAxisHorizontal > 0) facingRight = true;
        if (inputAxisHorizontal < 0) facingRight = false;

        visualImage.flipX = !facingRight;
        animator.SetFloat("movement", Mathf.Abs(inputAxisHorizontal));
    }

    protected virtual void Move()
    {
        if (physics.collisions.isGrounded && !physics.collisions.isGroundedPrev)
        {
            animator.SetTrigger("fall");
            print("grounded");
        }

        physics.OnUpdate();
    }

    protected virtual void OnTriggerCollisionEnter(Collider2D collision)
    {
        if (freeze) return;
        if (collision.TryGetComponent(out ITriggerWithPlayer itwp))
        {
            itwp.OnEnter(this);
        }
    }

    protected virtual void OnTriggerCollisionExit(Collider2D collision)
    {
        if (freeze) return;
        if (collision.TryGetComponent(out ITriggerWithPlayer itwp))
        {
            itwp.OnExit(null);
        }
    }

    #region Unity Function
    protected void OnEnable()
    {
        gameManager.RegisterGameStateListener(this);
    }

    protected void OnDisable()
    {
        gameManager.UnregisterGameStateListener(this);
    }

    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Normal_Update()
    {
        if (freeze) return;
        ReadInput();
        Move();
        UpdateAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerCollisionEnter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerCollisionExit(collision);
    }

    void IGameState.Pause()
    {
        state.ChangeState(CharacterState.Pause);
    }

    void IGameState.Resume()
    {
        state.ChangeState(CharacterState.Normal);
    }
    #endregion
}
