using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM
{
    public class Controller : MonoBehaviour
    {
        public State currentState;
        public GameObject currentTarget;
        
        
        public void Update() 
        {
            currentState.UpdateState(this); 
        }
        public void Transition(State nextState)
        {
            currentState = nextState;
        }
    }
}
     