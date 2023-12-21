using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int agentsQuantity = 100;

    [SerializeField]
    private GameObject agentPrefab = null;

    [SerializeField]
    [Range(2, 8)]
    private int basesQuantity = 100;

    [SerializeField]
    private GameObject basePrefab = null;

    public static List<int> Bases { get; private set; } = new List<int>();

    public static Vector2 Size { get; private set; }

    private Color[] colors = { Color.white, Color.red, Color.green, Color.blue };

    // Start is called before the first frame update
    void Start()
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        transform.localScale = new Vector2(transform.localScale.y * aspectRatio, transform.localScale.y);
        Size = transform.localScale;

        if (basePrefab)
        {
            GameObject container = GameObject.FindGameObjectWithTag("BasesContainer");

            for (int i = 0; i < basesQuantity; i++)
            {
                GameObject baseObject = Instantiate(basePrefab, RandomPositionInBounds(1, 1), Quaternion.identity);
                baseObject.name = i.ToString();
                baseObject.transform.parent = container.transform;

                BaseController baseController = baseObject.GetComponent<BaseController>();
                baseController.index = i % 4 == 0 ? 0 : i;
                baseObject.GetComponent<SpriteRenderer>().color = colors[i % 4];

                if (Bases.Contains(baseController.index)) continue;
                Bases.Add(baseController.index);
            }
        }

        if (agentPrefab)
        {
            GameObject container = GameObject.FindGameObjectWithTag("AgentsContainer");

            for (int i = 0; i < agentsQuantity; i++)
            {
                GameObject agent = Instantiate(agentPrefab, RandomPositionInBounds(7, 1), Quaternion.identity);
                agent.name = i.ToString();
                agent.transform.parent = container.transform;

                AgentController agentController = agent.GetComponent<AgentController>();
                agentController.favoriteObjective = Random.Range(1, Bases.Count);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private static Vector2 RandomPosition(float maxX, float maxY)
    {
        return new Vector2(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY));
    }

    public static Vector2 RandomPositionInBounds(float offsetX = 0, float offsetY = 0)
    {
        return RandomPosition((Size.x / 2) - offsetX, (Size.y / 2) - offsetY);
    }
}
