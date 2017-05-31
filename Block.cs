using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour {

    protected bool rendered = false;
    protected List<Vector3> verts;
    protected List<int> tris;
    protected int numverts;
    protected MeshFilter mesh;
    protected MeshRenderer rend;
    protected MeshCollider collid;
    protected Material material;
    protected float updateTime = .5f;
    protected float currentTime;
    public int type;
    protected List<int> renderThreshold;
    protected GameObject player;
    public int[,,] chunk;
    public int x;
    public int z;

    public virtual void CreateBlock(int[,,] chunk, int posX, int posY, int posZ)
    {
        this.chunk = chunk;
        if (renderThreshold.Contains(chunk[posX - 1, posY, posZ]))
        {
            Vector3[] vertices = new Vector3[4];
            vertices[0] = new Vector3(-0.5f + posX, -0.5f + posY, -0.5f + posZ);
            vertices[1] = new Vector3(-0.5f + posX, -0.5f + posY, 0.5f + posZ);
            vertices[2] = new Vector3(-0.5f + posX, 0.5f + posY, -0.5f + posZ);
            vertices[3] = new Vector3(-0.5f + posX, 0.5f + posY, 0.5f + posZ);
            verts.AddRange(vertices);
            numverts += 4;
            tris.AddRange(new int[6] { numverts - 2, numverts - 4, numverts - 3, numverts - 2, numverts - 3, numverts - 1 });

        }
        if (renderThreshold.Contains(chunk[posX + 1, posY, posZ])) 
        {
            Vector3[] vertices = new Vector3[4];
            vertices[0] = new Vector3(0.5f + posX, -0.5f + posY, -0.5f + posZ);
            vertices[1] = new Vector3(0.5f + posX, -0.5f + posY, 0.5f + posZ);
            vertices[2] = new Vector3(0.5f + posX, 0.5f + posY, -0.5f + posZ);
            vertices[3] = new Vector3(0.5f + posX, 0.5f + posY, 0.5f + posZ);
            verts.AddRange(vertices);
            numverts += 4;
            tris.AddRange(new int[6] { numverts - 2, numverts - 3, numverts - 4, numverts - 2, numverts - 1, numverts - 3 });
            
        }
        if (posY > 0 && renderThreshold.Contains(chunk[posX, posY - 1, posZ]))
        {
            Vector3[] vertices = new Vector3[4];
            vertices[0] = new Vector3(0.5f + posX, -0.5f + posY, -0.5f + posZ);
            vertices[1] = new Vector3(0.5f + posX, -0.5f + posY, 0.5f + posZ);
            vertices[2] = new Vector3(-0.5f + posX, -0.5f + posY, -0.5f + posZ);
            vertices[3] = new Vector3(-0.5f + posX, -0.5f + posY, 0.5f + posZ);

            verts.AddRange(vertices);
            numverts += 4;
            tris.AddRange(new int[6] { numverts - 2, numverts - 4, numverts - 3, numverts - 2, numverts - 3, numverts - 1 });
            

        }
        if (posY < 254 && renderThreshold.Contains(chunk[posX, posY + 1, posZ]))
        {
            Vector3[] vertices = new Vector3[4];
            vertices = new Vector3[4];
            vertices[0] = new Vector3(0.5f + posX, 0.5f + posY, -0.5f + posZ);
            vertices[1] = new Vector3(0.5f + posX, 0.5f + posY, 0.5f + posZ);
            vertices[2] = new Vector3(-0.5f + posX, 0.5f + posY, -0.5f + posZ);
            vertices[3] = new Vector3(-0.5f + posX, 0.5f + posY, 0.5f + posZ);

            verts.AddRange(vertices);
            numverts += 4;
            tris.AddRange(new int[6] { numverts - 2, numverts - 3, numverts - 4, numverts - 2, numverts - 1, numverts - 3 });
            

        }
        if (renderThreshold.Contains(chunk[posX, posY, posZ - 1]))
        {
            Vector3[] vertices = new Vector3[4];
            vertices = new Vector3[4];
            vertices[0] = new Vector3(0.5f + posX, -0.5f + posY, -0.5f + posZ);
            vertices[1] = new Vector3(-0.5f + posX, -0.5f + posY, -0.5f + posZ);
            vertices[2] = new Vector3(0.5f + posX, 0.5f + posY, -0.5f + posZ);
            vertices[3] = new Vector3(-0.5f + posX, 0.5f + posY, -0.5f + posZ);


            verts.AddRange(vertices);
            numverts += 4;
            tris.AddRange(new int[6] { numverts - 2, numverts - 4, numverts - 3, numverts - 2, numverts - 3, numverts - 1 });
            

        }
        if (renderThreshold.Contains(chunk[posX, posY, posZ + 1]))
        {
            Vector3[] vertices = new Vector3[4];
            vertices = new Vector3[4];
            vertices[0] = new Vector3(0.5f + posX, -0.5f + posY, 0.5f + posZ);
            vertices[1] = new Vector3(-0.5f + posX, -0.5f + posY, 0.5f + posZ);
            vertices[2] = new Vector3(0.5f + posX, 0.5f + posY, 0.5f + posZ);
            vertices[3] = new Vector3(-0.5f + posX, 0.5f + posY, 0.5f + posZ);

            verts.AddRange(vertices);
            numverts += 4;
            tris.AddRange(new int[6] { numverts - 2, numverts - 3, numverts - 4, numverts - 2, numverts - 1, numverts - 3 });
            

        }
        if (posY == 254)
        {
            Vector3[] vertices = new Vector3[4];
            vertices = new Vector3[4];
            vertices[0] = new Vector3(0.5f + posX, 0.5f + posY, -0.5f + posZ);
            vertices[1] = new Vector3(0.5f + posX, 0.5f + posY, 0.5f + posZ);
            vertices[2] = new Vector3(-0.5f + posX, 0.5f + posY, -0.5f + posZ);
            vertices[3] = new Vector3(-0.5f + posX, 0.5f + posY, 0.5f + posZ);

            verts.AddRange(vertices);
            numverts += 4;
            tris.AddRange(new int[6] { numverts - 2, numverts - 4, numverts - 3, numverts - 2, numverts - 3, numverts - 1 });
            
        }
    }

    public virtual void renderBlock()
    {
        mesh.mesh = new Mesh();
        mesh.mesh.vertices = verts.ToArray();
        mesh.mesh.triangles = tris.ToArray();

        gameObject.gameObject.SetActive(true);
        rend.sharedMaterial = material;
        mesh.mesh.RecalculateNormals();
        collid.sharedMesh = mesh.sharedMesh;

        currentTime = Time.time;
        rendered = true;
    }
    public virtual void Initialize(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
    public void resetValues()
    {
        verts.Clear();
        tris.Clear();
        numverts = 0;
    }

}
