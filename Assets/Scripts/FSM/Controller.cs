using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM
{
    public class Controller : MonoBehaviour
    {
        [Header("FSM data")]
        public State currentState;
        public GameObject currentTarget; 
        public float stateTime;

        [System.NonSerialized] public float stateTimeLimit = -1f;

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
            stateTimeLimit = -1f;
            currentState.DoOnEnterActions(this);
        }
    }
}
