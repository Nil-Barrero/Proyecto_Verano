using FSM;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/Health/HealthBasedDecision")]
public class HealthBasedDecision : Decision
{
    public Comparison comparison = Comparison.LessEqual;
    public float targetHealth = 0f;

    public override bool Decide(Controller controller)
    {
        if (controller.TryGetComponent<HealthBehaviour>(out HealthBehaviour health))
            return comparison.Compare(health.GetHealth(), targetHealth);
        else
            return false;
    }
}
