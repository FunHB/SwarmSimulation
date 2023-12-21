using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveController : MonoBehaviour
{
    [SerializeField]
    private float InitialSpeed = 5f;

    [SerializeField]
    private float randomRotationAngle = 0f;

    [SerializeField]
    private bool showVector = false;

    [SerializeField]
    private GameObject vectorPrefab;

    private GameObject velocityVector;

    private float speed = 0;

    public Vector2 Direction { get; set; } = Vector2.zero;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Direction = RandomDirection();
        RandomizeSpeed();

        if (showVector && vectorPrefab)
        {
            velocityVector = Instantiate(vectorPrefab, transform.position, GetVelocityRotation());
        }
    }

    // Update is called once per frame
    void Update()
    {
        TakeAStep();

        if (showVector && vectorPrefab)
        {
            ChangeVector();
        }
    }

    private void TakeAStep()
    {
        RotateByRandomAngle();
        rb.velocity = speed * Direction;

        //rb.MovePosition(transform.position + new Vector3(direction.x, direction.y, 0f) * Random.Range(0.07f, 0.12f));
    }

    private void ChangeVector()
    {
        float scale = rb.velocity.magnitude / speed;

        velocityVector.transform.SetPositionAndRotation(transform.position, GetVelocityRotation());
        velocityVector.transform.localScale = new Vector2(scale, scale);
    }

    public void Reflect(Vector2 contact, Vector2 center)
    {
        Direction = (center - contact).normalized;
    }

    public Quaternion GetVelocityRotation() => Rotation(Vector2.SignedAngle(Vector2.up, Direction));
    public void RandomizeSpeed() => speed = Random.Range(0.7f, 1.2f) * InitialSpeed;
    public void ReverseDirection() => Direction = -Direction;

    public Vector2 RandomDirection()
    {
        float random = Random.Range(0f, 2 * Mathf.PI);
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }

    private void RotateByRandomAngle() => Direction = Rotation(Random.Range(-randomRotationAngle, randomRotationAngle)) * Direction;

    private Quaternion Rotation(float angle) => Quaternion.AngleAxis(angle, Vector3.forward);
}
