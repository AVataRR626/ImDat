//
//
//Modified from:
//https://gist.github.com/Shrimpey/f80ad90ac3845beda7b58f5672981494
//https://www.youtube.com/watch?v=YG-gIX_OvSE&t=99s

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGen : MonoBehaviour
{
    private MeshFilter mf;

    public int xVertCount = 3;
    public int yVertCount = 3;

    public float xSize = 1.0f;
    public float ySize = 1.0f;

    public Vector3 refVectorA;
    public Vector3 refVectorB;


    private void Awake() 
    {
        
        TryGetComponent(out mf);
    }

    private void Start() 
    {
        if (mf) 
        {
            Plane plane = new Plane(mf.mesh, xVertCount, yVertCount, xSize, ySize);
        }
    }
}


public abstract class ProceduralShape 
{
    protected Mesh mesh_;
    protected Vector3[] vertices;
    protected int[] triangles;
    protected Vector2[] uvs;

    public ProceduralShape(Mesh mesh) 
    {
        mesh_ = mesh;
    }
}

public class Plane : ProceduralShape 
{
    private int xCount_, yCount_;

    private float xStep_;
    private float yStep_;


    public Plane(Mesh mesh, int xCount, int yCount, float xSize, float ySize) : base(mesh) 
    {
        xCount_ = xCount;
        yCount_ = yCount;

        xStep_ = xSize / xCount;
        yStep_ = ySize / yCount;

        CreateMesh();
    }

    private void CreateMesh() 
    {
        CreateVertices();
        //CreateTrianglesOneFace();
        CreateTrianglesTwoFace();
        CreateUVs();

        mesh_.vertices = vertices;
        mesh_.triangles = triangles;
        mesh_.uv = uvs;

        mesh_.RecalculateNormals();
    }
    private void CreateVertices() 
    {
        vertices = new Vector3[xCount_ * yCount_];
        for (int y=0; y<yCount_; y++) 
        {
            for (int x = 0; x < xCount_; x++) 
            {
                vertices[x + y * xCount_] = new Vector3(x * xStep_, 0.0f, y * yStep_);
                Debug.Log("Vertex: " + vertices[x + y * xCount_].ToString());
            }
        }
    }
    private void CreateTrianglesOneFace()
    {
        triangles = new int[3*2*(xCount_ * yCount_ - xCount_ - yCount_ + 1)];
        int triangleVertexCount = 0;
        for(int vertex = 0; vertex < xCount_ * yCount_ - xCount_; vertex++) 
        {
            if(vertex%xCount_ != (xCount_ - 1)) 
            {
                //--Top Facing--
                // First triangle
                int A = vertex;
                int B = A + xCount_;
                int C = B + 1;
                int D = A + 1;
                triangles[triangleVertexCount] = A;
                triangles[triangleVertexCount + 1] = B;
                triangles[triangleVertexCount + 2] = C;
                //Second triangle
                triangles[triangleVertexCount + 3] = A;
                triangles[triangleVertexCount + 4] = C;
                triangles[triangleVertexCount + 5] = D;
                triangleVertexCount += 6;

            }
        }
    }

    private void CreateTrianglesTwoFace()
    {
        triangles = new int[3 * 2 * 2 * (xCount_ * yCount_ - xCount_ - yCount_ + 1)];
        int triangleVertexCount = 0;
        for (int vertex = 0; vertex < xCount_ * yCount_ - xCount_; vertex++)
        {
            if (vertex % xCount_ != (xCount_ - 1))
            {
                //--Top Facing--
                // First triangle
                int A = vertex;
                int B = A + xCount_;
                int C = B + 1;
                int D = A + 1;
                triangles[triangleVertexCount] = A;
                triangles[triangleVertexCount + 1] = B;
                triangles[triangleVertexCount + 2] = C;

                //Second triangle
                triangles[triangleVertexCount + 3] = A;
                triangles[triangleVertexCount + 4] = C;
                triangles[triangleVertexCount + 5] = D;


                //--Bottom Facing--
                //First triangle
                triangles[triangleVertexCount + 6] = A;
                triangles[triangleVertexCount + 7] = C;
                triangles[triangleVertexCount + 8] = B;

                //Second triangle
                triangles[triangleVertexCount + 9] = A;
                triangles[triangleVertexCount + 10] = D;
                triangles[triangleVertexCount + 11] = C;


                triangleVertexCount += 12;
            }
        }
    }

    private void CreateUVs() 
    {
        uvs = new Vector2[xCount_ * yCount_];
        int uvIndexCounter = 0;
        foreach(Vector3 vertex in vertices) 
        {
            uvs[uvIndexCounter] = new Vector2(vertex.x, vertex.z);
            uvIndexCounter++;
        }
    }

    public void SetVertex(int x, int y, Vector3 newPos)
    {
        vertices[x + y * xCount_] = newPos;
    }
}