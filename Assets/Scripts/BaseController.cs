using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public int index;

    private Vector2 initialSize;
    private MoveController moveController;

    public float Capacity { get; set; } = 1.0f;
    public Color Resource { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        initialSize = transform.localScale;
        Resource = GetComponent<SpriteRenderer>().color;
        moveController = GetComponent<MoveController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Capacity * initialSize;

        if (Capacity <= 0.1f)
        {
            transform.position = GameController.RandomPositionInBounds(1, 1);
            Capacity = 1.0f;
            moveController.RandomizeSpeed();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];

        moveController.Reflect(contact.point, transform.position);
    }
}
