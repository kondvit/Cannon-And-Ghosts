using System;
using Unity.Collections;
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

        var topLine = DrawLine(lineResolution);
        var bottomLine = DrawLine(lineResolution);
        var rightSide = DrawCurvedSide(curveResolution, radius);
       // var leftSide = DrawCurvedSide(curveResolution, radius);

        AlignShape(topLine, bottomLine, rightSide, topLine);
        //AlignShape(topLine, bottomLine, rightSide, leftSide);

    }

    private void AlignShape((GameObject, Vector3[]) topLine, (GameObject, Vector3[]) bottomLine, (GameObject, Vector3[]) rightSide, (GameObject, Vector3[]) leftSide)
    {
        //Start by aligning the top line
        // then move to the sides
        //lastly the bottom line

        //top line is always higher than y = 0
        //can use unity function to find the angle

        //align the topLine
        AlignTopLine(topLine);
        AlignRightSide(rightSide);
        //AlignBottomLine(bottomLine);



        //TODO: just do half a circle and then scale it,




    }

    private void AlignRightSide((GameObject, Vector3[]) rightSide)
    {
        float angle = 0.5f * Vector3.Angle(Vector3.right, rightSide.Item2[1]);
        rightSide.Item1.transform.Rotate(0, 0, -angle);
        rightSide.Item1.transform.position = new Vector3(width/2 - 0.1f, 0);
    }

    private void AlignTopLine((GameObject, Vector3[]) topLine)
    {
        Vector3 connectedEndPoints = topLine.Item2[1] - topLine.Item2[0];
        topLine.Item1.transform.Rotate(0, 0, -Vector3.SignedAngle(Vector3.right, connectedEndPoints, Vector3.forward));
        topLine.Item1.transform.position = new Vector3(-width / 2, height / 2);
    }

    private void AlignBottomLine((GameObject, Vector3[]) bottomLine)
    {
        Vector3 connectedEndPoints = bottomLine.Item2[1] - bottomLine.Item2[0];
        bottomLine.Item1.transform.Rotate(0, 0, -Vector3.SignedAngle(Vector3.right, connectedEndPoints, Vector3.forward));
        bottomLine.Item1.transform.position = new Vector3(-width / 2, -height / 2);
    }

    private (GameObject, Vector3[]) DrawCurvedSide(int resolution, float radius)
    {

        GameObject curve = new GameObject("Curve");
        curve.transform.SetParent(transform);

        LineRenderer lineRenderer = curve.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = resolution;

        int pointCount = resolution; 
        Vector3[] points = new Vector3[pointCount];
        Vector3 firstPoint = Vector3.zero;
        Vector3 lastPoint = Vector3.zero;
        float firstRadiusDifference = 0;


        float angle = Mathf.Acos(Mathf.Pow(height, 2) / (-2 * Mathf.Pow(radius, 2)) + 1); //law of cosines formula to find angle from radius

        PerlinNoise noise = new PerlinNoise(initialAmplitude, amplitudeScalingFactor, initialFrequency, frequencyScalingFactor, numberOfOctaves);

        for (int i = 0; i < pointCount; i++)
        {
            
            float section = i * angle / pointCount;
            float n = noise.GenerateNoise(section * radius);
            float x = Mathf.Cos(section) * (radius + n - firstRadiusDifference);
            float y = Mathf.Sin(section) * (radius + n - firstRadiusDifference);
       
            points[i] = transform.TransformPoint(new Vector3(x, y));

            if (i == 0)
            {
                firstPoint = new Vector3(x, y);
                x = Mathf.Cos(section) * radius;
                y = Mathf.Sin(section) * radius;
                points[i] = transform.TransformPoint(new Vector3(x, y));
                firstRadiusDifference = n;
            }
            if (i == pointCount - 1)
            {
                lastPoint = new Vector3(x, y);
            }
        }

        lineRenderer.SetPositions(points);

        return (curve, new Vector3[2] { firstPoint, lastPoint });
    }

    private (GameObject, Vector3[]) DrawLine(int resolution)
    {
        GameObject line = new GameObject("Line");
        line.transform.SetParent(transform);

        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = resolution;

        int pointCount = resolution; 
        Vector3[] points = new Vector3[pointCount];
        Vector3 firstPoint = Vector3.zero;
        Vector3 lastPoint = Vector3.zero;

        PerlinNoise noise = new PerlinNoise(initialAmplitude, amplitudeScalingFactor, initialFrequency, frequencyScalingFactor, numberOfOctaves);

        for (int i = 0; i < pointCount; i++)
        {
            float x = i * width / pointCount;
            float y = noise.GenerateNoise(x) - firstPoint.y;

            points[i] = transform.TransformPoint(new Vector3(x, y));

            if(i == 0)
            {
                firstPoint = new Vector3(x, y);
                points[i] = transform.TransformPoint(Vector3.zero);
            }
            if(i == pointCount - 1)
            {
                lastPoint = new Vector3(x, y);
            }

        }
        
        lineRenderer.SetPositions(points);

        return (line, new Vector3[2] { Vector3.zero, lastPoint });
    }
}
