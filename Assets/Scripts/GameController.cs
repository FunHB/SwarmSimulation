using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int agentsQuantity = 100;

    [SerializeField]
    private GameObject agentPrefab = null;

    public static List<int> Bases { get; private set; }

    public static Vector2 Size { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Size = transform.localScale;

        Bases = GetDistinctBases();

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

    private static List<int> GetDistinctBases()
    {
        BaseController[] bases = FindObjectsOfType<BaseController>();

        List<int> distincBases = new();

        foreach (BaseController baseController in bases)
        {
            if (distincBases.Contains(baseController.index)) continue;
            distincBases.Add(baseController.index);
        }

        return distincBases;
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
