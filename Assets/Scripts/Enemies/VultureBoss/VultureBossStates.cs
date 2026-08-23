using System.Collections.Generic;
using Character;
using UnityEngine;
[System.Serializable]
public class VultureBoss_AppearingState : IState
{
    public VultureBoss v;
    public float distanceToPatrol = 4;
    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<VultureBoss>();
    }
    public void Update(GameObject owner)
    {
        Vector2 targetPos = Vector2.zero;//Controller.instance.transform.position + new Vector3(v.loopState.seekOffset.x, v.loopState.seekOffset.y);
        Vector2 dir = targetPos - new Vector2(v.transform.position.x, v.transform.position.y);
        v.rigidbody.linearVelocity = dir.normalized * v.speed;
        if (Vector2.Distance(v.transform.position, targetPos) < distanceToPatrol)
            v.controller.ChangeState(v.loopState);

    }
    public void Exit(GameObject owner)
    {
        v.rigidbody.linearVelocity = Vector2.zero;
    }
}

[System.Serializable]
public class VultureBoss_LoopState : IState
{
    [Header("Params")]
    public Vector2 seekOffset = new Vector2(0, 4);
    public float amplitude = 4f;
    public float height = 1.5f;
    public float loopSpeed = 1;
    public float minLoopDuration = 5f;
    public float maxLoopDuration = 8f;
    public float enrageSpeedMultiplier = 1.5f;
    public float minTimeToShoot, maxTimeToShoot;
    float shotTimer;

    VultureBoss v;
    float phase;
    float timer;
    float loopDuration;
    public void Enter(GameObject owner)
    {
        Debug.Log(owner + " entered LoopState");
        v = owner.GetComponent<VultureBoss>();
        phase = Mathf.PI / 2f;
        timer = 0f;
        loopDuration = Random.Range(minLoopDuration, maxLoopDuration);
        shotTimer = Random.Range(minTimeToShoot, maxTimeToShoot);
    }
    public void Update(GameObject owner)
    {

        Vector2 center = Vector2.zero;
        //Vector2 center = Controller.instance.transform.position + new Vector3(seekOffset.x, seekOffset.y, 0);
        Vector2 offset = new Vector2(
            Mathf.Cos(phase) * amplitude,
            Mathf.Sin(phase * 2f) * height);
        owner.transform.position = center + offset;
        Vector2 tangent = new Vector2(
            -Mathf.Sin(phase) * amplitude,
             2f * Mathf.Cos(phase * 2f) * height);
        //phase += Time.deltaTime * v.speed / tangent.magnitude;
        float currentLoopSpeed = loopSpeed;
        if (v.IsEnraged())
            currentLoopSpeed *= enrageSpeedMultiplier;
        phase += Time.deltaTime * currentLoopSpeed / tangent.magnitude;
            
        shotTimer-= Time.deltaTime;
        if (shotTimer <= 0)
        {
            shotTimer = Random.Range(minTimeToShoot, maxTimeToShoot);
            Vector2 dir = ((Vector2)Controller.instance.transform.position - (Vector2)owner.transform.position).normalized;
            GameObject bullet = PoolingManager.instance.GetInstanceOfClass("Bullet");
            bullet.transform.position = (Vector2)owner.transform.position + dir * 1f;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().spawner = owner.gameObject;
        }
        timer += Time.deltaTime;

        if (timer > loopDuration)
            v.controller.ChangeState(v.GetNextState());

    }
    public void Exit(GameObject owner)
    {
        Debug.Log(owner + " abandoned LoopState");
    }
}

[System.Serializable]
public class VultureBoss_PrepearingBlowState : IState
{
    public Vector2 targetPosition;         
    public float positionTolerance = 0.3f;
    VultureBoss v;

    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<VultureBoss>();
    }
    public void Update(GameObject owner)
    {
        Vector2 dir = targetPosition - (Vector2)owner.transform.position;
        v.rigidbody.linearVelocity = dir.normalized * v.speed;
        if (Vector2.Distance(owner.transform.position, targetPosition) < positionTolerance)
            v.controller.ChangeState(v.blowState);
    }
    public void Exit(GameObject owner)
    {
        v.rigidbody.linearVelocity = Vector2.zero;
    }
}

