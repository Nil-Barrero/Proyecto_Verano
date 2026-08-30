using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] public GameObject assignedZoneReference;
    protected virtual void OnEnable()
    {
        this.GetComponent<HealthBehaviour>().FullHeal();
        
    }
}
