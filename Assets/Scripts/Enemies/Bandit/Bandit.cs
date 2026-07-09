using UnityEngine;

public class Bandit : Enemy
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
    [SerializeField] public Bandit_AppearingState appearingState;
    [SerializeField] public Bandit_PrepearingShotState prepearingState;
    [SerializeField] public Bandit_ShootingState shootingState;
    [SerializeField] public Bandit_HidingState hidingState;

    void Start()
    {
        animator = GetComponent<Animator>();

        rigidbody = GetComponent<Rigidbody2D>();

        healthBehaviour = GetComponent<HealthBehaviour>();
        healthBehaviour.OnDie.AddListener(OnBanditDies);
        healthBehaviour.OnAlterHealth.AddListener(OnBanditHealthAltered);

        controller = this.GetComponent<FSMController>();
        controller.StartFSM(appearingState);
    }

    void OnBanditDies()
    {
        Destroy(this.gameObject);
    }
    void OnBanditHealthAltered(int health, int maxhealth, int prevHealth, int prevMaxhealth)
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
