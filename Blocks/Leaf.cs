using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Leaf : Block
{

    private bool changed = true;
    private int[,,] renderedA;
    new public int[,,] chunk
    {
        get;
        set;
    }

    override public void Initialize(int x, int z)
    {
        this.x = x = x / 15;
        this.z = z = z / 15;

        renderedA = new int[16, 256, 16];

        player = GameObject.FindGameObjectWithTag("Player");
        material = (Material)Resources.Load("Material/leaf", typeof(Material));
        verts = new List<Vector3>();
        tris = new List<int>();
        renderThreshold = new List<int>();
        renderThreshold.Add(0);
        renderThreshold.Add(1);
        renderThreshold.Add(5);
        numverts = 0;
        mesh = gameObject.AddComponent<MeshFilter>();
        rend = gameObject.AddComponent<MeshRenderer>();
        type = 7;
        currentTime = Time.time;
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

}