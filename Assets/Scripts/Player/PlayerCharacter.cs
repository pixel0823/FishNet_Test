using System.Collections;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerCharacter_Full : BaseCharacter
{
    [Header("Player Stats")]
    public float stamina = 100f;
    public float hunger = 100f;
    public float oxygen = 100f;
    public float temperature = 37f;

    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    [Header("Attack / Interaction")]
    public float attackRange = 1f;
    public int attackDamage = 10;
    public LayerMask attackLayer;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isActionPlaying = false;

    public PlayerState currentPlayerState = PlayerState.Idle;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void ChangeStateOnDamage()
    {
        ChangeStateTo(PlayerState.TakeDamage);
    }

    public override void Die()
    {
        ChangeStateTo(PlayerState.Die);
        rb.velocity = Vector2.zero;
        Debug.Log("Player has died.");
        StartCoroutine(RespawnCoroutine());
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (!isActionPlaying)
        {
            HandleInput();
        }

        HandleStamina();
        HandleTemperature();
        HandleRecovery();
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        Move();
    }

    void HandleInput()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        else if (Input.GetKey(KeyCode.D)) horizontal = 1f;

        if (Input.GetKey(KeyCode.W)) vertical = 1f;
        else if (Input.GetKey(KeyCode.S)) vertical = -1f;

        moveInput = new Vector2(horizontal, vertical).normalized;

        // 기본 이동 상태
        if (moveInput.magnitude > 0) ChangeStateTo(PlayerState.Walk);
        else ChangeStateTo(PlayerState.Idle);

        // 달리기
        if (Input.GetKey(KeyCode.LeftShift) && moveInput.magnitude > 0 && stamina > 0)
        {
            ChangeStateTo(PlayerState.Run);
            stamina -= 20f * Time.deltaTime;
        }
        else
        {
            if (currentPlayerState == PlayerState.Run) ChangeStateTo(PlayerState.Walk);
            stamina += 10f * Time.deltaTime;
        }
        stamina = Mathf.Clamp(stamina, 0f, 100f);

        // 공격/채집
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(PerformAction(PlayerState.AttackOrGather));
        }

        // 식사
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PerformAction(PlayerState.Eat));
        }

        // 수면
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(PerformAction(PlayerState.Sleep));
        }
    }

    void Move()
    {
        if (isActionPlaying) return;

        float speed = currentPlayerState == PlayerState.Run ? runSpeed : walkSpeed;
        rb.velocity = moveInput * speed;

        // 좌우 반전
        if (moveInput.x != 0)
            spriteRenderer.flipX = moveInput.x < 0;

        animator.SetFloat("MoveY", moveInput.y);
        animator.SetFloat("Speed", moveInput.magnitude);
    }

    void HandleStamina()
    {
        if (stamina <= 0f && currentPlayerState == PlayerState.Run)
        {
            ChangeStateTo(PlayerState.Walk);
        }
    }

    void HandleTemperature()
    {
        if (temperature < 35f)
        {
            ChangeStateTo(PlayerState.Cold);
            currentHP -= 5f * Time.deltaTime;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            if (currentHP <= 0) Die();
        }
        else if (currentPlayerState == PlayerState.Cold && temperature >= 35f)
        {
            ChangeStateTo(PlayerState.Idle);
        }

        temperature -= 0.1f * Time.deltaTime;
        temperature = Mathf.Clamp(temperature, 30f, 40f);
    }

    void HandleRecovery()
    {
        // 간단 회복 예시: 허기/산소/체온에 따른 체력 회복
        if (currentPlayerState != PlayerState.Die && currentHP < maxHP)
        {
            if (hunger > 50f && oxygen > 50f && temperature > 35f)
            {
                currentHP += 2f * Time.deltaTime;
                currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            }
        }
    }

    IEnumerator PerformAction(PlayerState actionState)
    {
        isActionPlaying = true;
        ChangeStateTo(actionState);

        // 공격/채집 처리
        if (actionState == PlayerState.AttackOrGather)
        {
            // 범위 내 적/오브젝트 판정
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, attackLayer);
            foreach (var hit in hits)
            {
                var target = hit.GetComponent<BaseCharacter>();
                if (target != null)
                {
                    target.photonView.RPC("TakeDamage", RpcTarget.All, attackDamage);
                }
            }
        }

        // 애니메이션 길이 대기
        float animLength = 0.5f; // 예: 공격/식사/수면 애니 길이
        yield return new WaitForSeconds(animLength);

        isActionPlaying = false;
        ChangeStateTo(PlayerState.Idle);
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(3f); // 3초 후 부활
        currentHP = maxHP;
        stamina = 100f;
        ChangeStateTo(PlayerState.Revive);
        yield return new WaitForSeconds(0.5f); // 부활 애니메이션 시간
        ChangeStateTo(PlayerState.Idle);
    }

    void ChangeStateTo(PlayerState newState)
    {
        if (currentPlayerState == newState) return;
        currentPlayerState = newState;
        currentState = (int)newState;
        photonView.RPC(nameof(ChangeState), RpcTarget.All, (int)newState);
        if (animator != null) animator.SetInteger("State", (int)newState);

    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);

        if (stream.IsWriting)
        {
            stream.SendNext(stamina);
            stream.SendNext(hunger);
            stream.SendNext(oxygen);
            stream.SendNext(temperature);
            stream.SendNext((int)currentPlayerState);
        }
        else
        {
            stamina = (float)stream.ReceiveNext();
            hunger = (float)stream.ReceiveNext();
            oxygen = (float)stream.ReceiveNext();
            temperature = (float)stream.ReceiveNext();
            currentPlayerState = (PlayerState)(int)stream.ReceiveNext();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
