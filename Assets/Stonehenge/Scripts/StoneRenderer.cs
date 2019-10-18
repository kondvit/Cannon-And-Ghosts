using UnityEngine;

public class StoneRenderer : MonoBehaviour
{
    [Header("Perlin Noise Settings")]
    [SerializeField]
    private float initialAmplitude = 1f;
    [SerializeField]
    private float amplitudeScalingFactor = 0.5f;
    [SerializeField]
    private float initialFrequency = 1f;
    [SerializeField]
    private float frequencyScalingFactor = 2f;
    [SerializeField]
    private int numberOfOctaves = 5;
    [SerializeField]
    private int resolution = 300; //controls the line smoothness


    [Header("Stone Settings")]
    [SerializeField]
    private float height = 4; //controls the line smoothness
    [SerializeField]
    private float width = 8; //controls the line smoothness
    [SerializeField]
    private float lineWidth = 0.02f;
    [SerializeField]
    private float radius;

    //TODO: solution create 6 game objects 
    void Awake()
    {
        //TODO: create a second noise for the second line ???
        if (radius < 2) radius = new Vector2(width / 2, height / 2).magnitude;

        //the resolutions are proportions of total points rendered with respect to the size
        int lineResolution = (int)(resolution * width / (width + height) / 2);
        int curveResolution = (int)(resolution * height / (width + height) / 2);

        GameObject topLine = DrawLine(lineResolution);
        topLine.transform.position = new Vector3(-width / 2, height / 2);

        // GameObject rightSide = DrawCurvedSide(curveResolution, radius);

    }

    //TOD:
    private GameObject DrawCurvedSide(int resolution, float radius)
    {

        GameObject curve = new GameObject("Curve");
        curve.transform.SetParent(transform);

        LineRenderer lineRenderer = curve.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = resolution;

        int pointCount = resolution; //+1 to connect the shape
        Vector3[] points = new Vector3[pointCount];

        float angle = Mathf.Acos(Mathf.Pow(height, 2) / (-2 * Mathf.Pow(radius, 2)) + 1); //law of cosines formula to find angle from radius

        PerlinNoise noise = new PerlinNoise(initialAmplitude, amplitudeScalingFactor, initialFrequency, frequencyScalingFactor, numberOfOctaves);

        for (int i = 0; i < pointCount; i++)
        {
            
            float section = i * angle / pointCount;
            float n = noise.GenerateNoise(section * radius);
            float x = Mathf.Cos(section) * (radius + n);
            float y = Mathf.Sin(section) * (radius + n);
            points[i] = new Vector3(x, y);
        }

        lineRenderer.SetPositions(points);

        return curve;
    }

    private GameObject DrawLine(int resolution)
    {
        GameObject line = new GameObject("Line");
        line.transform.SetParent(transform);

        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = resolution;

        int pointCount = resolution; //+1 to connect the shape 
        Vector3[] points = new Vector3[pointCount];

        PerlinNoise noise = new PerlinNoise(initialAmplitude, amplitudeScalingFactor, initialFrequency, frequencyScalingFactor, numberOfOctaves);

        for (int i = 0; i < pointCount; i++)
        {
            float x = i * width / pointCount;
            float y = noise.GenerateNoise(x);
            points[i] = new Vector3(x, y);
        }

        lineRenderer.SetPositions(points);

        return line;
    }
}
