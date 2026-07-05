using UnityEngine;

public interface IState
{
    void Enter(GameObject owner);
    void Update(GameObject owner);
    void Exit(GameObject owner);
}
