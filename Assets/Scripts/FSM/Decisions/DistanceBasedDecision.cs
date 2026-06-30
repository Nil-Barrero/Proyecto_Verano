using FSM;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/Distance/DistanceBasedDecision")]
public class DistanceBasedDecision : Decision
{
    public Comparison comparison = Comparison.Less;
    public float targetDistance = 3f;

    public override bool Decide(Controller controller)
    {
        if (controller.currentTarget == null) return false;

        float distance = Vector2.Distance(
            controller.transform.position,
            controller.currentTarget.transform.position);

        return comparison.Compare(distance, targetDistance);
    }
}
