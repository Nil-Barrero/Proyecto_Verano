using Character;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


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
        Vector2 targetPos = Controller.instance.transform.position + new Vector3(v.loopState.seekOffset.x, v.loopState.seekOffset.y);
        Vector2 dir = targetPos - new Vector2(v.transform.position.x, v.transform.position.y) ;
        v.rigidbody.linearVelocity = dir.normalized * v.speed;
        if (Vector2.Distance(v.transform.position, targetPos) < v.targetDistanceTolerance)
            v.controller.ChangeState(v.loopState);

        v.spriteRenderer.flipX = (owner.transform.position.x < Controller.instance.transform.position.x);

    }
    public void Exit(GameObject owner)
    {
        v.rigidbody.linearVelocity = Vector2.zero;
    }
}

[System.Serializable]
public class Vulture_LoopState : IState
{
    [Header("Params")]
    public Vector2 seekOffset = new Vector2(0,4);
    public float amplitude = 4f;
    public float height = 1.5f;
    public float loopDuration = 5f;
    
    Vulture v;
    float phase;   
    float timer;
    public void Enter(GameObject owner)
    {
        Debug.Log(owner + " entered LoopState");
        v = owner.GetComponent<Vulture>();
        phase = Mathf.PI / 2f;
        timer = 0f;
    }
    public void Update(GameObject owner)
    {
        Vector2 center = Controller.instance.transform.position + new Vector3(seekOffset.x, seekOffset.y, 0);
        Vector2 offset = new Vector2(
            Mathf.Cos(phase) * amplitude,
            Mathf.Sin(phase * 2f) * height);
        owner.transform.position = center + offset;
        Vector2 tangent = new Vector2(
            -Mathf.Sin(phase) * amplitude,
             2f * Mathf.Cos(phase * 2f) * height);
        phase += Time.deltaTime * v.speed / tangent.magnitude;

        timer += Time.deltaTime;
        if (timer > loopDuration)
            v.controller.ChangeState(v.goingToAttackPosState);

        v.spriteRenderer.flipX = (owner.transform.position.x < Controller.instance.transform.position.x);
    }
    public void Exit(GameObject owner)
    {
        Debug.Log(owner + " abandoned LoopState");
    }
}

[System.Serializable]
public class Vulture_GoingToAttackPosState : IState
{
    public float speedMult = 2.5f;
    public Vulture v;
    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<Vulture>();
    }
    public void Update(GameObject owner)
    {
        v.rigidbody.linearVelocity = Vector2.down * v.speed * speedMult;
        if (owner.transform.position.y < Controller.instance.transform.position.y)
            v.controller.ChangeState(v.waitingToAttackState);

        v.spriteRenderer.flipX = (owner.transform.position.x < Controller.instance.transform.position.x);
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
    [SerializeField] Vector3 constantDistanceToPlayer;
    public void Enter(GameObject owner) {
        timer = waitingToAttackTime;
        constantDistanceToPlayer = Controller.instance.transform.position - owner.transform.position;
    }
    public void Update(GameObject owner){
        owner.transform.position = Controller.instance.transform.position + constantDistanceToPlayer;
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Vulture vulture = owner.GetComponent<Vulture>();
            vulture.controller.ChangeState(vulture.attackState);
        }   

       
    }
    public  void Exit(GameObject owner){
        
    }
}

[System.Serializable]
public class Vulture_AttackState : IState
{
    [Header("Params")]
    public float attackSpeed = 20f; 

    Vulture v;
    Vector2 start;
    Vector2 end;
    Vector2 dir;

    public void Enter(GameObject owner)
    {
        if (!v)
            v = owner.GetComponent<Vulture>();

        start = owner.transform.position;
        Vector2 player = Controller.instance.transform.position;
        end = player + (player - start);  
        dir = (end - start).normalized;
        v.spriteRenderer.flipX = dir.x < 0;
    }

    public void Update(GameObject owner)
    {
        v.rigidbody.linearVelocity = dir * attackSpeed;
        if (Vector2.Dot(end - (Vector2)owner.transform.position, dir) <= 0f)
            v.controller.ChangeState(v.returningToLoopPosState);

    }

    public void Exit(GameObject owner)
    {
        v.rigidbody.linearVelocity = Vector2.zero;
    }
}

[System.Serializable]
public class Vulture_ReturningToLoopPosState : IState
{
    public Vulture v;
    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<Vulture>();
    }
    public void Update(GameObject owner)
    {
        Vector2 targetPos = Controller.instance.transform.position + new Vector3(v.loopState.seekOffset.x, v.loopState.seekOffset.y);
        Vector2 dir = targetPos - new Vector2(v.transform.position.x, v.transform.position.y) ;
        v.rigidbody.linearVelocity = dir.normalized * v.speed;
        if (Vector2.Distance(v.transform.position, targetPos) < v.targetDistanceTolerance)
            v.controller.ChangeState(v.loopState);

        //v.spriteRenderer.flipX = (targetPos.x < dir.x); Por si se quiere que mire hacia donde se mueve
        v.spriteRenderer.flipX = (owner.transform.position.x < Controller.instance.transform.position.x); //siga mirando al jugador
    }
    public void Exit(GameObject owner)
    {
        v.rigidbody.linearVelocity = Vector2.zero;
    }
}