using UnityEngine;

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
       
    }
    public void Exit(GameObject owner)
    {
    }
}
[System.Serializable]
public class Bandit_PrepearingShotState : IState
{
    public Bandit b;
    public void Enter(GameObject owner)
    {
        b = owner.GetComponent<Bandit>();
    }
    public void Update(GameObject owner)
    {

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

    }
    public void Exit(GameObject owner)
    {
    }
}