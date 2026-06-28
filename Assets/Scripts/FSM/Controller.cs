using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM
{
    public class Controller : MonoBehaviour
    {
        public State currentState;
        public float stateTime;

        public void Update()
        {
            if (currentState == null) return;
            stateTime += Time.deltaTime;
            currentState.UpdateState(this);
        }
        public void Transition(State nextState)
        {
            currentState.DoOnExitActions(this);
            currentState = nextState;
            stateTime = 0f; 
            currentState.DoOnEnterActions(this);
        }
    }
}
