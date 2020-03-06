using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RoundedCube : MonoBehaviour {
    public int xSize, ySize, zSize;
    public int roundness;

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;

    private void Awake() {
        Generate();
    }

    private void Generate() {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";
        CreateVertices();
        CreateTriangles();
    }
    
    private void SetVertex (int i, int x, int y, int z) {
        vertices[i] = new Vector3(x, y, z);
    }

    private void CreateVertices() {
        
        // for values x == 3, y == 2, z == 4 
        // calculation will be:
        // cornerVertices == 9
        // edgeVertices == (3 + 2 + 4 - 3) * 4 == 24
        // faceVertices == ((3 - 1) * (2 - 1) + (3 - 1) * (4 - 1) + (2 - 1) * (4 - 1)) * 2
        //              == (2 * 1 + 2 * 3 + 1 * 3) * 2 = 11 * 2 == 22
        
        int cornerVertices = 9;
        int edgeVertices = (xSize + ySize + zSize - 3) * 4;
        int faceVertices = ((xSize - 1) * (ySize - 1) + 
                            (xSize - 1) * (zSize - 1) +
                            (ySize - 1) * (zSize - 1)) * 2;
        
        // so our final array size == (cornerVertices + edgeVertices + faceVertices) == 9 + 24 + 22 == 55
        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
        normals = new Vector3[vertices.Length];
        
        int v = 0;
        
        // to build walls
        for (int y = 0; y <= ySize; y++) {
            
            for (int x = 0; x <= xSize; x++) {
                SetVertex(v++, x, y, 0);            }
            
            for (int z = 1; z <= zSize; z++) {
                SetVertex(v++, xSize, y, z);            }
            
            for (int x = xSize - 1; x >= 0; x--) {
                SetVertex(v++, x, y, zSize);            }
            
            for (int z = zSize - 1; z > 0; z--) {
                SetVertex(v++, 0, y, z);            }
        }
        
        // to fill top
        for (int z = 1; z < zSize; z++) {
            
            for (int x = 1; x < xSize; x++) {
                SetVertex(v++, x, ySize, z);            }
        }
        
        // to fill bottom
        for (int z = 1; z < zSize; z++) {
            
            for (int x = 1; x < xSize; x++) {
                SetVertex(v++, x, 0, z);            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
    }
         
    private void CreateTriangles() {
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        
        // the number of triangles is
        // simply equal to that of the six faces combined.
        // It doesn't matter whether they use shared vertices or not.
        int[] triangles = new int[quads * 6];
        
        int ring = (xSize + zSize) * 2;
        
        int t = 0, v = 0;

        // to triangulate all rings along Y axis
        for (int y = 0; y < ySize; y++, v++) {
            
            // to triangulate an entire ring
            // this works, except for the last quad.
            // its second and fourth vertex need to rewind to the start of the ring.
            // so we extract it from the loop.
            for (int q = 0; q < ring - 1; q++, v++) {
                t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
            }
            // right here
            t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
            
        }

        t = CreateTopFace(triangles, t, ring);
        t = CreateBottomFace(triangles, t, ring);
            
        mesh.triangles = triangles;
    }

    private int CreateTopFace(int[] triangles, int t, int ring) {
        int v = ring * ySize;

        for (int x = 0; x < xSize - 1; x++, v++) {
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
        }

        t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);
        
        int vMin = ring * (ySize + 1) - 1;
        int vMid = vMin + 1;
        int vMax = v + 2;

        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
            t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
            for (int x = 1; x < xSize - 1; x++, vMid++) {
                t = SetQuad(
                    triangles, t,
                    vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
            }
            t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
        }

        int vTop = vMin - 2;
        t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);

        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
            t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
        }

        t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);
        return t;
    }

    private int CreateBottomFace(int[] triangles, int t, int ring) {
        int v = 1;
        int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
        t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);

        for (int x = 1; x < xSize - 1; x++, v++, vMid++) {
            t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
        }
        t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);
        int vMin = ring - 2;
        vMid -= xSize - 2;
        int vMax = v + 2;

        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
            t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
            
            for (int x = 1; x < xSize - 1; x++, vMid++) {
                t = SetQuad(triangles, t, vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
            }
            t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
        }
        int vTop = vMin - 1;
        t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
        
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
            t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);
        }
        t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);
        return t;
    }
    
    private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11) {
        /*
        v01              v11
           +------------+
           |1 \ 4      5|
           |    \       |        <<< Anatomy of a quad.
           |      \     |
           |0      2 \ 3|
           +------------+
        v00              v10
        */
        
        /*
        === Why a triangles parameter? ===
        While we gave our cube object a vertices field, we won't do so for the triangles. 
        So we have to pass it to the SetQuad method as an argument. 
        That's why the method can be static.
        
        Of course you could store the triangles at the object level as well, 
        but be aware that we'll take advantage of the parameter approach.
        */
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
        /*
        Why return the triangle index?
        For the same reason that I increment the vertex index when accessing the array. 
        This way each time you set a quad you just assign the result back to the index and you're done.
        */
    }
         
    private void OnDrawGizmos () {
        if (vertices == null) {
            return;
        }
        for (int i = 0; i < vertices.Length; i++) {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }
}
