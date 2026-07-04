using Character;
using UnityEngine;


[System.Serializable]
public class Vulture_AppearingState : IState
{
    public Vulture v;
    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<Vulture>();
    }
    public void Update(GameObject owner)
    { 
        if (v)
        {
            Vector2 dir = new Vector2(v.transform.position.x, v.transform.position.y) - Vector2.zero;
            v.rigidbody.linearVelocity = dir.normalized * v.speed * Time.deltaTime;
            if (Vector2.Distance(v.transform.position, Vector2.zero) < v.targetDistanceTolerance)
                v.controller.ChangeState(v.loopState);
        }
    }
    public void Exit(GameObject owner)
    {
        v.rigidbody.linearVelocity = Vector2.zero;
    }
}

[System.Serializable]
public class Vulture_LoopState : IState
{
    public float amplitude = 2f;
    public float loopSpeed = 1f;
    public float t = 0;
    public void Enter(GameObject owner)
    {
        Debug.Log(owner + " entered LoopState");
        t = 0;
    }
    public void Update(GameObject owner)
    {
        float sin = Mathf.Sin(t * 2);
        float cos = Mathf.Cos(t * loopSpeed) * amplitude;
        owner.transform.position = new Vector2(cos, sin);
        t += Time.deltaTime;
        if (t > 10)
            owner.GetComponent<FSMController>().ChangeState(owner.GetComponent<Vulture>().goingToAttackPosState);
    }
    public void Exit(GameObject owner)
    {
        Debug.Log(owner + " abandoned LoopState");
    }
}

[System.Serializable]
public class Vulture_GoingToAttackPosState : IState
{
    public Vulture v;
    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<Vulture>();
    }
    public void Update(GameObject owner)
    {
        v.rigidbody.linearVelocity = Vector2.down * v.speed * Time.deltaTime;
        if (owner.transform.position.y < Controller.instance.transform.position.y)
            v.controller.ChangeState(v.attackState);
    }
    public void Exit(GameObject owner)
    {
        v.rigidbody.linearVelocity = Vector2.zero;
    }
}

[System.Serializable]
public class Vulture_WaitingToAttackState : IState
{
    public float waitingToAttackTime = 3;
    [SerializeField]float timer = 0;
    public void Enter(GameObject owner) {
        timer = waitingToAttackTime;
    }
    public void Update(GameObject owner){
        waitingToAttackTime -= Time.deltaTime;
        if(waitingToAttackTime < 0)
        {
            Vulture vulture = owner.GetComponent<Vulture>();
            vulture.controller.ChangeState(vulture.loopState);
        }   
    }
    public  void Exit(GameObject owner){
        
    }
}

[System.Serializable]
public class Vulture_AttackState : IState
{
    public Vulture v;
    public void Enter(GameObject owner)
    {
        if(!v)
            v = owner.GetComponent<Vulture>();
    }
    public void Update(GameObject owner)
    {
       
    }
    public void Exit(GameObject owner)
    {
    }
}

[System.Serializable]
public class Vulture_ReturningToLoopPosState : IState
{
    public void Enter(GameObject owner)
    {
    }
    public void Update(GameObject owner)
    {

    }
    public void Exit(GameObject owner)
    {

    }
}