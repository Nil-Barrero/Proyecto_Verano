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
    [SerializeField] public SpriteRenderer spriteRenderer;

    [Header("Objects")]
    [SerializeField] public GameObject blowZone;
    [SerializeField] public StripesController stripesController;

    [Header("States")]
    [SerializeField] public FSMController controller;
    
    [SerializeField] public VultureBoss_AppearingState appearingState;
    [SerializeField] public VultureBoss_LoopState loopState;
    [SerializeField] public VultureBoss_PrepearingBlowState prepearingBlowState;
    [SerializeField] public VultureBoss_BlowState blowState;
    [SerializeField] public VultureBoss_PrepearingTackleState prepearingTackleState;
    [SerializeField] public VultureBoss_TackleState tackleState;
    [SerializeField] public VultureBoss_PrepearingTackleShotState prepearingTackleShotState; 
    [SerializeField] public VultureBoss_TackleShotState tackleShotState;
    [SerializeField] public VultureBoss_ReturningToLoopState returningToLoopState;


    [Header("States/Transitions")]
    public float special1Transition = 50f;
    public float weightShift = 20f;     
    public int enrageThreshold = 20;
    Vector3 lastPos;

    public bool IsEnraged() { return enrageThreshold >= healthBehaviour.GetHealth(); }

    public IState GetNextState()
    {
        if (IsEnraged())
            return prepearingTackleShotState;

        if (Random.value * 100f < special1Transition)
        {
            special1Transition = Mathf.Clamp(special1Transition - weightShift, 0f, 100f);
            return prepearingBlowState;                          
        }
        else
        {
            special1Transition = Mathf.Clamp(special1Transition + weightShift, 0f, 100f);
            return prepearingTackleState;                
        }
    }

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
        lastPos = transform.position;
    }
    private void Update()
    {
        if (controller.GetCurrentStateType() == typeof(VultureBoss_BlowState))
            spriteRenderer.flipX = false;
        else
        {
            float deltaX = transform.position.x - lastPos.x;
            if (Mathf.Abs(deltaX) > 0.001f)
                spriteRenderer.flipX = deltaX > 0;
        }
        lastPos = transform.position;
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
    private void OnDisable() { DisableBlowZone(); }
    public void EnableBlowZone() { blowZone.SetActive(true); }
    public void DisableBlowZone() { blowZone.SetActive(false); }
}
