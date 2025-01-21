//
//
//Modified from:
//https://gist.github.com/Shrimpey/f80ad90ac3845beda7b58f5672981494
//https://www.youtube.com/watch?v=YG-gIX_OvSE&t=99s

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorPlaneGen : MonoBehaviour
{   

    public int xVertCount = 3;
    public int yVertCount = 3;

    public float xSize = 1.0f;
    public float ySize = 1.0f;

    public Vector3WorldOp referenceOp;
    

    [Header("System Stuff (Usually Don't Touch")]
    public Vector3 refVectorA;
    public Vector3 refVectorB;
    public int centreXIndex;
    public int centreYIndex;
    public MeshRenderer myMeshRenderer;
    public MeshFilter myMeshFilter;
    public Plane referencePlane;
    public Vector2 stretchStep;


    void Awake() 
    {   
        TryGetComponent(out myMeshFilter);
        TryGetComponent(out myMeshRenderer);
    }

    void Start() 
    {
        if (myMeshFilter) 
        {
            referencePlane = new Plane(myMeshFilter.mesh, xVertCount, yVertCount, xSize, ySize);

            centreXIndex = xVertCount / 2;
            centreYIndex = yVertCount / 2;
        }

        stretchStep.x = xSize / xVertCount;
        stretchStep.y = ySize / yVertCount;
    }

    private void Update()
    {
        OperandSync();
    }

    public void OperandSync()
    {
        if (referenceOp != null)
        {
            if (referenceOp.operands.Count == 2)
            {
                if (myMeshRenderer != null)
                {
                    myMeshRenderer.enabled = true;
                }

                refVectorA = referenceOp.operands[0].value.value;
                refVectorB = referenceOp.operands[1].value.value;
                VectorSync();
            }
            else
            {
                if(myMeshRenderer != null)
                {
                    myMeshRenderer.enabled = false;
                }
            }
        }        
    }

    public void VectorSync()
    {
        //referencePlane.SetVertex(centreXIndex + 1, centreYIndex + 1, refVectorA + refVectorB);
        //p.SetVertex(centreXIndex, centreYIndex, Vector3.zero);




        //---- Origin is middle - Top right quadrant ----
        for (int i = 0; i <= centreXIndex; i++)
        {
            for (int j = 0; j <= centreYIndex; j++)
            {
                Vector3 newVertex = 
                    (i * refVectorA * stretchStep.x) + 
                    (j * refVectorB * stretchStep.y);

                referencePlane.SetVertex(centreXIndex + i, centreYIndex + j, newVertex);
            }
        }


        //---- Origin is middle - Top left quadrant ----
        for (int i = 0; i <= centreXIndex; i++)
        {
            for (int j = 0; j <= centreYIndex; j++)
            {
                Vector3 newVertex =
                    (-i * refVectorA * stretchStep.x) +
                    (j * refVectorB * stretchStep.y);

                referencePlane.SetVertex(centreXIndex - i, centreYIndex + j, newVertex);
            }
        }

        //---- Origin is middle - Bottom left quadrant ----
        for (int i = 0; i <= centreXIndex; i++)
        {
            for (int j = 0; j <= centreYIndex; j++)
            {
                Vector3 newVertex =
                    (-i * refVectorA * stretchStep.x) +
                    (-j * refVectorB * stretchStep.y);

                referencePlane.SetVertex(centreXIndex - i, centreYIndex - j, newVertex);
            }
        }

        //---- Origin is middle - Bottom right quadrant ----
        for (int i = 0; i <= centreXIndex; i++)
        {
            for (int j = 0; j <= centreYIndex; j++)
            {
                Vector3 newVertex =
                    (i * refVectorA * stretchStep.x) +
                    (-j * refVectorB * stretchStep.y);

                referencePlane.SetVertex(centreXIndex + i, centreYIndex - j, newVertex);
            }
        }

        //----- Origin is bottom left -----
        //for(int i = 0; i < xVertCount; i++)
        //{
        //    for(int j = 0; j < yVertCount; j++)
        //    {
        //        Vector3 newVertex = (i * refVectorA) + (j * refVectorB);
        //        referencePlane.SetVertex(i, j, newVertex);
        //        Debug.Log("VectorSync: " + i + "," + j);
        //    }
        //}

        //mf.mesh = p.GetMesh();
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

    public Mesh GetMesh()
    {
        return mesh_;
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
                //Debug.Log("Vertex: " + vertices[x + y * xCount_].ToString());
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


        mesh_.vertices = vertices;
    }
}