using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Block {

    // Use this for initialization
    override public void Initialize(int x, int z) {
        material = (Material)Resources.Load("Material/stone", typeof(Material));
        verts = new List<Vector3>();
        tris = new List<int>();
        renderThreshold = new List<int>();
        renderThreshold.Add(0);
        renderThreshold.Add(1);
        numverts = 0;
        mesh = gameObject.AddComponent<MeshFilter>();
        rend = gameObject.AddComponent<MeshRenderer>();
        collid = gameObject.AddComponent<MeshCollider>();
        type = 2;
    }
}
