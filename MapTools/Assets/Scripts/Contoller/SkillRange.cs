using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
    Mesh mMesh;
    MeshFilter mFilter;

    // Start is called before the first frame update
    void Start()
    {
        mFilter = GetComponent<MeshFilter>();
    }

    public void SetMesh(Mesh mesh)
    {
        mMesh = mesh;
    }

    public void DrawRange()
    {
        mFilter.mesh = mMesh;
    }

    public void StopDrawing()
    {
        mFilter.mesh = null;
    }
}
