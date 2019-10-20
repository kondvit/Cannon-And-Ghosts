using System;
using System.Collections;
using Unity.Collections;
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


    private int resolution = 50; //controls the line smoothness
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



    //TODO: solution create 6 game objects 
    void Start()
    {

        //the resolutions are proportions of total points rendered with respect to the size
        //int lineResolution = (int)(resolution * width / (width + height) / 2);
        //int curveResolution = (int)(resolution * height / (width + height) / 2);

        var topLine = DrawLine(resolution, AlignTopLine);
        var bottomLine = DrawLine(resolution, AlignBottomLine);
        var rightSide = DrawCurvedSide(resolution, AlignRightSide);
        var leftSide = DrawCurvedSide(resolution, AlignLeftSide);

        //AlignShape(topLine, bottomLine, rightSide, leftSide);

        //Debug.Log(transform.position);
        //Debug.Log(transform.rotation.eulerAngles);
        //Debug.Log(transform.localScale);

    }


    private void AlignShape(GameObject topLine, GameObject bottomLine, GameObject rightSide, GameObject leftSide)
    {
        //Start by aligning the top line
        // then move to the sides
        //lastly the bottom line

        //top line is always higher than y = 0
        //can use unity function to find the angle

        //LineRenderer line = topLine.GetComponent<LineRenderer>();
        //Debug.Log(line.transform.position);
        //Debug.Log(topLine.transform.position);
        //Vector3 firstPoint = line.GetPosition(0);
        //Vector3[] pos = new Vector3[line.positionCount];
        //line.GetPositions(pos);
        //Vector3 lastPoint = line.GetPosition(line.positionCount - 1);
        //Debug.Log(line.transform.TransformDirection(firstPoint));
        //Debug.Log(line.transform.TransformDirection(lastPoint));

        var lineRenderer = topLine.GetComponent<LineRenderer>();
        var pos = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        Debug.Log(topLine.transform.TransformPoint(pos));
        Debug.Log(lineRenderer.transform.TransformPoint(pos));


        //align the topLine
        //AlignTopLine(topLine);
        //AlignBottomLine(bottomLine);
        //AlignRightSide(rightSide, bottomLine, topLine);

        //AlignLeftSide(leftSide);



        //TODO: just do half a circle and then scale it,




    }

    //private void AlignRightSide(GameObject rightSide, GameObject bottomLine, GameObject topLine)
    //{
    //    LineRenderer line = rightSide.GetComponent<LineRenderer>();
    //    Vector3 firstPoint = line.GetPosition(0);
    //    Vector3 lastPoint = line.GetPosition(line.positionCount - 1);



    //    LineRenderer bottomLineRenderer = bottomLine.GetComponent<LineRenderer>();
    //    Vector3 bottomLineLastPoint = bottomLineRenderer.GetPosition(bottomLineRenderer.positionCount - 1);

    //    LineRenderer topLineRenderer = topLine.GetComponent<LineRenderer>();
    //    Vector3 topLineLastPoint = topLineRenderer.GetPosition(topLineRenderer.positionCount - 1);

    //    bottomLineLastPoint = bottomLine.transform.TransformPoint(bottomLineLastPoint);
    //    topLineLastPoint = topLine.transform.TransformPoint(topLineLastPoint);

    //    bottomLineLastPoint = transform.InverseTransformPoint(bottomLineLastPoint);
    //    topLineLastPoint = transform.InverseTransformPoint(topLineLastPoint);


    //    //firstPoint = rightSide.transform.TransformPoint(firstPoint);
    //    //firstPoint = transform.InverseTransformPoint(firstPoint);

    //    //lastPoint = rightSide.transform.TransformPoint(lastPoint);
    //    //lastPoint = transform.InverseTransformPoint(lastPoint); // in parent coords

    //    float scale = (topLineLastPoint.y - bottomLineLastPoint.y) / (firstPoint.x - lastPoint.x);

    //    float angle = 90;
    //    rightSide.transform.Rotate(0, 0, -angle);

    //    //float scale = height / (firstPoint.x - lastPoint.x);
    //    rightSide.transform.localScale = new Vector3(scale, 1, 1);

    //    Debug.Log(bottomLineLastPoint);
    //    bottomLineLastPoint = transform.TransformPoint(bottomLineLastPoint);

    //    bottomLineLastPoint = rightSide.transform.InverseTransformPoint(bottomLineLastPoint);

    //    Debug.Log(bottomLineLastPoint);
    //    //Debug.Log(firstPoint);


    //    //Vector3 scaledPos = new Vector3(transform.localScale.x * 0.5f * width, transform.localScale.y * -0.5f * height);
    //    //Vector3 parentPosition = transform.TransformPoint(scaledPos);
    //    //Vector3 firstPointLastPos = rightSide.transform.InverseTransformPoint(parentPosition);
    //    rightSide.transform.position = bottomLineLastPoint;
    //    //rightSide.transform.Translate(bottomLineLastPoint - firstPoint);

    //}

    private void AlignLeftSide(GameObject leftSide)
    {
        LineRenderer line = leftSide.GetComponent<LineRenderer>();
        Vector3 firstPoint = line.GetPosition(0);
        Vector3 lastPoint = line.GetPosition(line.positionCount - 1);

        float scale = height / (firstPoint.x - lastPoint.x);
        leftSide.transform.localScale = new Vector3(scale, 1, 1);

        float angle = 90;
        leftSide.transform.Rotate(0, 0, angle);

        Vector3 scaledPos = new Vector3(transform.localScale.x * -0.5f * width, transform.localScale.y * 0.5f * height);
        Vector3 parentPosition = transform.TransformPoint(scaledPos);
        Vector3 firstPointLastPos = leftSide.transform.InverseTransformPoint(parentPosition);
        leftSide.transform.Translate(firstPointLastPos - firstPoint);

    }

    //private void AlignTopLine(GameObject topLine)
    //{
    //    LineRenderer line = topLine.GetComponent<LineRenderer>();
       
    //    Vector3 firstPoint = line.GetPosition(0);
    //    Vector3 lastPoint = line.GetPosition(line.positionCount - 1);

    //    Vector3 connectedEndPoints = lastPoint - firstPoint;
    //    topLine.transform.Rotate(0, 0, -Vector3.SignedAngle(Vector3.right, connectedEndPoints, Vector3.forward));

    //    Vector3 scaledPos = new Vector3(transform.localScale.x * -width / 2, transform.localScale.y * height / 2);
    //    Vector3 parentPosition = transform.TransformPoint(scaledPos);
    //    Vector3 finalFirstPointPos = topLine.transform.InverseTransformPoint(parentPosition);
    //    topLine.transform.Translate(finalFirstPointPos - firstPoint);

    //}

    //private void AlignBottomLine(GameObject bottomLine)
    //{
    //    LineRenderer line = bottomLine.GetComponent<LineRenderer>();
    //    Vector3 firstPoint = line.GetPosition(0);
    //    Vector3 lastPoint = line.GetPosition(line.positionCount - 1);

    //    Vector3 connectedEndPoints = lastPoint - firstPoint;
    //    bottomLine.transform.Rotate(0, 0, -Vector3.SignedAngle(Vector3.right, connectedEndPoints, Vector3.forward));

    //    Vector3 scaledPos = new Vector3(transform.localScale.x * -width / 2, transform.localScale.y * -height / 2);
    //    Vector3 parentPosition = transform.TransformPoint(scaledPos);
    //    Vector3 finalFirstPointPos = bottomLine.transform.InverseTransformPoint(parentPosition);
    //    bottomLine.transform.Translate(finalFirstPointPos - firstPoint);

    //}

    private GameObject DrawCurvedSide(int resolution, Action<Vector3[]> Align)
    {

        GameObject curve = new GameObject("Curve");
        curve.transform.SetParent(transform, false);

        LineRenderer lineRenderer = curve.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = resolution;
        lineRenderer.material = material;

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

        return curve;
    }

    private GameObject DrawLine(int resolution, Action<Vector3[]> Align)
    {
        GameObject line = new GameObject("Line");
        line.transform.SetParent(transform, false);

        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = resolution;
        lineRenderer.material = material;

        int pointCount = resolution; 
        Vector3[] points = new Vector3[pointCount];

        PerlinNoise noise = new PerlinNoise(initialAmplitude, amplitudeScalingFactor, initialFrequency, frequencyScalingFactor, numberOfOctaves);

        //Vector3 offSet = new Vector3(-0.5f * width, 0.5f * height);

        //float x = 0;
        //float y = noise.GenerateNoise(x);
        //points[0] = new Vector3(x, y) + offSet;



        for (int i = 0; i < pointCount; i++)
        {
            float x = i * width / (pointCount - 1);
            float y = noise.GenerateNoise(x);

            points[i] = new Vector3(x, y);
        }

        Align(points);

        lineRenderer.SetPositions(points); //transform is applied here localy



        return line;
    }

    private void AlignTopLine(Vector3[] points)
    {
        Vector3 connectedEndPoints = points[points.Length - 1] - points[0];
        Quaternion rotation = Quaternion.Euler(0, 0, -Vector3.SignedAngle(Vector3.right, connectedEndPoints, Vector3.forward));
        
        Vector3 translation = new Vector3(-0.5f * width, 0.5f * height) - points[0];
        //Vector3 translation = new Vector3(-0.5f * width, 0.5f * height);
        //Matrix4x4 m1 = Matrix4x4.Rotate(rotation);
        //Matrix4x4 m2 = Matrix4x4.Translate(translation);//, rotation, Vector3.one);
        Matrix4x4 m = Matrix4x4.TRS(translation, rotation, Vector3.one);//, rotation, Vector3.one);

        for(int i = 0; i < points.Length; i++)
        {
            points[i] = m.MultiplyPoint3x4(points[i]);
            //points[i] = m2.MultiplyPoint3x4(points[i]);
        }





        //    topLine.transform.Rotate(0, 0, -Vector3.SignedAngle(Vector3.right, connectedEndPoints, Vector3.forward));
    }

    private void AlignBottomLine(Vector3[] points)
    {
        Vector3 connectedEndPoints = points[points.Length - 1] - points[0];
        Quaternion rotation = Quaternion.Euler(0, 0, -Vector3.SignedAngle(Vector3.right, connectedEndPoints, Vector3.forward));

        Vector3 translation = new Vector3(-0.5f * width, -0.5f * height) - points[0];
        //Vector3 translation = new Vector3(-0.5f * width, -0.5f * height);
        //Matrix4x4 m1 = Matrix4x4.Rotate(rotation);
        //Matrix4x4 m2 = Matrix4x4.Translate(translation);//, rotation, Vector3.one);
        Matrix4x4 m = Matrix4x4.TRS(translation, rotation, Vector3.one);//, rotation, Vector3.one);

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = m.MultiplyPoint3x4(points[i]);
            //points[i] = m2.MultiplyPoint3x4(points[i]);
        }





        //    topLine.transform.Rotate(0, 0, -Vector3.SignedAngle(Vector3.right, connectedEndPoints, Vector3.forward));
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

        //Matrix4x4 m4 = Matrix4x4.TRS(translation, rotation, new Vector3(1, scale, 1));

        for (int i = 1; i < points.Length; i++)
        {
            points[i] = m1.MultiplyPoint3x4(points[i]);
            points[i] = m2.MultiplyPoint3x4(points[i]);
            points[i] = m3.MultiplyPoint3x4(points[i]);
            //points[i] = m2.MultiplyPoint3x4(points[i]);
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

        //Matrix4x4 m4 = Matrix4x4.TRS(translation, rotation, new Vector3(1, scale, 1));

        for (int i = 1; i < points.Length; i++)
        {
            points[i] = m1.MultiplyPoint3x4(points[i]);
            points[i] = m2.MultiplyPoint3x4(points[i]);
            points[i] = m3.MultiplyPoint3x4(points[i]);
            //points[i] = m2.MultiplyPoint3x4(points[i]);
        }

    }

}