[System.Serializable]
public class VultureBoss_BlowState : IState
{
    VultureBoss v;
    public float minStateTime, maxStateTime;
    public float stateTimer;
    public float shotTimer;
    public float minTimeToShoot, maxTimeToShoot;
    float timer;
    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<VultureBoss>();
        v.EnableBlowZone();
        stateTimer = Random.Range(minStateTime, maxStateTime);   
        shotTimer = Random.Range(minTimeToShoot, maxTimeToShoot);
    }
    public void Update(GameObject owner)
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            v.controller.ChangeState(v.returningToLoopState);
            return;
        }

        shotTimer -= Time.deltaTime;
        if (shotTimer <= 0)
        {
            shotTimer = Random.Range(minTimeToShoot, maxTimeToShoot);
            Vector2 dir = ((Vector2)Controller.instance.transform.position - (Vector2)owner.transform.position).normalized;
            GameObject bullet = PoolingManager.instance.GetInstanceOfClass("Bullet");
            bullet.transform.position = (Vector2)owner.transform.position + dir * 1f;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().spawner = owner.gameObject;
        }
    }
    public void Exit(GameObject owner)
    {
        v.DisableBlowZone();
    }
}

[System.Serializable]
public class VultureBoss_PrepearingTackleState : IState
{
    public Vector2 targetPosition;
    public float positionTolerance = 0.3f;
    VultureBoss v;

    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<VultureBoss>();
    }
    public void Update(GameObject owner)
    {
        Vector2 dir = targetPosition - (Vector2)owner.transform.position;
        v.rigidbody.linearVelocity = dir.normalized * v.speed;
        if (Vector2.Distance(owner.transform.position, targetPosition) < positionTolerance)
            v.controller.ChangeState(v.tackleState);
    }
    public void Exit(GameObject owner)
    {
        v.rigidbody.linearVelocity = Vector2.zero;
    }
}
[System.Serializable]
public class VultureBoss_TackleState : IState
{
    public List<string> stripeNames;
    public float signalInterval = 1f;
    public float tackleSpeed = 20f;
    public float positionTolerance = 0.3f;
    VultureBoss v;
    List<Stripe> stripes;
    int index;
    float timer;
    bool signaling;

    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<VultureBoss>();
        stripes = new List<Stripe>();
        foreach (string stripeName in stripeNames)
            stripes.Add(v.stripesController.GetStripe(stripeName));
        foreach (Stripe stripe in stripes)
            stripe.renderer.color = new Color(1, 0, 0, .2f);
        index = 0;
        timer = signalInterval;
        signaling = true;
        v.animator.SetTrigger("CastAttack");
    }
    public void Update(GameObject owner)
    {
        if (signaling)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                stripes[index].renderer.enabled = true;
                index++;
                timer = signalInterval;
                if (index >= stripes.Count)
                {
                    signaling = false;
                    index = 0;
                    owner.transform.position = stripes[0].startPoint.position;
                }
            }
        }
        else
        {
            Vector2 target = stripes[index].endPoint.position;
            Vector2 dir = target - (Vector2)owner.transform.position;
            v.rigidbody.linearVelocity = dir.normalized * tackleSpeed;
            if (Vector2.Distance(owner.transform.position, target) < positionTolerance)
            {
                stripes[index].renderer.color = new Color(1,0,0,0);
                index++;
                if (index >= stripes.Count)
                {
                    v.controller.ChangeState(v.returningToLoopState);
                    return;
                }
                owner.transform.position = stripes[index].startPoint.position;
            }
        }
    }
    public void Exit(GameObject owner)
    {
        v.animator.SetTrigger("EndAttack");
        v.rigidbody.linearVelocity = Vector2.zero;
        foreach (Stripe s in stripes)
            s.renderer.enabled = false;
    }
}

