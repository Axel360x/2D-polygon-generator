using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolygonGenerator : MonoBehaviour {

    public int worldX = 128;
    public int worldY = 128;
    public int chunkSize = 64;

    public byte[,] data;

    public GameObject chunk;
    public Chunk[,] chunks;

    private float tUnit = 0.5f;
    private Vector2 tStone = new Vector2(0, 1);
    private Vector2 tGrass = new Vector2(1, 1);

    private int colCount;

    private int squareCount;
	// Use this for initialization
	void Start () {

        GenTerrain();
	}
	
	// Update is called once per frame
	void Update () {
        LoadChunks();
	}

 

    void GenTerrain()
    {
        data = new byte[worldX, worldX];
		System.Random rnd = new System.Random();
		int Irnd = rnd.Next(0, 256);
        for (int px = 0; px < data.GetLength(0); px++)
        {
            int stone = Noise(px, Irnd, 80, 15, 1);
            stone += Noise(px, 0, 50, 30, 1);
            stone += Noise(px, 0, 10, 10, 1);
            stone += 750;

            int dirt = Noise(px, Irnd, 100, 35, 1);
            dirt += Noise(px, 100, 50, 30, 1);
            dirt += 20;


            for (int py = 0; py < data.GetLength(1); py++)
            {
                if (py < stone)
                {
                    data[px, py] = 1;
                    if (Noise(px, py, 12, 16, 1) > 10)
                    {
                        data[px, py] = 2;
                    }
                    if (Noise(px, py * 2, 64, 16, 1.1f) > 10)
                    {
                        data[px, py] = 0;
                    }
                }
                else if(py<dirt + stone)
                {
                    data[px, py] = 2;
					if (Noise(px, py * 2, 24, 12, 1.1f) > 10)
                    {
                        data[px, py] = 0;
                    }
                }
                //if (px == 5)
                //{
                //    blocks[px, py] = 0;
                //}
            }
        }

        chunks = new Chunk[Mathf.FloorToInt(worldX / chunkSize), Mathf.FloorToInt(worldY / chunkSize)];

    }

    public void LoadChunks()
    {


        for (int x = 0; x < chunks.GetLength(0); x++)
        {


                //float dist = Vector2.Distance(new Vector2(x * chunkSize, z * chunkSize), new Vector2(playerPos.x, playerPos.z));

                //if (dist < distToLoad)
                //{
                //    if (chunks[x, 0] == null)
                //    {
                //        GenColumn(x);
                //    }
                //}
                //else if (dist > distToUnload)
                //{
                //    if (chunks[x, 0] != null)
                //    {

                //        UnloadColumn(x);
                //    }
                //}
            if (chunks[x, 0] == null)
                GenColumn(x);
            
        }

    }


    public void GenColumn(int x)
    {
        for (int y = 0; y < chunks.GetLength(1); y++)
        {
            GameObject newChunk = Instantiate(chunk, new Vector3(x * chunkSize - 0.5f, y * chunkSize + 0.5f, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
            chunks[x, y] = newChunk.GetComponent<Chunk>();
            chunks[x, y].worldGO = gameObject;
            chunks[x, y].chunkSize = chunkSize;
            chunks[x, y].chunkX = x * chunkSize;
            chunks[x, y].chunkY = y * chunkSize;
        }
    }

    public void UnloadColumn(int x)
    {
        for (int y = 0; y < chunks.GetLength(1); y++)
        {
            UnityEngine.Object.Destroy(chunks[x, y].gameObject);

        }
    }

    public byte Block(int x, int y)
    {
        if (x >= worldX || x < 0 || y >= worldY || y < 0)
        {
            return (byte)1;
        }
        return data[x, y];
    }

    int Noise(int x, int y, float scale, float mag, float exp)
    {
        return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * mag), (exp)));
    }
}
