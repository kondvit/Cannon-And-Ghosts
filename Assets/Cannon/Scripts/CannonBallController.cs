using UnityEngine;

public class CannonBallController : MonoBehaviour
{
    public Vector3 ballVelocity;
    public Vector3 translation; //translation made in the last frame, useful for collision resolution

    private Vector3 gravity = new Vector3(0, -15);

    private float cameraBoundY = -4.0f;

    private float timeToDespawn = 2.0f; // time for ball to despawn after loosing velocity
    private float despawnFactor = 0.1f; // velocity threshold for despawn
    private float despawnTimer = 0;

    public Vector3[] convexHall { get; private set; } //convex hall of the ball
    private int convexHallResolution = 8; //how many points represent the circle of the ball
    public float radius = 0.15f;

    void Start()
    {
        //initialize convex hall
        convexHall = new Vector3[convexHallResolution];
        float sector = 2 * Mathf.PI / convexHallResolution;
        for(int i = 0; i < convexHall.Length; i++)
        {
            convexHall[i] = radius * new Vector3(Mathf.Cos(i * sector), Mathf.Sin(i * sector));
        }
    }

    void Update()
    {
        MoveBall();
        DespawnBall();
    }

    private void DespawnBall()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        if(yPos < cameraBoundY)
        {
            Destroy(gameObject);
        }

        if(ballVelocity.sqrMagnitude < despawnFactor)
        {
            despawnTimer += Time.deltaTime;
        }
        else
        {
            despawnTimer = 0; //reset timer, the ball restarted moving
        }

        if (despawnTimer > timeToDespawn)
        {
            Destroy(gameObject);
        }
    }

    private void MoveBall()
    {
        
        if (transform.position.y > -0.25)
        {
            ballVelocity = ballVelocity + (gravity + CloudController.windVelocity) * Time.deltaTime;
        }
        else
        {
            ballVelocity = ballVelocity + gravity * Time.deltaTime;
        }

        translation = ballVelocity * Time.deltaTime;
        transform.Translate(translation);
    }
}
