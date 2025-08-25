using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// 캐릭터 기본 스탯 (플레이어 & 몬스터 공통)
/// </summary>

public abstract class BaseCharacter : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Base Stats")]
    public float maxHP = 100f;
    public float currentHP = 100f;

    public float attackPower = 10f;
    public float defense = 5f;
    public float attackSpeed = 1f; // 공격 속도 (초당 공격 횟수)

    [Header("Animation/State")]
    public int currentState = 0;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 데미지 처리 (RPC로 전체 공유)
    [PunRPC]
    public virtual void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(damage - defense, 1);
        currentHP -= finalDamage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log($"{gameObject.name} took {finalDamage} damage. HP: {currentHP}/{maxHP}");
        ChangeStateOnDamage();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected abstract void ChangeStateOnDamage();
    public abstract void Die();

    // 상태 변경
    [PunRPC]
    public void ChangeState(int newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        if (animator != null) animator.SetInteger("State", currentState);
    }

    // 네트워크 상태 동기화
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // 내가 소유자
        {
            // 로컬 플레이어가 데이터를 보냄
            stream.SendNext(currentHP);
            stream.SendNext(currentState);
        }
        else
        {
            // 원격 플레이어가 데이터를 받음
            currentHP = (float)stream.ReceiveNext();
            currentState = (int)stream.ReceiveNext();
            if (animator != null) animator.SetInteger("State", currentState);
        }
    }


}
