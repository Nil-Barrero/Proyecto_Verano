using UnityEngine;



public class VultureStates : MonoBehaviour { }

[System.Serializable]
public class Vulture_LoopState : IState
{
    public float amplitude = 2f;
    public float loopSpeed = 1f;
    public float t = 0;
    public void Enter(GameObject owner)
    {
        t = 0;
    }
    public void Update(GameObject owner)
    {
        float sin = Mathf.Sin(t * 2);
        float cos = Mathf.Cos(t * loopSpeed) * amplitude;
        owner.transform.position = new Vector2(cos, sin);
        t += Time.deltaTime;
    }
    public void Exit(GameObject owner)
    {

    }
}
