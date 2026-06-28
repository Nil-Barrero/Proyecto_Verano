using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName ="FSM/State")]
    public class State : ScriptableObject
    {
        public Action[] actions;
        public Action[] onEnterActions;
        public Action[] onExitActions;
        public Transition[] transitions; 

        
         public void UpdateState(Controller controller) 
        {
            DoActions(controller);
            CheckTransitions(controller); 
        }
        public void DoOnEnterActions(Controller controller)
        {
            for (int i = 0; i < onEnterActions.Length; i++)
            {
                onEnterActions[i].Act(controller);
            }
        }
        public void DoOnExitActions(Controller controller)
        {
            for (int i = 0; i < onExitActions.Length; i++)
            {
                onExitActions[i].Act(controller);
            }
        }
        private void DoActions(Controller controller)
        {
            for(int i =0; i<actions.Length; i++)
            {
                actions[i].Act(controller);
            }
        }

        private void CheckTransitions(Controller controller)
        {
            for ( int i=0; i < transitions.Length;i++)
            {
                bool decision = transitions[i].decision.Decide(controller);
                if(decision)
                {
                    controller.Transition(transitions[i].trueState);
                    return;
                }
            }    
        }
    }
}
