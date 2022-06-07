using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    private ShapeGenerator _shapeGenerator;
    private Mesh _mesh;
    private int resolution;
    private Vector3 localUp; // Know which direction we're facing
    private Vector3 axisA;
    private Vector3 axisB;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        _mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        this._shapeGenerator = shapeGenerator;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x); // Swap the axis
        axisB = Vector3.Cross(localUp, axisA); // Find a vector that is perpendicular to both axisA and localUp
    }

    public void ConstructMesh()
    {
        Vector3[] verices = new Vector3[resolution * resolution]; // Total number of vertices
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6]; // Hold the vertices of all the triangles
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution; // Number of iterations of inner loop
                
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnicCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointsOnUnitSphere = pointOnUnicCube.normalized;
                verices[i] = pointsOnUnitSphere;
                verices[i] = _shapeGenerator.CalculatePointOnPlanet(pointsOnUnitSphere);

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;
                    
                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6;
                }
            }
        }

        _mesh.Clear();
        _mesh.vertices = verices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }
}
