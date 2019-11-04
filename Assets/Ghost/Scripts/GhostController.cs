using System.Collections.Generic;
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
    public List<VerletPoint> points = new List<VerletPoint>();
    private List<Stick> sticks = new List<Stick>();

    [SerializeField]
    private Vector3 impulse = new Vector3(0.01f, 0.01f); //impuse given at the moment of creation
    [SerializeField]
    private Vector3 gravity = new Vector3(0, -1f);
    [SerializeField]
    private GameObject prefab; //used to instantiate a new ghost

    //used for collision with stonehenge
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

        for(int i = 0; i < 1; i++) //can increase cycles to reinforce the constraints
        {
            UpdateSticks(); //place in a 4 loop
        }

        RenderPoints();

        DespawnGhost();
    }

    //despawn condition for the ghost
    private void DespawnGhost()
    {
        Vector3 position = transform.TransformPoint(points[0].current);
        if (position.x < -14.0f || position.x > 14.0f || position.y > 8.0f)
        {
            float randomX = Random.Range(-10, -4);
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

        for (int i = 0; i < linePoints.Length; i++)
        {
            points.Add(new VerletPoint(linePoints[i], linePoints[i] - impulse));
        }

        //generate the sticks between the points on the outline
        for (int i = 1; i < linePoints.Length; i++)
        {
            sticks.Add(new Stick(points[i - 1], points[i]));
        }
        
        //body sticks
        sticks.Add(new Stick(points[0], points[9]));
        sticks.Add(new Stick(points[1], points[10])); 
        sticks.Add(new Stick(points[1], points[9]));
        sticks.Add(new Stick(points[1], points[8]));
        sticks.Add(new Stick(points[2], points[9]));

        //ghost's skirt
        sticks.Add(new Stick(points[2], points[4]));
        sticks.Add(new Stick(points[4], points[6]));
        sticks.Add(new Stick(points[6], points[8]));

        AddEyes();
    }

    private void AddEyes()
    {
        Vector3 rightEye = new Vector3(0.4f, -0.63f);
        Vector3 leftEye = new Vector3(0.09f, -0.61f);
        points.Add(new VerletPoint(rightEye, rightEye - impulse));
        points.Add(new VerletPoint(leftEye, leftEye - impulse));

        //fix left eye
        sticks.Add(new Stick(points[points.Count - 1], points[1])); //left eye
        sticks.Add(new Stick(points[points.Count - 1], points[2])); //left eye
        sticks.Add(new Stick(points[points.Count - 1], points[8])); //left eye
        sticks.Add(new Stick(points[points.Count - 1], points[9])); //left eye

        //fix right eye
        sticks.Add(new Stick(points[points.Count - 2], points[1])); //right eye
        sticks.Add(new Stick(points[points.Count - 2], points[2])); //right eye
        sticks.Add(new Stick(points[points.Count - 2], points[8])); //right eye
        sticks.Add(new Stick(points[points.Count - 2], points[9])); //right eye
    }

    private void RenderPoints()
    {
        Vector3[] linePoints = new Vector3[points.Count - 2];

        for (int i = 0; i < points.Count - 2; i++) //-2 for the eyes 
        {
            linePoints[i] = points[i].current;
        }

        transform.GetChild(0).position = transform.TransformPoint(points[points.Count - 1].current);
        transform.GetChild(1).position = transform.TransformPoint(points[points.Count - 2].current);

        line.SetPositions(linePoints);
    }

    private void UpdatePoints()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 velocity = points[i].current - points[i].old;

            points[i].old = points[i].current;

            points[i].current += velocity;
            points[i].current += gravity  * Time.deltaTime * Time.deltaTime;

            //collision with ground
            float bound = transform.InverseTransformPoint(new Vector3(0, -4.0f)).y;
            if (points[i].current.y < bound)
            {
                points[i].current.y = bound;
                points[i].old.y = points[i].current.y + velocity.y;
            }
    
            //collision with stonehenge
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
        for (int i = 0; i < sticks.Count; i++)
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
