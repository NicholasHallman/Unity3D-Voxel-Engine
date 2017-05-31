using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bark : Block {

    private bool changed = true;
    private bool leafChange = false;
    private Leaf[] leaf;
    public Leaf leafC;

    override public void Initialize(int x, int z)
    {
        this.x = x / 15;
        this.z = z / 15;

        player = GameObject.FindGameObjectWithTag("Player");

        material = (Material)Resources.Load("Material/bark", typeof(Material));
        verts = new List<Vector3>();
        tris = new List<int>();
        renderThreshold = new List<int>();
        renderThreshold.Add(0);
        renderThreshold.Add(1);
        renderThreshold.Add(5);
        renderThreshold.Add(7);
        numverts = 0;
        mesh = gameObject.AddComponent<MeshFilter>();
        rend = gameObject.AddComponent<MeshRenderer>();
        collid = gameObject.AddComponent<MeshCollider>();
        type = 6;

        leaf = FindObjectsOfType(typeof(Leaf)) as Leaf[];
        
        foreach(Leaf item in leaf)
        {
            if(item.x == this.x && this.z == item.z)
            {
                leafC = item;
            }
        }
        
    }

    private void Update()
    {
        int playerX = (int)(player.transform.position.x / 15);
        int playerZ = (int)(player.transform.position.z / 15);
        if (Time.time - currentTime > updateTime && rendered && Enumerable.Range(playerX - 1, playerX + 1).Contains(x) && Enumerable.Range(playerZ - 1, playerZ + 1).Contains(z) && chunk != null && changed)
        {
            updateBark();
        }
    }

    private void updateBark()
    {
        changed = false;
        leafChange = false;
        for (int x = 1; x <= 15; x++)
        {
            for (int z = 1; z <= 15; z++)
            {
                for (int y = 254; y > 0; y--)
                {
                    if (chunk[x, y, z] == 6 && chunk[x, y + 1, z] == 0 && (chunk[x, y - 1, z] == 6 || chunk[x, y - 1, z] == 4))
                    {
                        for(var i = 0; i < 10; i++)
                        {
                            if(chunk[x,y - i ,x] == 4)
                            {
                                changed = true;
                                chunk[x, y + 1, z] = 6;
                                int rand = Random.Range(0, 10/2 - i/2);
                                if(rand == 0)
                                {
                                    leafChange = true;
                                    rand = Random.Range(0, 4);
                                    if (rand == 0)
                                    {
                                        chunk[x + 1, y + 1, z] = 6;
                                        //chunk[x + 2, y + 1, z] = 7;
                                        chunk[x + 1, y + 1, z + 1] = 7;
                                        chunk[x, y + 1, z - 1] = 7;
                                        chunk[x + 1, y + 2, z] = 7;
                                    }
                                    if (rand == 1)
                                    {
                                        chunk[x - 1, y + 1, z] = 6;
                                        //chunk[x - 2, y + 1, z] = 7;
                                        chunk[x - 1, y + 1, z + 1] = 7;
                                        chunk[x - 1, y + 1, z - 1] = 7;
                                        chunk[x - 1, y + 2, z] = 7;
                                    }
                                    if (rand == 2)
                                    {
                                        chunk[x, y + 1, z - 1] = 6;
                                        chunk[x + 1, y + 1, z - 1] = 7;
                                        chunk[x - 1, y + 1, z - 1] = 7;
                                        //chunk[x, y + 1, z - 2] = 7;
                                        chunk[x, y + 2, z - 1] = 7;
                                    }
                                    if (rand == 3)
                                    {
                                        chunk[x, y + 1, z + 1] = 6;
                                        chunk[x + 1, y + 1, z + 1] = 7;
                                        chunk[x - 1, y + 1, z + 1] = 7;
                                        //chunk[x, y + 1, z + 2] = 7;
                                        chunk[x, y + 2, z + 1] = 7;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        if (changed)
        {
            resetValues();
            leafC.resetValues();
            
            for (var posZ = 1; posZ < chunk.GetLength(0) - 1; posZ++)
            {
                for (var posY = 0; posY < chunk.GetLength(1); posY++)
                {
                    for (var posX = 1; posX < chunk.GetLength(2) - 1; posX++)
                    {
                        if (chunk[posX, posY, posZ] == 6)
                        {
                            CreateBlock(chunk, posX, posY, posZ);

                        }
                        if (chunk[posX, posY, posZ] == 7 && leafChange)
                        {
                            leafC.CreateBlock(chunk, posX, posY, posZ);
                        }
                    }
                }
            }
            renderBlock();
            leafC.renderBlock();

        }
       
    }

}
