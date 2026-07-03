using Unity.VisualScripting;
using UnityEngine;

public class FSMController : MonoBehaviour
{
    private IState currentState;
    
    public void StartFSM(IState newState)
    {
        currentState = newState;
        currentState.Enter(this.gameObject);
    }
    public void ChangeState(IState newState)
    {
        currentState.Exit(this.gameObject);
        currentState = newState;
        currentState.Enter(this.gameObject);
    }
    public void Update()
    {
        currentState?.Update(this.gameObject);
    }
}
