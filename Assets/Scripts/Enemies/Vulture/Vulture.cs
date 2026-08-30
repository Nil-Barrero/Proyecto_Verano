using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

public class Vulture : Enemy
{
    [Header("Params")]
    [SerializeField] public float speed = 10;
    [SerializeField] public int damageValue = 1;
    [SerializeField] public float targetDistanceTolerance;

    [Header("Components")]
    [SerializeField] public Animator animator;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public Rigidbody2D rigidbody;
    [SerializeField] public HealthBehaviour healthBehaviour;

    [Header("States")]
    [SerializeField] public FSMController controller;
    [SerializeField] public Vulture_AppearingState appearingState;
    [SerializeField] public Vulture_LoopState loopState;
    [SerializeField] public Vulture_GoingToAttackPosState goingToAttackPosState;
    [SerializeField] public Vulture_WaitingToAttackState waitingToAttackState;
    [SerializeField] public Vulture_AttackState attackState;
    [SerializeField] public Vulture_ReturningToLoopPosState returningToLoopPosState;


    protected override void OnEnable()
    {
        base.OnEnable();
        if (controller != null)
        {
            rigidbody.linearVelocity = Vector2.zero;
            controller.StartFSM(appearingState);
        }
    }
    private void Start()
    {
        animator = GetComponent<Animator>();

        rigidbody = GetComponent<Rigidbody2D>();
        
        healthBehaviour = GetComponent<HealthBehaviour>();
        healthBehaviour.OnDie.AddListener(OnVultureDies);
        healthBehaviour.OnAlterHealth.AddListener(OnVultureHealthAltered);

        controller = this.GetComponent<FSMController>();
        controller.StartFSM(appearingState);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnVultureDies()
    {
        this.gameObject.SetActive(false);
     }
    void OnVultureHealthAltered(int health, int maxhealth, int prevHealth, int prevMaxhealth)
    {
        if(health == 0)
            EnemyTracker.instance.AddEnemyDead();
        healthBehaviour.SetInvincibility(1.5f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (controller.GetCurrentStateType() == typeof(Vulture_AttackState) && collision.transform.TryGetComponent<HealthBehaviour>(out HealthBehaviour hb))
            hb.Damage(damageValue);
    }
}
