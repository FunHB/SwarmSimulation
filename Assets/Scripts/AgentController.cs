using UnityEngine;

public class AgentController : MonoBehaviour
{
    [SerializeField]
    private int propagationRadius = 50;

    public int favoriteObjective = 1;
    private int objective = 0;

    private SpriteRenderer sprite;
    private MoveController moveController;

    public int[] counters = new int[0];

    public (int index, int counter) shoutValue = (0, 999);

    //private bool bumpedIntoItem = false;
    //private BaseController item = null;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        moveController = GetComponent<MoveController>();

        counters = new int[GameController.Bases.Count];

        foreach (int index in GameController.Bases)
        {
            counters[index] = 800;
        }

        propagationRadius /= 50;

        //InvokeRepeating(nameof(ManualUpdate), 1f, 0.1f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        IncreaseCounters();
        Bump();
        Shout();
        ListenToAgents();
    }

    private void IncreaseCounters()
    {
        for (int i = 0; i < counters.Length; i++)
        {
            counters[i] += 1;
        }
    }

    private void Shout()
    {
        int index = shoutValue.index < counters.Length - 1 ? shoutValue.index + 1 : 0;

        shoutValue = (index, counters[index] + propagationRadius);
    }

    private void ListenToAgents()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, propagationRadius);

        if (colliders.Length == 0) return;

        foreach (Collider2D collider in colliders)
        {
            AgentController agent = collider.GetComponent<AgentController>();

            if (!agent) continue;

            if (counters[agent.shoutValue.index] > agent.shoutValue.counter)
            {
                counters[agent.shoutValue.index] = agent.shoutValue.counter;

                if (agent.shoutValue.index == objective)
                {
                    //Debug.Log("changed direction: " + (agent.transform.position - transform.position).normalized.ToString());

                    moveController.Direction = (agent.transform.position - transform.position).normalized;
                }
            }
        }
    }

    public void Bump()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2);

        if (colliders.Length == 0) return;

        foreach (Collider2D collider in colliders)
        {
            BaseController item = collider.GetComponent<BaseController>();

            if (!item || counters.Length == 0) continue;

            counters[item.index] = 0;

            if (item.index == objective)
            {
                moveController.ReverseDirection();

                if (objective == favoriteObjective)
                {
                    sprite.color = item.Resource;
                    item.Capacity -= 0.01f;
                    objective = 0;
                    return;
                }

                sprite.color = Color.white;
                objective = favoriteObjective;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveController.ReverseDirection();
    }
}
