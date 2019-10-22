using UnityEngine;

public class CannonBallController : MonoBehaviour
{
    public Vector3 ballVelocity;

    private Vector3 gravity = new Vector3(0,-15);

    private float cameraBoundY = -4.0f;

    private float timeToDespawn = 2.0f; // in seconds
    private float despawnFactor = 0.1f;
    private float despawnTimer = 0;

    public Vector3[] convexHall { get; private set; }
    private int convexHallResolution = 8;
    private float radius = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        convexHall = new Vector3[convexHallResolution];
        float sector = 2 * Mathf.PI / convexHallResolution;
        for(int i = 0; i < convexHall.Length; i++)
        {
            convexHall[i] = radius * new Vector3(Mathf.Cos(i * sector), Mathf.Sin(i * sector));
        }
        //GetComponent<LineRenderer>().positionCount = convexHallResolution;
        //GetComponent<LineRenderer>().SetPositions(convexHall);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBall();
        DespawnBall();

        //debug
        //GetComponent<LineRenderer>().SetPositions(convexHall);
    }

    //might have bugs
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

        Vector3 translation = ballVelocity * Time.deltaTime;
        transform.Translate(translation);

        //translate convexHall
        Matrix4x4 m = Matrix4x4.Translate(translation);
        for (int i = 0; i < convexHall.Length; i++)
        {
            convexHall[i] = m.MultiplyPoint3x4(convexHall[i]);
        }

    }


    /**************************
     * TODO:
     * We slap a collider on the objects that collide
     * That collider will have information about the object
     * we go through all the world object and if they have collider we resorve the collision
     * 
     * balls should have circle collider
     * balls should have dynamic resolution collide with eachother
     * TODO: restitution coefficient on the ball
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */ 
    

}
