using FSM;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/Time/TimeBasedDecision")]
public class TimeBasedDecision : Decision
{
    public float targetTime = 3f; 

    public override bool Decide(Controller controller)
    {
        return controller.stateTime > targetTime;
    }
}

[CreateAssetMenu(menuName = "FSM/Decisions/Time/RandomTimeBasedDecision")]
public class RandomTimeBasedDecision : Decision
{
    public float minTime = 3f;
    public float maxTime = 5f;

    public override bool Decide(Controller controller)
    {
        if (controller.stateTimeLimit < 0f)
            controller.stateTimeLimit = Random.Range(minTime, maxTime);

        return controller.stateTime >= controller.stateTimeLimit;
    }
}
