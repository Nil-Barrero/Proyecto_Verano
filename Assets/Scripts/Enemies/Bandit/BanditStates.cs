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
    public float timeToShoot = 2;
    public float patrolSpeed = 2f;
    public float patrolFrequency = 3f;
    private float timer = 0;
    float side;
    Camera cam;
    public void Enter(GameObject owner)
    {
        b = owner.GetComponent<Bandit>();
        timer = timeToShoot;
        cam = Camera.main;
        side = Mathf.Sign(owner.transform.position.x - Controller.instance.transform.position.x);
    }
    public void Update(GameObject owner)
    {
       
        float distance = owner.transform.position.x - Controller.instance.transform.position.x;
 
        if(Mathf.Abs(distance) > b.shootDistance)
            b.rigidbody.linearVelocityX = Mathf.Sign(Controller.instance.transform.position.x - owner.transform.position.x) * b.speed;
        else
        {
            float targetX = Controller.instance.transform.position.x + side * b.shootDistance + Mathf.Sin(timer * patrolFrequency) * patrolSpeed;
            float halfWidth = cam.orthographicSize * cam.aspect;
            float camX = cam.transform.position.x;
            targetX = Mathf.Clamp(targetX, camX - halfWidth, camX + halfWidth);

            b.rigidbody.linearVelocityX = targetX - b.transform.position.x;
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
            b.controller.ChangeState(b.shootingState);
    }
    public void Exit(GameObject owner)
    {
        b.rigidbody.linearVelocityX = 0;
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
        bullet.GetComponent<Bullet>().spawner = owner.gameObject;
        bullet.GetComponent<Bullet>().SetLayer("EnemyBullet");
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
    Camera cam;
    public void Enter(GameObject owner)
    {
        b = owner.GetComponent<Bandit>();
        timer = timeBetweenShoots;
        cam = Camera.main;
        inverseDirection = owner.transform.position.x > cam.transform.position.x;
    }
    public void Update(GameObject owner)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timeBetweenShoots;
            Vector2 dir = ((Vector2)Controller.instance.transform.position - (Vector2)owner.transform.position).normalized;
            GameObject bullet = PoolingManager.instance.GetInstanceOfClass("Bullet");
            bullet.transform.position = (Vector2)owner.transform.position + dir * 1f;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().spawner = owner.gameObject;
        }

        float halfWidth = cam.orthographicSize * cam.aspect;
        float camX = cam.transform.position.x;
        /*
         * Esto es para que vaya haiendo "pingpong"
        if (owner.transform.position.x >= camX + halfWidth){
            inverseDirection = true;
            EnemyTracker.instance.AddEnemyDead();
        }
        else if (owner.transform.position.x <= camX - halfWidth){
            inverseDirection = false;
            EnemyTracker.instance.AddEnemyDead();
        }
        */
        //stas lineas de abajo por si se quiere que vaya de punto a a b y ya
        if (inverseDirection && owner.transform.position.x < camX - halfWidth)
        {
            owner.SetActive(false);
            //EnemyTracker.instance.AddEnemyDead(); esto lo marca como que te lo has cargado
        }
        else if (!inverseDirection && owner.transform.position.x > camX + halfWidth)
        {
            owner.SetActive(false);
            //EnemyTracker.instance.AddEnemyDead();
        }
        float inverted = 1f;
        if (inverseDirection)
            inverted *= -1;
        b.rigidbody.linearVelocityX =inverted * b.speed;
    }
    public void Exit(GameObject owner)
    {
        b.rigidbody.linearVelocityX = 0;
    }
}
