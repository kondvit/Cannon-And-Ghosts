using System.Collections;
using UnityEngine;

public class VerletPoint
{
    public Vector3 current;
    public Vector3 old;

    public VerletPoint(Vector3 current, Vector3 old)
    {
        this.current = current;
        this.old = old;
    }
}

struct Stick
{
    public VerletPoint p0;
    public VerletPoint p1;
    public float distance;

    public Stick(VerletPoint p0, VerletPoint p1)
    {
        this.p0 = p0;
        this.p1 = p1;
        distance = (p0.current - p1.current).magnitude;
    }
}


public class GhostController : MonoBehaviour
{

    private LineRenderer line;
    public VerletPoint[] points;
    private Stick[] sticks;
    private int numberOfExtraSticks = 16;
    

    //private Vector3 verticalImpulse = new Vector3(0, 0.01f);
    [SerializeField]
    private Vector3 impulse = new Vector3(0.01f, 0.01f);
    [SerializeField]
    private Vector3 gravity = new Vector3(0, -1);
    [SerializeField]
    private GameObject prefab;

    float stoneTopBound;
    float stoneLeftBound;
    float stoneRightBound;

    void Start()
    {
        InitializeGhost();

        stoneTopBound = transform.InverseTransformPoint(new Vector3(0, -0.121f)).y;
        stoneLeftBound = transform.InverseTransformPoint(new Vector3(-2.8f, 0)).x;
        stoneRightBound = transform.InverseTransformPoint(new Vector3(1.8f, 0)).x;
    }

    void Update()
    {
        UpdatePoints();
        for(int i = 0; i < 1; i++)
        {
            UpdateSticks(); //place in a 4 loop
        }
        RenderPoints();

        DespawnGhost();

    }

    private void DespawnGhost()
    {
        Vector3 position = transform.TransformPoint(points[0].current);
        if (position.x < -16.0f || position.x > 16.0f || position.y > 9.0f)
        {
            float randomX = Random.Range(-7, -4);
            float randomY = Random.Range(-1, 1);

            Instantiate(prefab, new Vector3(randomX, randomY), Quaternion.identity);
            gameObject.tag = "Untagged";
            Destroy(gameObject);
        }
    }

    private void InitializeGhost()
    {
        line = GetComponent<LineRenderer>();

        Vector3[] linePoints = new Vector3[line.positionCount];
        line.GetPositions(linePoints);

        int i; //counter

        points = new VerletPoint[linePoints.Length + 2]; //for the eyes
        for (i = 0; i < linePoints.Length; i++)
        {
            points[i] = new VerletPoint(linePoints[i] , linePoints[i]);
            points[i].old -= impulse;
        }

        Vector3 rightEye = transform.TransformPoint(new Vector3(0.4f, -0.63f));
        Vector3 leftEye = transform.TransformPoint(new Vector3(0.09f, -0.61f));
        points[i] = new VerletPoint(rightEye, rightEye);
        points[i].old -= impulse;
        points[i+1] = new VerletPoint(leftEye, leftEye);
        points[i+1].old -= impulse;

        sticks = new Stick[points.Length - 1 + numberOfExtraSticks]; //3 additions // 1 less stick than points

        for (i = 1; i < linePoints.Length; i++)
        {
            sticks[i - 1] = new Stick(points[i - 1], points[i]);
        }

        //Invisible sticks
        Debug.Log(points[i].current);
        Debug.Log(rightEye);
        sticks[i - 1 ] = new Stick(points[i], points[1]); //right eye
        sticks[i + 9] = new Stick(points[i], points[2]); //right eye
        sticks[i + 11] = new Stick(points[i], points[8]); //right eye
        sticks[i + 13] = new Stick(points[i], points[9]); //right eye
        sticks[i + 15] = new Stick(points[i], points[0]); //right eye
        sticks[i     ] = new Stick(points[i + 1], points[1]);// left eye
        sticks[i + 10] = new Stick(points[i + 1], points[2]);
        sticks[i + 12] = new Stick(points[i + 1], points[8]);
        sticks[i + 14] = new Stick(points[i + 1], points[9]);
        sticks[i + 16] = new Stick(points[i + 1], points[10]);

        sticks[i + 1] = new Stick(points[0], points[9]);
        sticks[i + 2] = new Stick(points[1], points[10]);
        sticks[i + 3] = new Stick(points[1], points[9]);

        //sticks[i + 2] = new Stick(points[1], points[8]);
        sticks[i + 4] = new Stick(points[2], points[4]);
        sticks[i + 5] = new Stick(points[4], points[6]);
        sticks[i + 6] = new Stick(points[6], points[8]);

        //skirt
        sticks[i + 7] = new Stick(points[1], points[8]);
        sticks[i + 8] = new Stick(points[2], points[9]);

    }

    private void RenderPoints()
    {
        Vector3[] linePoints = new Vector3[points.Length - 2];
        int i;
        for (i = 0; i < points.Length - 2; i++) //-2 for the eyes 
        {
            linePoints[i] = points[i].current;
        }

        transform.GetChild(0).position = points[i].current;
        transform.GetChild(1).position = points[i+1].current;

        line.SetPositions(linePoints);
    }

    private void UpdatePoints()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 velocity = points[i].current - points[i].old;

            points[i].old = points[i].current;

            points[i].current += velocity;
            points[i].current += gravity * Time.deltaTime * Time.deltaTime;

            //float worldYcoord = transform.TransformPoint(points[i].current).y;
            ////Debug.Log(worldYcoord);
            //if (worldYcoord > 0 && !movingRight)
            //{
            //    points[i].current += horizontalImpulse;
            //    movingRight = true;
            //}


            //TODO:remove grav
            //////////////////////////////////////
            //Just grav
            float bound = transform.InverseTransformPoint(new Vector3(0, -4.0f)).y;
            if (points[i].current.y < bound)
            {
                points[i].current.y = bound;
                points[i].old.y = points[i].current.y + velocity.y;
            }



            if (points[i].current.y < stoneTopBound && points[i].current.x > stoneLeftBound && points[i].current.x < stoneRightBound)
            {
                if (points[i].current.y > stoneTopBound - 0.1f)
                {
                    points[i].current.y = stoneTopBound;
                }
                else if (points[i].current.x < stoneLeftBound + 0.1f)
                {
                    points[i].current.x = stoneLeftBound;
                }
                else if (points[i].current.x > stoneRightBound - 0.1f)
                {
                    points[i].current.x = stoneRightBound;
                }
            }

        }
    }

    private void UpdateSticks()
    {
        for (int i = 0; i < sticks.Length; i++)
        {
            Vector3 delta = sticks[i].p1.current - sticks[i].p0.current;
            float falseLength = delta.magnitude;
            float difference = sticks[i].distance - falseLength;
            float percent = difference / falseLength / 2;

            Vector3 offset = percent * delta;
            sticks[i].p0.current -= offset;
            sticks[i].p1.current += offset;
        }
    }
}
