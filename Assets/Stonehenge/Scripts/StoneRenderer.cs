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

    private float lineWidth;

    void Awake()
    {
        PerlinNoise noise = new PerlinNoise(initialAmplitude, amplitudeScalingFactor, initialFrequency, frequencyScalingFactor, numberOfOctaves);

        LineRenderer line = gameObject.AddComponent<LineRenderer>();

        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = resolution + 1;

        int pointCount = resolution + 1; //+1 to connect the shape together
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            points[i] = new Vector3(i * width/pointCount, noise.GenerateNoise(i));
        }

        line.SetPositions(points);
    }
}
