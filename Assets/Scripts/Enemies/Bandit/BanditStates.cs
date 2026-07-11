using Character;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Bandit_AppearingState : IState
{
    public Bandit b;
    public void Enter(GameObject owner)
    {
        b = owner.GetComponent<Bandit>();
    }
    public void Update(GameObject owner)
    {
        
        b.rigidbody.linearVelocityX = Mathf.Sign(Controller.instance.transform.position.x - owner.transform.position.x) * b.speed;
        if (Mathf.Abs(Controller.instance.transform.position.x - owner.transform.position.x) < b.shootDistance)
            b.controller.ChangeState(b.prepearingState);
    }
    public void Exit(GameObject owner)
    {
    }
}
[System.Serializable]
public class Bandit_PrepearingShotState : IState
{
    public Bandit b;
    public float timeToShoot = 5;
    private float timer = 0;
    public void Enter(GameObject owner)
    {
        b = owner.GetComponent<Bandit>();
        timer = timeToShoot;
        b.rigidbody.linearVelocityX = 0;
    }
    public void Update(GameObject owner)
    {
        timer-= Time.deltaTime;
        if(timer <= 0 )
            b.controller.ChangeState(b.shootingState);

    }
    public void Exit(GameObject owner)
    {
    }
}
[System.Serializable]
public class Bandit_ShootingState : IState
{
    public Bandit b;
    public void Enter(GameObject owner)
    {
        b = owner.GetComponent<Bandit>();
        Vector2 dir = ((Vector2)Controller.instance.transform.position - (Vector2)owner.transform.position).normalized;
        GameObject bullet = PoolingManager.instance.GetInstanceOfClass("Bullet");
        bullet.transform.position = (Vector2)owner.transform.position + dir * 1f;
        bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);
        bullet.SetActive(true);
        b.lastBullet = bullet;
        b.controller.ChangeState(b.hidingState);
    }
    public void Update(GameObject owner)
    {

    }
    public void Exit(GameObject owner)
    {
    }
}
[System.Serializable]
public class Bandit_HidingState : IState
{
    public Bandit b;
    public void Enter(GameObject owner)
    {
        b = owner.GetComponent<Bandit>();
    }
    public void Update(GameObject owner)
    {
        if (b.lastBullet == null || !b.lastBullet.activeInHierarchy)
            b.controller.ChangeState(b.prepearingState);
    }
    public void Exit(GameObject owner)
    {
    }
}

[System.Serializable]
public class Bandit_PassThroughState : IState
{
    public Bandit b;
    public float timeBetweenShoots = 2;
    float timer = 0;
    bool inverseDirection;
    public void Enter(GameObject owner)
    {
        b = owner.GetComponent<Bandit>();
        timer = timeBetweenShoots;
    }
    public void Update(GameObject owner)
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            timer = timeBetweenShoots;
            Vector2 dir = ((Vector2)Controller.instance.transform.position - (Vector2)owner.transform.position).normalized;
            GameObject bullet = PoolingManager.instance.GetInstanceOfClass("Bullet");
            bullet.transform.position = (Vector2)owner.transform.position + dir * 1f;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);
            bullet.SetActive(true);
        }

        if (!inverseDirection)
            b.rigidbody.linearVelocityX = 1;
        else
            b.rigidbody.linearVelocityX = -1;
    }
    public void Exit(GameObject owner)
    {
    }
}
