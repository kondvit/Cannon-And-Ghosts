using UnityEngine;

public class CloudController : MonoBehaviour
{

    public static Vector3 windVelocity { get; private set; }

    private static float wrapDistance = 14.0f;
    private static float windForce = 9.0f;

    private float directionChangeTime = 2.0f;//in seconds
    private float timer = 0;


    void Start()
    {
        windVelocity = new Vector3(windForce, 0);
    }

    void Update()
    {
        UpdateWind();
        WrapCloud();
        transform.Translate(windVelocity * Time.deltaTime);

    }

    private void WrapCloud()
    {
        if (transform.position.x > wrapDistance)
        {
            transform.position = new Vector3(-wrapDistance, transform.position.y);
        }
        else if(transform.position.x < -wrapDistance)
        {
            transform.position = new Vector3(wrapDistance, transform.position.y);
        }
    }

    private void UpdateWind()
    {
        if (timer < directionChangeTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            windVelocity = new Vector3(Random.Range(-windForce, windForce), 0);
            timer = 0;
        }
    }
}
