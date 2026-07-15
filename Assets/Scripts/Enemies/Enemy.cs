using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        this.GetComponent<HealthBehaviour>().FullHeal();
    }
}
