using UnityEngine;

public class Vulture : Enemy
{
    [Header("States")]
    [SerializeField]FSMController controller;
    [SerializeField] public Vulture_LoopState loopState;

    private void Start()
    {
        controller = this.GetComponent<FSMController>();
        controller.StartFSM(loopState);
    }
}
