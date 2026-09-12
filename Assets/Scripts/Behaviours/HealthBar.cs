using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
   [SerializeField] private HealthBehaviour healthBehaviour;
   [SerializeField] private Image healthFill;

    private void Start()
    {
        healthBehaviour.OnAlterHealth.AddListener(UpdateHealthBar);

        // Inicializar la barra
        UpdateHealthBar(
            (int)healthBehaviour.GetHealth(),
            (int)healthBehaviour.GetMaxHealth(),
            0,
            0
        );
    }

    private void OnDestroy()
    {
        healthBehaviour.OnAlterHealth.RemoveListener(UpdateHealthBar);
    }

    private void UpdateHealthBar(int health, int maxHealth, int previousHealth, int previousMaxHealth)
    {
        healthFill.fillAmount = (float)health / maxHealth;
    }
}
