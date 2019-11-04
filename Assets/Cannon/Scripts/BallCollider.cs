using UnityEngine;

public class BallCollider : MonoBehaviour
{
    private Vector3 ballVelocity;
    private Vector3[] convexHall; //convex hall of the cannon ball
    private CannonBallController ballController;
    private float coefficientOfRest = 0.75f; //coefficient of restitution

    void Start()
    {
        ballController = GetComponent<CannonBallController>();
    }

    void Update()
    {
        ballVelocity = ballController.ballVelocity;
        convexHall = ballController.convexHall;

        ResolveCollisions();
    }

    private void ResolveCollisions()
    {
        ResolveCollisionWithRock(GameObject.Find("TopStone"));
        ResolveCollisionWithRock(GameObject.Find("RightStone"));

        ResolveCollisionWithGhost(GameObject.FindGameObjectsWithTag("Ghost"));
    }

    private void ResolveCollisionWithGhost(GameObject[] ghosts)
    {
        foreach(GameObject ghost in ghosts)
        {
            GhostController ghostController = ghost.GetComponent<GhostController>();
            try
            {
                for (int i = 0; i < ghostController.points.Count; i++) //for every verlet point in ghost
                {
                    Vector3 verletPoint = ghost.transform.TransformPoint(ghostController.points[i].current); //get point position in world coords
                    float distance = (transform.position - verletPoint).magnitude; //distance from point to center of ball

                    if (distance < ballController.radius)// then its inside the ball
                    {
                        // give velocity to that point
                        verletPoint += transform.TransformDirection(ballController.translation);
                        ghostController.points[i].current = ghost.transform.InverseTransformPoint(verletPoint);

                        Destroy(gameObject); // destroys cannon ball
                        return;
                    }

                }
            }
            catch { }
        }
    }

    private void ResolveCollisionWithRock(GameObject rock)
    {
        for(int i = 1; i < 4; i++) //check collision with 3 outside sides of each rock
        {
            LineRenderer line = rock.transform.GetChild(i).gameObject.GetComponent<LineRenderer>();

            Vector3[] points = new Vector3[line.positionCount];
            line.GetPositions(points);

            for(int k = 0; k < convexHall.Length; k++) //for every point of the ball
            {
                for (int j = 1; j < points.Length; j++) //for every point on a side of the rock
                {
                    
                    Vector3 firstPointOnStone = line.transform.TransformPoint(points[j - 1]);
                    Vector3 secondPointOnStone = line.transform.TransformPoint(points[j]); 

                    Vector3 circlePoint = transform.TransformPoint(convexHall[k]); 

                    bool crossing = AreCrossing(firstPointOnStone, secondPointOnStone, transform.position, circlePoint); //checks if little edge crosses the ball's radius
                    if (crossing) //resolve
                    {
                        transform.Translate(-ballController.translation); //translate back to the previous frame

                        Vector3 edge = secondPointOnStone - firstPointOnStone; //get collided little edge of the rock
                        Vector3 normal = new Vector3(-edge.y, edge.x).normalized; //get the normal vector
                        ballController.ballVelocity = ballVelocity - 2 * Vector3.Dot(ballVelocity, normal) * normal; //reflect
                        ballController.ballVelocity *= coefficientOfRest; //apply restitution

                        return; //don't continue collision detection after resolution
                    }
                }
            }

        }
    


    }

    //Outputs True if crossing, if on the edge, outputs as non crossing
    //standard dot product test
    private bool AreCrossing(Vector3 v0p0, Vector3 v0p1, Vector3 v1p0, Vector3 v1p1)
    {
        Vector3 v0 = v0p1 - v0p0;
        Vector3 v1 = v1p1 - v1p0;

        //first test
        Vector3 p1_To_p1 = v1p1 - v0p1;
        Vector3 p0_To_p1 = v1p1 - v0p0;

        float norm_1 = Mathf.Sign(v1.x * p1_To_p1.y - v1.y * p1_To_p1.x);
        float norm_2 = Mathf.Sign(v1.x * p0_To_p1.y - v1.y * p0_To_p1.x);
        
        if(norm_1 == norm_2 || norm_1 == 0 || norm_2 == 0)
        {
            return false;
        }
        else
        {
            //second test
            p1_To_p1 = v0p1 - v1p1;
            p0_To_p1 = v0p1 - v1p0;

            norm_1 = Mathf.Sign(v0.x * p1_To_p1.y - v0.y * p1_To_p1.x);
            norm_2 = Mathf.Sign(v0.x * p0_To_p1.y - v0.y * p0_To_p1.x);

            if (norm_1 == norm_2 || norm_1 == 0 || norm_2 == 0)
            {
                return false; //not crossing
            }
            else
            {
                return true;
            }
        }
    }
}
