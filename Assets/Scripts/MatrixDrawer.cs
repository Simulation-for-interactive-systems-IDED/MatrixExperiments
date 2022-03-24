using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixDrawer : MonoBehaviour
{
    [Header("Matrix transform")]
    [SerializeField]
    Vector3 position;
    [SerializeField]
    Vector3 rotation;
    [SerializeField]
    Vector3 scale;

    [Header("Drawing related")]
    [SerializeField]
    float stepSize = 0.15f;
    [SerializeField]
    int totalSteps = 50;
    [SerializeField]
    bool drawXY = true;
    [SerializeField]
    bool drawZX = true;
    [SerializeField]
    bool drawYZ = true;
    [SerializeField]
    Transform otherObject;


    // Internals
    Matrix4x4 matrix;
    Vector3 otherObjectInitialPosition;


    private void Start()
    {
        otherObjectInitialPosition = otherObject.position;
    }

    private void Update()
    {
        matrix = Matrix4x4.TRS(position, Quaternion.Euler(rotation), scale);
        UpateOtherObject();
        DrawBasis();
        DrawPlanes();
    }

    private void UpateOtherObject()
    {
        if (otherObject == null) return;
        otherObject.position = otherObjectInitialPosition;
        otherObject.position = matrix.MultiplyPoint3x4(otherObject.position);
    }

    private void DrawBasis()
    {
        Vector3 pos = matrix.GetColumn(3);
        Debug.DrawRay(pos, matrix.GetColumn(0), Color.red);
        Debug.DrawRay(pos, matrix.GetColumn(1), Color.green);
        Debug.DrawRay(pos, matrix.GetColumn(2), Color.blue);
    }

    private void DrawPlanes()
    {
        Vector3 pos = matrix.GetColumn(3);
        Vector3 xAxis = matrix.GetColumn(0);
        Vector3 yAxis = matrix.GetColumn(1);
        Vector3 zAxis = matrix.GetColumn(2);
        if (drawXY) DrawGrid(pos, xAxis, yAxis, scale.x, scale.y);
        if (drawZX) DrawGrid(pos, zAxis, xAxis, scale.z, scale.x);
        if (drawYZ) DrawGrid(pos, yAxis, zAxis, scale.y, scale.z);
    }

    private void DrawGrid(Vector3 pos, Vector3 xAxis, Vector3 yAxis, float scaleX, float scaleY)
    {
        for (int i = 1; i <= totalSteps; ++i)
        {
            // Draw x-y grid
            Debug.DrawRay(pos + xAxis * stepSize * i, yAxis.normalized * stepSize * totalSteps * Mathf.Abs(scaleY));
            Debug.DrawRay(pos + yAxis * stepSize * i, xAxis.normalized * stepSize * totalSteps * Mathf.Abs(scaleX));
        }
    }
}
