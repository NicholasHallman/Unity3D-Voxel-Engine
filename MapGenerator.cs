using System.Collections;
using System.Collections.Generic;using UnityEngine;

public class MapGenerator : MonoBehaviour {

    // Use this for initialization
    Vector3 prevChunk = new Vector3(160,0,160);
    int size;
    Chunk[,] world;
     

	void Start () {

        GameObject player = GameObject.FindWithTag("Player");
        Vector3 currPos = player.transform.position;
        Vector3 chunkPos = new Vector3(((int)currPos.x) / 15, ((int)currPos.y) / 15, ((int)currPos.z) / 15);

        size = 8;

        world = new Chunk[256,256];
        int seed = Simplex.Noise.Seed;
        
        for(var x = (size * -1) + (int)chunkPos.x; x < size + (int)chunkPos.x; x++)
        {
            for(var z = (size * -1) + (int) chunkPos.z; z < size + (int)chunkPos.z; z++)
            {
                var chunk = gameObject.AddComponent<Chunk>();
                chunk.GenChunk(x,z);
                world[x + size, z + size] = chunk;
            }
        }

	}
    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 currPos = player.transform.position;
        Vector3 chunkPos = new Vector3(((int) currPos.x) / 15, ((int)currPos.y) / 15, ((int)currPos.z) / 15);
        if(chunkPos.x != prevChunk.x || chunkPos.z != prevChunk.z)
        {
           //StartCoroutine(chunkUpdate(chunkPos));
        }

    }
    IEnumerator chunkUpdate(Vector3 chunkPos)
    {
        prevChunk = chunkPos;
        int x = 0;
        int z = 0;
        int dx = 0;
        int dz = -1;
        int t = 16;
        for (var i = 0; i <= 128; i++)
        {
            if ((-16 / 2 <= x && x <= 16 / 2) && (-16 / 2 <= z && z <= 16 / 2))
            {
                int relx = x + (int)chunkPos.x;
                int relz = z + (int)chunkPos.z;
                if (world[relx, relz] == null)
                {
                    var chunk = gameObject.AddComponent<Chunk>();
                    chunk.GenChunk(relx, relz);
                    world[relx, relz] = chunk;
                    yield return null;
                }
                else if (!world[relx, relz].filled)
                {
                    Debug.Log("Redraw");
                    world[relx, relz].GenChunk(relx, relz);
                    yield return null;
                }

            }
            if ((x == z) || (x < 0) && (x == -z) || ((x > 0) && (x == 1 - z)))
            {
                t = dx;
                dx = -dz;
                dz = t;
            }
            x += dx;
            z += dz;

        }
    }

}
