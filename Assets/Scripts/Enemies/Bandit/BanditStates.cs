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
        if (owner.transform.position.x > 0)
            b.rigidbody.linearVelocityX = 1;
        else
            b.rigidbody.linearVelocityX = -1;

        if (Vector3.Distance(owner.transform.position, Controller.instance.transform.position) < 1)
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
        GameObject bullet = PoolingManager.instance.GetInstanceOfClass("Bullet");
        bullet.transform.position = owner.transform.position;
        bullet.transform.LookAt(Controller.instance.transform.position);
        bullet.SetActive(true);
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
        if (owner.transform.position.x < 0)
            b.rigidbody.linearVelocityX = 1;
        else
            b.rigidbody.linearVelocityX = -1;
    }
    public void Exit(GameObject owner)
    {
    }
}