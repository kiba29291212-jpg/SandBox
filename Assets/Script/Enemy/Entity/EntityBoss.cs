using UnityEngine;
using System;

public class EntityBoss : MonoBehaviour
{
    [Header("Boss Stats")]
    [SerializeField] private float maxHP = 2000f;
    [SerializeField] private float damage = 30f;
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float corruption = 0f;
    [SerializeField] private float lookDamage = 5f;

    [Header("Phase Settings")]
    [SerializeField] private float phase2Threshold = 0.7f;
    [SerializeField] private float phase3Threshold = 0.4f;
    [SerializeField] private float phase4Threshold = 0.15f;
    [SerializeField] private bool startInvulnerable = true;

    private float currentHP;
    private int phase = 1;
    private bool invulnerableState;
    private bool hasGlasses;

    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;
    public int Phase => phase;
    public float Damage => damage;
    public float AttackRange => attackRange;
    public float Corruption => corruption;
    public float LookDamage => lookDamage;
    public bool IsInvulnerable => invulnerableState || !hasGlasses;
    public bool IsDead => currentHP <= 0f;

    public event Action<float, float> OnHealthChanged;
    public event Action<int> OnPhaseChanged;
    public event Action OnBossDefeated;

    private void Awake()
    {
        currentHP = maxHP;
        invulnerableState = startInvulnerable;
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void SetGlassesState(bool equipped)
    {
        hasGlasses = equipped;
    }

    public void SetInvulnerable(bool value)
    {
        invulnerableState = value;
    }

    public void ReceiveDamage(float amount, EntityPlayerHealth attacker)
    {
        if (IsDead)
            return;

        if (!hasGlasses)
        {
            if (attacker != null)
                attacker.Die();

            return;
        }

        if (invulnerableState || amount <= 0f)
            return;

        currentHP = Mathf.Max(0f, currentHP - amount);
        UpdatePhase();
        OnHealthChanged?.Invoke(currentHP, maxHP);

        if (currentHP <= 0f)
        {
            OnBossDefeated?.Invoke();
            Destroy(gameObject);
        }
    }

    private void UpdatePhase()
    {
        float hpRatio = currentHP / maxHP;
        int nextPhase = 1;

        if (hpRatio <= phase4Threshold)
            nextPhase = 4;
        else if (hpRatio <= phase3Threshold)
            nextPhase = 3;
        else if (hpRatio <= phase2Threshold)
            nextPhase = 2;

        if (nextPhase != phase)
        {
            phase = nextPhase;
            OnPhaseChanged?.Invoke(phase);
        }
    }

    public void AddCorruption(float amount)
    {
        corruption = Mathf.Clamp(corruption + amount, 0f, 100f);
    }

    public void ResetBoss()
    {
        currentHP = maxHP;
        phase = 1;
        corruption = 0f;
        invulnerableState = startInvulnerable;
        hasGlasses = false;
        OnHealthChanged?.Invoke(currentHP, maxHP);
        OnPhaseChanged?.Invoke(phase);
    }
}
