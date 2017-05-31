using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Water : Block {

    private bool changed = true;

    override public void Initialize(int x, int z) {
        this.x = x / 15;
        this.z = z / 15;

        player = GameObject.FindGameObjectWithTag("Player");
        material = (Material)Resources.Load("Material/water", typeof(Material));
        verts = new List<Vector3>();
        tris = new List<int>();
        renderThreshold = new List<int>();
        renderThreshold.Add(0);
        numverts = 0;
        mesh = gameObject.AddComponent<MeshFilter>();
        rend = gameObject.AddComponent<MeshRenderer>();
        collid = gameObject.AddComponent<MeshCollider>();
        type = 1;
    }

    override public void renderBlock()
    {
        mesh.mesh.Clear();
        mesh.mesh = new Mesh();
        mesh.mesh.vertices = verts.ToArray();
        mesh.mesh.triangles = tris.ToArray();

        gameObject.gameObject.SetActive(true);
        rend.sharedMaterial = material;
        mesh.mesh.RecalculateNormals();
        currentTime = Time.time;
        rendered = true;
    }

    private void Update()
    {
        int playerX = (int)(player.transform.position.x / 15);
        int playerZ = (int)(player.transform.position.z / 15);
        if (Time.time - currentTime > updateTime && rendered && Enumerable.Range(playerX - 1, playerX + 1).Contains(x) && Enumerable.Range(playerZ - 1, playerZ + 1).Contains(z) && chunk != null && changed) 
        {
            updateWater();
        }
    }

    private void updateWater()
    {
        changed = false;
        for (int x = 1; x <= 15; x++)
        {
            for (int z = 1; z <= 15; z++)
            {
                for (int y = 0; y < 254; y++)
                {
                    if (chunk[x, y, z] == 0 && chunk[x, y + 1, z] == 1)
                    {
                        changed = true;
                        chunk[x, y, z] = 1;
                    }
                }
            }
        }
        if (changed)
        {
            resetValues();
            for (var posZ = 1; posZ < chunk.GetLength(0) - 1; posZ++)
            {
                for (var posY = 0; posY < chunk.GetLength(1); posY++)
                {
                    for (var posX = 1; posX < chunk.GetLength(2) - 1; posX++)
                    {
                        if (chunk[posX, posY, posZ] == 1)
                        {
                            CreateBlock(chunk, posX, posY, posZ);
                        }
                    }
                }
            }
        }
        renderBlock();
    }


}
