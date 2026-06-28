using FSM;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/Vulture/LoopMovement")]
public class Vulture_loop : FSM.Action
{
    public override void Act(Controller controller)
    {
        float t = controller.stateTime;
        float sin = Mathf.Sin(t * 2);
        float cos = Mathf.Cos(t * 1) * 2;
        controller.transform.position = new Vector2(cos, sin);
    }
}
