using UnityEngine;

public class VultureBoss : Enemy
{
    [Header("Params")]
    [SerializeField] public float speed = 10;
    [SerializeField] public int damageValue = 1;
    [SerializeField] public float targetDistanceTolerance;

    [Header("Components")]
    [SerializeField] public Animator animator;
    [SerializeField] public Rigidbody2D rigidbody;
    [SerializeField] public HealthBehaviour healthBehaviour;

    [Header("States")]
    [SerializeField] public FSMController controller;
    [SerializeField] public VultureBoss_AppearingState appearingState;
    [SerializeField] public VultureBoss_LoopState loopState;

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
    }
    void OnVultureDies()
    {
        this.gameObject.SetActive(false);
    }
    void OnVultureHealthAltered(int health, int maxhealth, int prevHealth, int prevMaxhealth)
    {
        //healthBehaviour.SetInvincibility(1.5f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent<HealthBehaviour>(out HealthBehaviour hb))
            hb.Damage(damageValue);
    }
}
