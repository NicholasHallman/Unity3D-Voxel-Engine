using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTopper : Block {

    public override void Initialize(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override void CreateBlock(int[,,] chunk, int posX, int posY, int posZ)
    {
        GameObject grass = new GameObject();
        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            grass = Instantiate(Resources.Load("Environment/Grass", typeof(GameObject))) as GameObject;
        }
        else if(rand == 1)
        {
            grass = Instantiate(Resources.Load("Environment/Flower1", typeof(GameObject))) as GameObject;
        }
        else if(rand == 2)
        {
            grass = Instantiate(Resources.Load("Environment/Flower2", typeof(GameObject))) as GameObject;
        }
        grass.transform.parent = gameObject.transform;
        grass.transform.position = new Vector3(posX + x, posY - 0.5f, posZ + z );

        Vector3 euler = transform.eulerAngles;
        euler.y = 0 + (90 * Random.Range(0, 4));
        grass.transform.eulerAngles = euler;

        grass.transform.localScale = grass.transform.localScale / 8;
    }

    public override void renderBlock()
    {

    }
}
