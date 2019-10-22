using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Cannon Controls")]
    [SerializeField]
    private float rotationSpeed = 200.0f;
    [SerializeField]
    private float maxAngle = 120.0f;
    [SerializeField]
    private float minAngle = 20.0f;

    [Header("Cannon Ball Controls")]
    [SerializeField]
    private GameObject cannonBall;
    [SerializeField]
    private float initialVelocity = 1.0f; //units per second
    [SerializeField]
    private float timeBetweenShots = 0.3f; //in seconds


    private float currentAngle = 0;
    private float shootingRateTimer;
    
  
    void Start()
    {
        shootingRateTimer = timeBetweenShots;
    }

    void Update()
    {
        RotateBarrel();
        ShootFromCannon();
    }

    private void ShootFromCannon()
    {
        if (shootingRateTimer < timeBetweenShots)
        {
            shootingRateTimer += Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Vector3 ballInitialDirection = new Vector3(0, 0.6f / transform.localScale.y, 0);
                Vector3 ballInitialVelocityVector = initialVelocity * transform.TransformDirection(ballInitialDirection).normalized;
                Vector3 cannonBallInitialPosititon = transform.TransformPoint(ballInitialDirection);

                GameObject ball = Instantiate(cannonBall, cannonBallInitialPosititon, Quaternion.identity);
                ball.GetComponent<CannonBallController>().ballVelocity = ballInitialVelocityVector;
                shootingRateTimer = 0; //start timer
            }   
        }
    }

    private void RotateBarrel()
    {
        float pressedKey = Input.GetAxisRaw("Vertical");

        float rotationAngle = rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, -pressedKey * rotationAngle);

        currentAngle += -pressedKey * rotationAngle;

        if (currentAngle < minAngle)
        {
            ClampRoationToValue(minAngle);
            currentAngle = minAngle;
        }
        else if (currentAngle > maxAngle)
        {
            ClampRoationToValue(maxAngle);
            currentAngle = maxAngle;
        }
    }

    private void ClampRoationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.z = value;
        transform.eulerAngles = eulerRotation;
    }
}
