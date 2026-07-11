using Character;
using UnityEngine;
[System.Serializable]
public class VultureBoss_AppearingState : IState
{
    public VultureBoss v;
    public void Enter(GameObject owner)
    {
        v = owner.GetComponent<VultureBoss>();
    }
    public void Update(GameObject owner)
    {
        Vector2 targetPos = Controller.instance.transform.position + new Vector3(v.loopState.seekOffset.x, v.loopState.seekOffset.y);
        Vector2 dir = targetPos - new Vector2(v.transform.position.x, v.transform.position.y);
        v.rigidbody.linearVelocity = dir.normalized * v.speed;
        if (Vector2.Distance(v.transform.position, targetPos) < v.targetDistanceTolerance)
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
    public float loopDuration = 5f;
    public float minTimeToShoot, maxTimeToShoot;
    float shotTimer;

    VultureBoss v;
    float phase;
    float timer;
    public void Enter(GameObject owner)
    {
        Debug.Log(owner + " entered LoopState");
        v = owner.GetComponent<VultureBoss>();
        phase = Mathf.PI / 2f;
        timer = 0f;
        shotTimer = Random.Range(minTimeToShoot, maxTimeToShoot);
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

        shotTimer-= Time.deltaTime;
        if (shotTimer <= 0)
        {
            Vector2 dir = ((Vector2)Controller.instance.transform.position - (Vector2)owner.transform.position).normalized;
            GameObject bullet = PoolingManager.instance.GetInstanceOfClass("Bullet");
            bullet.transform.position = (Vector2)owner.transform.position + dir * 1f;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);
            bullet.SetActive(true);
        }
        timer += Time.deltaTime;
        //if (timer > loopDuration)
        //    v.controller.ChangeState(v.goingToAttackPosState);

    }
    public void Exit(GameObject owner)
    {
        Debug.Log(owner + " abandoned LoopState");
    }
}


