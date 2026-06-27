using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(Controller controller);
        public virtual void Act(Controller controller, GameObject target) { }
    }
}
