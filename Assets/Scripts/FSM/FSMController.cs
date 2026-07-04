using Unity.VisualScripting;
using UnityEngine;

public class FSMController : MonoBehaviour
{
    [SerializeField] public IState currentState;
    [SerializeField] public string currentStateType;
    public System.Type GetCurrentStateType() { return currentState?.GetType();}
    public void StartFSM(IState newState)
    {
        currentState = newState;
        currentStateType = currentState.GetType().Name;
        currentState.Enter(this.gameObject);
    }
    public void ChangeState(IState newState)
    {
        currentState.Exit(this.gameObject);
        currentState = newState;
        currentStateType = currentState.GetType().Name;
        currentState.Enter(this.gameObject);
    }
    public void Update()
    {
        currentState?.Update(this.gameObject);
    }
}
