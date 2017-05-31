using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Chunk : MonoBehaviour {

    private int   width = 15;
    private int  length = 15;
    private int  heigth = 255;
    private float scale = 0.01f;
    private int[,,] chunk;
    private float thresh = 80;
    public bool filled = false;
    public Block[] types; 
    private int numOfTypes = 8;
    private int waterLevel = 85;

    private const int airId = 0;
    private const int watId = 1;
    private const int stnId = 2;
    private const int dirId = 3;
    private const int graId = 4;
    private const int tgrId = 5;

    public void GenChunk(int x , int z)
    {
        x = x * 15;
        z = z * 15;

        chunk = new int[width + 2, heigth, length + 2];

        InitBlockTypes(x, z);

        generateTypes(x, z);

        for (var posZ = 1; posZ < chunk.GetLength(0) - 1; posZ++)
        {
            for (var posY = 0; posY < chunk.GetLength(1); posY++)
            {
                for (var posX = 1; posX < chunk.GetLength(2) - 1; posX++)
                {
                    GenBlock(posX, posY, posZ, x, z);
                }
            }
        }
        filled = true;
        fuseMeshes();
    }

    private void fuseMeshes()
    {

        for(var i = 1; i < numOfTypes; i++)
        {
            types[i].renderBlock();
        }

    }
    /*
    public void DelChunk()
    {
        model.gameObject.SetActive(false);
        filled = false;
    }
    */
    private void GenBlock(int posX, int posY, int posZ, int x, int z)
    {
        if (chunk[posX, posY, posZ] != 0)
        {
            types[chunk[posX, posY, posZ]].CreateBlock(chunk, posX, posY, posZ);
        }
    }

    private float genNoise(int x, int z, int posX, int posY, int posZ)
    {
        double total = 0;
        int frequency = 1;
        double amplitude = 1;
        double maxValue = 0;
        double persistence = 4;
        double octaves = 2;
        for(int i = 0; i< octaves; i++)
        {
            total += Simplex.Noise.CalcPixel3D((x + posX) * frequency,  (posY * frequency), (z + posZ) * frequency, scale) / amplitude;
            maxValue += amplitude;

            amplitude *= persistence;
            frequency *= 2;
        }
        return (float) (total/maxValue);

    }

    private void genType(int x, int z, int i, int j, int k, int groundLvl)
    {
        if (j < 3)
        {
            chunk[i + 1, j, k + 1] = stnId;
        }
        else if (j <= groundLvl)
        {
            float value = 255 - (genNoise(x, z, i, j, k) * 4);
            if (value > thresh)
            {
                chunk[i + 1, j, k + 1] = stnId;
            }
            else
            {
                chunk[i + 1, j, k + 1] = airId;
            }

        }

        if (j == groundLvl)
        {
            if(chunk[i + 1, j, k + 1] == stnId)
            {
                chunk[i + 1, j, k + 1] = graId;
            }   
        }
        else if (j > groundLvl - 4 && j < groundLvl)
        {
            if (chunk[i + 1, j, k + 1] == stnId)
            {
                chunk[i + 1, j, k + 1] = dirId;
            }
            
        }
        else if(j > groundLvl && j < waterLevel)
        {
            chunk[i + 1, j, k + 1] = watId;
        }
        else if (j == groundLvl + 1)
        {
            if (Random.Range(0, 20) == 5 && chunk[i + 1, j - 1, k + 1] == graId)
            {
                int rng = (Random.Range(0, 10));
                if (rng < 9) {
                    chunk[i + 1, j, k + 1] = tgrId;
                }
                else
                {
                    if ( i > -1 && k > -1 && chunk[i, j, k] != 6 && chunk[i + 1, j, k] != 6 && chunk[i, j, k + 1] != 6)
                    {
                        chunk[i + 1, j, k + 1] = 6;
                    }
                }
            }
        }
        else if(j > groundLvl)
        {
            chunk[i + 1, j, k + 1] = airId;
        }
        
    }
    private void InitBlockTypes(int x, int z)
    {
        types = new Block[numOfTypes];
        types[0] = new GameObject().AddComponent<Air>();
        types[1] = new GameObject().AddComponent<Water>();
        types[2] = new GameObject().AddComponent<Stone>();
        types[3] = new GameObject().AddComponent<Dirt>();
        types[4] = new GameObject().AddComponent<Grass>();
        types[5] = new GameObject().AddComponent<GrassTopper>();
        types[6] = new GameObject().AddComponent<Bark>();
        var leaf = new GameObject().AddComponent<Leaf>();
        types[7] = leaf;

        types[0].Initialize(x,z);
        types[1].Initialize(x,z);
        types[2].Initialize(x,z);
        types[3].Initialize(x,z);
        types[4].Initialize(x,z);
        types[5].Initialize(x, z);
        types[7].Initialize(x, z);
        types[6].Initialize(x, z);


        types[0].transform.position = new Vector3(x, 0, z);
        types[1].transform.position = new Vector3(x, 0, z);
        types[2].transform.position = new Vector3(x, 0, z);
        types[3].transform.position = new Vector3(x, 0, z);
        types[4].transform.position = new Vector3(x, 0, z);
        types[5].transform.position = new Vector3(x, 0, z);
        types[6].transform.position = new Vector3(x, 0, z);
        types[7].transform.position = new Vector3(x, 0, z);

        leaf.chunk = chunk;

    }

    private void generateTypes(int x, int z)
    {
        for (var i = -1; i <= width; i++)
        {
            for (var k = -1; k <= length; k++)
            {
                float ground = Noise.GenerateNoise(x + i, z + k, 2);
                ground = (Mathf.Pow(ground, 3) / 4000) + 80;
                chunk[i + 1, (int)ground, k + 1] = 0;

                for (var j = 0; j < heigth; j++)
                {
                    genType(x, z, i, j, k, (int)ground);

                }
            }
        }
    }
}
