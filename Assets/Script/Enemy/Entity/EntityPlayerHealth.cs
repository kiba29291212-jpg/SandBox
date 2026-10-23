using UnityEngine;
using System;

public class EntityPlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 100f;

    private float currentHP;
    private bool isDead;

    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;
    public bool IsDead => isDead;

    public event Action<float, float> OnHealthChanged;
    public event Action OnPlayerDied;

    private void Awake()
    {
        currentHP = maxHP;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void TakeDamage(float amount)
    {
        if (isDead || amount <= 0f)
            return;

        currentHP = Mathf.Max(0f, currentHP - amount);
        OnHealthChanged?.Invoke(currentHP, maxHP);

        if (currentHP <= 0f)
            Die();
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;

        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = false;

        CharacterController characterController =
            GetComponent<CharacterController>();

        if (characterController != null)
            characterController.enabled = false;

        PlayerCamera playerCamera = GetComponentInChildren<PlayerCamera>();
        if (playerCamera != null)
            playerCamera.enabled = false;

        OnPlayerDied?.Invoke();
        Debug.Log("Player Died");
    }

    public void RestoreHealth(float amount)
    {
        if (isDead || amount <= 0f)
            return;

        currentHP = Mathf.Min(maxHP, currentHP + amount);
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }
}
