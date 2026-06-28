using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HealthBehaviour : MonoBehaviour
{
    public enum DeathAction { None, Disable, Destroy }

    [Header("Params")]
    [SerializeField] int health;
    [SerializeField] int maxhealth;
    [SerializeField] int lifes;

    [Header("Config")]
    [SerializeField] bool healOnSceneChanged = true;
    [SerializeField] bool invincibilityOnDamage = true;
    [SerializeField] float damageInvincibilityTime = 0;
    [SerializeField] bool fullDamageIgnoresInvincibility = false;

    [Header("Lifes")]
    [SerializeField] bool useLifes = true;
    [SerializeField] bool autoHealOnRespawn = true;
    [SerializeField] float respawnInvincibilityTime = 0;
    [SerializeField] DeathAction onFinalDeath = DeathAction.None;

    bool invincibility = false;
    Coroutine autoInvincibilityRoutine;

    [Header("Events")]
    public UnityEvent<int, int, int, int> OnAlterHealth;
    public UnityEvent<int, int> OnAlterLifes;
    public UnityEvent OnDie;

    private void Awake()
    {
        health = maxhealth;
        OnAlterHealth.Invoke(health, maxhealth, 0, 0);
        OnAlterLifes.Invoke(lifes, 0);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnValidate()
    {
        maxhealth = Mathf.Max(1, maxhealth);
        health = Mathf.Clamp(health, 0, maxhealth);
        lifes = Mathf.Max(0, lifes);
        damageInvincibilityTime = Mathf.Max(0, damageInvincibilityTime);
        respawnInvincibilityTime = Mathf.Max(0, respawnInvincibilityTime);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (healOnSceneChanged)
            FullHeal();
    }

    public void Heal(int amount)
    {
        int prevHealth = health;
        health = Mathf.Clamp(health + amount, 0, maxhealth);
        OnAlterHealth.Invoke(health, maxhealth, prevHealth, maxhealth);
    }
    public void Heal() => Heal(1);

    public void FullHeal()
    {
        int prevHealth = health;
        health = maxhealth;
        OnAlterHealth.Invoke(health, maxhealth, prevHealth, maxhealth);
    }

    public void Damage(int damage)
    {
        if (invincibility || health <= 0) return;

        int prevHealth = health;
        health = Mathf.Clamp(health - damage, 0, maxhealth);
        OnAlterHealth.Invoke(health, maxhealth, prevHealth, maxhealth);

        if (health <= 0)
            HandleDeath();
        else if (invincibilityOnDamage && damageInvincibilityTime > 0)
            StartAutoInvincibility(damageInvincibilityTime);
    }
    public void Damage() => Damage(1);

    public void FullDamage()
    {
        if (health <= 0) return;
        if (invincibility && !fullDamageIgnoresInvincibility) return;

        int prevHealth = health;
        health = 0;
        OnAlterHealth.Invoke(health, maxhealth, prevHealth, maxhealth);
        HandleDeath();
    }

    private void HandleDeath()
    {
        OnDie.Invoke();

        if (!useLifes)
        {
            HandleFinalDeath();
            return;
        }

        int prevLifes = lifes;
        lifes = Mathf.Max(0, lifes - 1);
        OnAlterLifes.Invoke(lifes, prevLifes);

        if (lifes > 0)
            Respawn();
        else
            HandleFinalDeath();
    }

    private void Respawn()
    {
        if (autoHealOnRespawn)
            FullHeal();

        if (respawnInvincibilityTime > 0)
            StartAutoInvincibility(respawnInvincibilityTime);
    }

    private void HandleFinalDeath()
    {
        switch (onFinalDeath)
        {
            case DeathAction.Disable:
                gameObject.SetActive(false);
                break;
            case DeathAction.Destroy:
                Destroy(gameObject);
                break;
        }
    }

    private void StartAutoInvincibility(float duration)
    {
        if (autoInvincibilityRoutine != null)
            StopCoroutine(autoInvincibilityRoutine);
        autoInvincibilityRoutine = StartCoroutine(SetInvincibility(duration));
    }

    public void AddLifes(int amount)
    {
        int prevLifes = lifes;
        lifes += amount;
        OnAlterLifes.Invoke(lifes, prevLifes);
    }
    public void AddLifes() => AddLifes(1);

    public void AddMaxHealth(int amount, bool recover)
    {
        int prevMaxHealth = maxhealth;
        maxhealth += amount;
        if (recover)
            FullHeal();
        else
            OnAlterHealth.Invoke(health, maxhealth, health, prevMaxHealth);
    }
    public void AddMaxHealth(int amount) => AddMaxHealth(amount, false);
    public void AddMaxHealth(bool recover) => AddMaxHealth(1, recover);
    public void AddMaxHealth() => AddMaxHealth(1, false);

    public void ReduceMaxHealth(int amount)
    {
        int prevMaxHealth = maxhealth;
        maxhealth = Mathf.Max(1, maxhealth - amount);

        if (maxhealth < health)
        {
            int prevHealth = health;
            health = maxhealth;
            OnAlterHealth.Invoke(health, maxhealth, prevHealth, prevMaxHealth);
        }
        else
        {
            OnAlterHealth.Invoke(health, maxhealth, health, prevMaxHealth);
        }
    }
    public void ReduceMaxHealth() => ReduceMaxHealth(1);

    public IEnumerator SetInvincibility(float duration)
    {
        invincibility = true;
        yield return new WaitForSeconds(duration);
        invincibility = false;
    }
    public IEnumerator SetInvincibility()
    {
        invincibility = true;
        yield return null;
    }
    public IEnumerator QuitInvincibility()
    {
        invincibility = false;
        yield return null;
    }

    public bool IsInvincible() { return invincibility; }
    public bool IsAlive() { return health > 0 && (!useLifes || lifes > 0); }
    public float GetHealthPercent() { return maxhealth > 0 ? (float)health / maxhealth : 0f; }
}