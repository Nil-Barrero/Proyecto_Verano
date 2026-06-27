using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HealthBehaviour : MonoBehaviour
{
    [SerializeField] bool healOnSceneChanged = true;
    public bool invincibility = false;
    public int health, maxhealth;

    [Header("Events")]
    public UnityEvent<int, int> OnAlterHealth;
    public UnityEvent OnDie;

    private void Awake()
    {
        health = maxhealth;
        OnAlterHealth.Invoke(health, maxhealth);
        SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadedSceneMode) {
            if (this.healOnSceneChanged)
                this.FullHeal();
        };
    }
    public void Heal()
    {
        health = Mathf.Min(health + 1, maxhealth);
        OnAlterHealth.Invoke(health, maxhealth);
    }
    public void Heal(int amount)
    {
        health = Mathf.Min(health + amount, maxhealth);
        OnAlterHealth.Invoke(health, maxhealth);
    }
    public void FullHeal()
    {
        health = maxhealth;
        OnAlterHealth.Invoke(health, maxhealth);
    }
    public void Damage()
    {
        if (!invincibility)
        {
            health--;
            if (health <= 0)
            {
                health = 0;
                OnDie.Invoke();
            }
            OnAlterHealth.Invoke(health, maxhealth);
        }
    }
    public void Damage(int damage)
    {
        if (!invincibility)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                OnDie.Invoke();
            }
            OnAlterHealth.Invoke(health, maxhealth);
        }
    }
    public void FullDamage()
    {
        if (!invincibility)
        {
            health = 0;
            OnAlterHealth.Invoke(health, maxhealth);
            OnDie.Invoke();
        }
    }
    public void AddMaxHealth()
    {
        maxhealth++;
        OnAlterHealth.Invoke(health, maxhealth);
    }
    public void AddMaxHealth(bool recover)
    {
        maxhealth++;
        if (recover)
        {
            FullHeal();
            return;
        }
        OnAlterHealth.Invoke(health, maxhealth);
    }
    public void AddMaxHealth(int amount)
    {
        maxhealth += amount;
        OnAlterHealth.Invoke(health, maxhealth);
    }
    public void AddMaxHealth(int amount, bool recover)
    {
        maxhealth += amount;
        if (recover)
            FullHeal();
        else
            OnAlterHealth.Invoke(health, maxhealth);
    }
    public void ReduceMaxHealth()
    {
        maxhealth--;
        if (maxhealth <= 0)
            maxhealth = 1;
        if (maxhealth < health)
            health = maxhealth;
        OnAlterHealth.Invoke(health, maxhealth);
    }
    public void ReduceMaxHealth(int amount)
    {
        maxhealth -= amount;
        if (maxhealth <= 0)
            maxhealth = 1;
        if (maxhealth < health)
            health = maxhealth;
        OnAlterHealth.Invoke(health, maxhealth);
    }
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
}