[System.Serializable]
public class VultureBoss_PrepearingTackleShotState : IState
{
    public Vector2 targetPosition;
    public float positionTolerance = 0.3f;
    VultureBoss v;

    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<VultureBoss>();
    }
    public void Update(GameObject owner)
    {
        Vector2 dir = targetPosition - (Vector2)owner.transform.position;
        v.rigidbody.linearVelocity = dir.normalized * v.speed;
        if (Vector2.Distance(owner.transform.position, targetPosition) < positionTolerance)
            v.controller.ChangeState(v.tackleShotState);
    }
    public void Exit(GameObject owner)
    {
        v.rigidbody.linearVelocity = Vector2.zero;
    }
}
[System.Serializable]
public class VultureBoss_TackleShotState : IState
{
    public List<string> stripeNames;
    public float signalInterval = 1f;
    public float tackleSpeed = 20f;
    public float positionTolerance = 0.3f;
    public int shotCount = 4;
    VultureBoss v;
    List<Stripe> stripes;
    int index;
    float timer;
    bool signaling;

    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<VultureBoss>();
        v.animator.SetTrigger("CastAttack");
        stripes = new List<Stripe>();
        foreach (string stripeName in stripeNames)
            stripes.Add(v.stripesController.GetStripe(stripeName));
        foreach (Stripe stripe in stripes)
            stripe.renderer.color = new Color(0, 0, 0, .2f);
        index = 0;
        timer = signalInterval;
        signaling = true;
    }
    public void Update(GameObject owner)
    {
        if (signaling)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                stripes[index].renderer.enabled = true;
                index++;
                timer = signalInterval;
                if (index >= stripes.Count)
                {
                    signaling = false;
                    index = 0;
                    owner.transform.position = stripes[0].startPoint.position;
                }
            }
        }
        else
        {
            Vector2 target = stripes[index].endPoint.position;
            Vector2 dir = target - (Vector2)owner.transform.position;
            v.rigidbody.linearVelocity = dir.normalized * tackleSpeed;
            if (Vector2.Distance(owner.transform.position, target) < positionTolerance)
            {
                stripes[index].renderer.color = new Color(0, 0, 0, 0);
                index++;
                if (index >= stripes.Count)
                {
                    v.rigidbody.linearVelocity = Vector2.zero;
                    for (int i = 0; i < shotCount; i++)
                    {
                        float angle = 45f + i * 90f;
                        Vector2 shotDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                        GameObject bullet = PoolingManager.instance.GetInstanceOfClass("Bullet");
                        bullet.transform.position = (Vector2)owner.transform.position + shotDir * 1f;
                        bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
                        bullet.SetActive(true);
                        bullet.GetComponent<Bullet>().spawner = owner.gameObject;
                    }
                    v.controller.ChangeState(v.returningToLoopState);
                    return;
                }
                owner.transform.position = stripes[index].startPoint.position;
            }
        }
    }
    public void Exit(GameObject owner)
    {
        v.animator.SetTrigger("EndAttack");
        v.rigidbody.linearVelocity = Vector2.zero;
        foreach (Stripe s in stripes)
            s.renderer.enabled = false;
    }
}

[System.Serializable]
public class VultureBoss_ReturningToLoopState : IState
{
    public float positionTolerance = 0.3f;
    VultureBoss v;

    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<VultureBoss>();
    }
    public void Update(GameObject owner)
    {
        Vector2 dir = Vector2.zero - (Vector2)owner.transform.position;
        v.rigidbody.linearVelocity = dir.normalized * v.speed;
        if (Vector2.Distance(owner.transform.position, Vector2.zero) < positionTolerance)
            v.controller.ChangeState(v.loopState);
    }
    public void Exit(GameObject owner)
    {
        v.rigidbody.linearVelocity = Vector2.zero;
    }
}