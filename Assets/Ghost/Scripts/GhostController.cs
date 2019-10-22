using UnityEngine;

class VerletPoint
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
    private VerletPoint[] points;
    private Stick[] sticks;
    private int numberOfExtraSticks = 6;
   

    //private Vector3 verticalImpulse = new Vector3(0, 0.01f);
    [SerializeField]
    private Vector3 impulse = new Vector3(0.01f, 0.01f);
    [SerializeField]
    private Vector3 gravity = new Vector3(0, -1);

    void Start()
    {
        InitializeGhost();
    }

    void Update()
    {
        UpdatePoints();
        for(int i = 0; i < 1; i++)
        {
            UpdateSticks(); //place in a 4 loop
        }
        RenderPoints();
    }

    private void InitializeGhost()
    {
        line = GetComponent<LineRenderer>();

        Vector3[] linePoints = new Vector3[line.positionCount];
        line.GetPositions(linePoints);

        int i; //counter

        points = new VerletPoint[linePoints.Length];
        for (i = 0; i < linePoints.Length; i++)
        {
            points[i] = new VerletPoint(linePoints[i] , linePoints[i]);
            points[i].old -= impulse;
        }

        sticks = new Stick[points.Length - 1 + numberOfExtraSticks]; //3 additions // 1 less stick than points

        for (i = 1; i < points.Length; i++)
        {
            sticks[i - 1] = new Stick(points[i - 1], points[i]);
        }

        //Invisible sticks
        sticks[i - 1] = new Stick(points[0], points[9]);
        sticks[i] = new Stick(points[1], points[10]);
        sticks[i + 1] = new Stick(points[1], points[9]);

        //sticks[i + 2] = new Stick(points[1], points[8]);
        sticks[i + 2] = new Stick(points[2], points[8]);

        //skirt
        sticks[i + 3] = new Stick(points[6], points[10]);
        sticks[i + 4] = new Stick(points[0], points[4]);

    }

    private void RenderPoints()
    {
        Vector3[] linePoints = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            linePoints[i] = points[i].current;
        }
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
