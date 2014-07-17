using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
    public GameObject worldGO;
    private PolygonGenerator world;

    public int chunkSize;
    public int chunkX;
    public int chunkY;

    public bool update = false;

    public List<Vector3> newVertices = new List<Vector3>();
    public List<int> newTriangles = new List<int>();
    public List<Vector2> newUV = new List<Vector2>();

    public byte[,] blocks;

    public List<Vector3> colVertices = new List<Vector3>();
    public List<int> colTriangles = new List<int>();

    private Mesh mesh;

    private float tUnit = 0.5f;
    private Vector2 tStone = new Vector2(0, 1);
    private Vector2 tGrass = new Vector2(1, 1);

    private int colCount;

    private MeshCollider col;

    private int squareCount;
    // Use this for initialization
    void Start()
    {
        world = worldGO.GetComponent<PolygonGenerator>();

        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        BuildMesh();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            BuildMesh();
            UpdateMesh();
            update = false;
        }
    }

    void UpdateMesh()
    {
        Mesh newMesh = new Mesh();
        newMesh.vertices = colVertices.ToArray();
        newMesh.triangles = colTriangles.ToArray();
        col.sharedMesh = newMesh;

        colVertices.Clear();
        colTriangles.Clear();
        colCount = 0;

        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        squareCount = 0;
        newVertices.Clear();
        newTriangles.Clear();
        newUV.Clear();
    }

    void GenSquare(int x, int y, Vector2 texture)
    {
        newVertices.Add(new Vector3(x, y, 0));
        newVertices.Add(new Vector3(x + 1, y, 0));
        newVertices.Add(new Vector3(x + 1, y - 1, 0));
        newVertices.Add(new Vector3(x, y - 1, 0));

        newTriangles.Add(squareCount * 4);
        newTriangles.Add((squareCount * 4) + 1);
        newTriangles.Add((squareCount * 4) + 3);
        newTriangles.Add((squareCount * 4) + 1);
        newTriangles.Add((squareCount * 4) + 2);
        newTriangles.Add((squareCount * 4) + 3);

        newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y + tUnit));
        newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
        newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y));
        newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y));

        squareCount++;
    }

    void BuildMesh()
    {
        for (int px = 0; px < chunkSize; px++)
        {
            for (int py = 0; py < chunkSize; py++)
            {
                if (Block(px, py) != 0)
                {
                    GenCollider(px, py);
                    if (Block(px, py) == 1)
                    {
                        GenSquare(px, py, tStone);
                    }
                    else if (Block(px, py) == 2)
                    {
                        GenSquare(px, py, tGrass);
                    }
                }
            }
        }

    }

    void GenCollider(int x, int y)
    {

        if (Block(x, y + 1) == 0)
        {
            colVertices.Add(new Vector3(x, y, 1));
            colVertices.Add(new Vector3(x + 1, y, 1));
            colVertices.Add(new Vector3(x + 1, y, 0));
            colVertices.Add(new Vector3(x, y, 0));

            ColliderTriangles();

            colCount++;
        }
        if (Block(x, y - 1) == 0)
        {
            colVertices.Add(new Vector3(x, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y - 1, 1));
            colVertices.Add(new Vector3(x, y - 1, 1));

            ColliderTriangles();

            colCount++;
        }

        if (Block(x - 1, y) == 0)
        {
            colVertices.Add(new Vector3(x, y - 1, 1));
            colVertices.Add(new Vector3(x, y, 1));
            colVertices.Add(new Vector3(x, y, 0));
            colVertices.Add(new Vector3(x, y - 1, 0));

            ColliderTriangles();

            colCount++;
        }
        if (Block(x + 1, y) == 0)
        {
            colVertices.Add(new Vector3(x + 1, y, 1));
            colVertices.Add(new Vector3(x + 1, y - 1, 1));
            colVertices.Add(new Vector3(x + 1, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y, 0));

            ColliderTriangles();

            colCount++;
        }
    }

    void ColliderTriangles()
    {
        colTriangles.Add(colCount * 4);
        colTriangles.Add((colCount * 4) + 1);
        colTriangles.Add((colCount * 4) + 3);
        colTriangles.Add((colCount * 4) + 1);
        colTriangles.Add((colCount * 4) + 2);
        colTriangles.Add((colCount * 4) + 3);
    }

    byte Block(int x, int y)
    {
       return world.Block(x + chunkX, y + chunkY);
    }

}
