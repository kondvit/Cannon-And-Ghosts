using System;
using UnityEngine;

public class StoneRenderer : MonoBehaviour
{
    [Header("Perlin Noise Settings")]
    [SerializeField]
    private float initialAmplitude = 1f;
    [SerializeField]
    private float initialFrequency = 1f;
    [SerializeField]
    private int numberOfOctaves = 5;

    [SerializeField]
    private int resolution = 50; //controls the line smoothness, number of sampled noise values
    private float amplitudeScalingFactor = 0.5f;
    private float frequencyScalingFactor = 2f;

    [Header("Stone Settings")]
    [SerializeField]
    private float lineWidth = 0.02f;
    [SerializeField]
    private Material material;

    private float height = 4; 
    private float width = 8; 
    private float radius = 2;

    void Awake()
    {
        DrawLine(resolution, AlignTopLine); //top line
        DrawLine(resolution, AlignBottomLine); //bottom line
        DrawCurvedSide(resolution, AlignRightSide); //right side
        DrawCurvedSide(resolution, AlignLeftSide); //left side
    }

    private void DrawCurvedSide(int resolution, Action<Vector3[]> Align)
    {

        GameObject curve = new GameObject("Curve");
        curve.transform.SetParent(transform, false);

        LineRenderer lineRenderer = curve.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = resolution;
        lineRenderer.material = material;
        lineRenderer.numCapVertices = 5;

        int pointCount = resolution; 
        Vector3[] points = new Vector3[pointCount];

        float angle = Mathf.PI;

        PerlinNoise noise = new PerlinNoise(initialAmplitude, amplitudeScalingFactor, initialFrequency, frequencyScalingFactor, numberOfOctaves);

        for (int i = 0; i < pointCount; i++)
        {
            
            float section = i * angle / pointCount;
            float n = noise.GenerateNoise(section * radius);
            float x = Mathf.Cos(section) * (radius + n);
            float y = Mathf.Sin(section) * (radius + n);
       
            points[i] = new Vector3(x, y);

            if (i == pointCount - 1)
            {
                points[i] = new Vector3(x, 0);
            }
        }

        Align(points);

        lineRenderer.SetPositions(points);
    }

    private void DrawLine(int resolution, Action<Vector3[]> Align)
    {
        GameObject line = new GameObject("Line");
        line.transform.SetParent(transform, false);

        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = resolution;
        lineRenderer.material = material;
        lineRenderer.numCapVertices = 5;

        int pointCount = resolution; 
        Vector3[] points = new Vector3[pointCount];

        PerlinNoise noise = new PerlinNoise(initialAmplitude, amplitudeScalingFactor, initialFrequency, frequencyScalingFactor, numberOfOctaves);

        for (int i = 0; i < pointCount; i++)
        {
            float x = i * width / (pointCount - 1);
            float y = noise.GenerateNoise(x);

            points[i] = new Vector3(x, y);
        }

        Align(points);

        lineRenderer.SetPositions(points);

    }

    private void AlignTopLine(Vector3[] points)
    {
        Vector3 connectedEndPoints = points[points.Length - 1] - points[0];
        Quaternion rotation = Quaternion.Euler(0, 0, -Vector3.SignedAngle(Vector3.right, connectedEndPoints, Vector3.forward));
        
        Vector3 translation = new Vector3(-0.5f * width, 0.5f * height) - points[0];
        Matrix4x4 m = Matrix4x4.TRS(translation, rotation, Vector3.one);

        for(int i = 0; i < points.Length; i++)
        {
            points[i] = m.MultiplyPoint3x4(points[i]);
        }
    }

    private void AlignBottomLine(Vector3[] points)
    {
        Vector3 connectedEndPoints = points[points.Length - 1] - points[0];
        Quaternion rotation = Quaternion.Euler(0, 0, -Vector3.SignedAngle(Vector3.right, connectedEndPoints, Vector3.forward));

        Vector3 translation = new Vector3(-0.5f * width, -0.5f * height) - points[0];
        Matrix4x4 m = Matrix4x4.TRS(translation, rotation, Vector3.one);

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = m.MultiplyPoint3x4(points[i]);
        }
    }

    private void AlignRightSide(Vector3[] points)
    {
        float scale = height / (points[0].x - points[points.Length - 1].x);
        
        Quaternion rotation = Quaternion.Euler(0, 0, -90);
        Matrix4x4 m1 = Matrix4x4.Rotate(rotation);
        Matrix4x4 m2 = Matrix4x4.Scale(new Vector3(1, scale, 1));

        points[0] = m1.MultiplyPoint3x4(points[0]);
        points[0] = m2.MultiplyPoint3x4(points[0]);
        Vector3 translation = new Vector3(0.5f * width, -0.5f * height) - points[0];
        Matrix4x4 m3 = Matrix4x4.Translate(translation);
        points[0] = m3.MultiplyPoint3x4(points[0]);

        for (int i = 1; i < points.Length; i++)
        {
            points[i] = m1.MultiplyPoint3x4(points[i]);
            points[i] = m2.MultiplyPoint3x4(points[i]);
            points[i] = m3.MultiplyPoint3x4(points[i]);
        }

    }

    private void AlignLeftSide(Vector3[] points)
    {
        float scale = height / (points[0].x - points[points.Length - 1].x);

        Quaternion rotation = Quaternion.Euler(0, 0, 90);
        Matrix4x4 m1 = Matrix4x4.Rotate(rotation);
        Matrix4x4 m2 = Matrix4x4.Scale(new Vector3(1, scale, 1));

        points[0] = m1.MultiplyPoint3x4(points[0]);
        points[0] = m2.MultiplyPoint3x4(points[0]);
        Vector3 translation = new Vector3(-0.5f * width, 0.5f * height) - points[0];
        Matrix4x4 m3 = Matrix4x4.Translate(translation);
        points[0] = m3.MultiplyPoint3x4(points[0]);

        for (int i = 1; i < points.Length; i++)
        {
            points[i] = m1.MultiplyPoint3x4(points[i]);
            points[i] = m2.MultiplyPoint3x4(points[i]);
            points[i] = m3.MultiplyPoint3x4(points[i]);
        }

    }

}
