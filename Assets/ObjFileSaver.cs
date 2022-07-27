using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ObjFileSaver : MonoBehaviour
{
    public string ObjPath;
    public bool SaveObj = false;
    public bool flipNormalX = false;

	// Use this for initialization
	void Start ()
    {
        ObjPath = "Avatar.obj";

    }
	
    void SaveObjFile (SkinnedMeshRenderer mr, MeshFilter mf)
    {
        string filePath = Application.dataPath + "/" + ObjPath;
        Debug.Log("Saving to \"" + filePath + "\"");
        string mtllibPath = Application.dataPath + "/" + Path.GetDirectoryName(ObjPath) + "/mesh.mtl";

        List<string> mtlNames = new List<string>();
        using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter(mtllibPath))
        {
            for (int i = 0; i < mr.materials.Length; i++)
            {
                string mtlName = mr.materials[i].name;
                mtlNames.Add(mtlName);
                file.WriteLine("newmtl " + mtlName);
                file.WriteLine("Kd 0.0000 1.0000 0.0000");
                file.WriteLine("illum 0");
            }
            file.Close();
        }
        Mesh m = mf.mesh;
        Debug.Log("Vertice count " + m.vertices);
        

        using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter(filePath))
        {
            file.WriteLine("# " + ObjPath);
            file.WriteLine("mtllib mesh.mtl");

            Vector3[] vs = m.vertices;
            Vector3[] ns = m.normals;
            Vector2[] uvs = m.uv;

            file.WriteLine("# {0} v's", vs.Length);
            for (int i = 0; i < vs.Length; i++)
            {
                file.WriteLine("v " + -vs[i].x + " " + vs[i].y + " " + vs[i].z);
            }
            file.WriteLine("# {0} vt's", uvs.Length);
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i].x = 1 - uvs[i].x;
                file.WriteLine("vt " + uvs[i].x + " " + uvs[i].y);
            }
            file.WriteLine("# {0} vn's", ns.Length);
            for (int i = 0; i < ns.Length; i++)
            {
                if (flipNormalX)
                    file.WriteLine("vn " + -ns[i].x + " " + -ns[i].y + " " + -ns[i].z);
                else
                    file.WriteLine("vn " + ns[i].x + " " + ns[i].y + " " + ns[i].z);
            }
            for (int subMeshIndex = 0; subMeshIndex < m.subMeshCount; subMeshIndex++)
            {
                file.WriteLine("usemtl " + mtlNames[subMeshIndex]);
                int[] fs = m.GetTriangles(subMeshIndex) ;
                for (int i = 0; i < fs.Length; i += 3)
                {
                    // objs are Left Hand world
                    int t0 = fs[i + 0];
                    int t1 = fs[i + 1];
                    int t2 = fs[i + 2];
                    t0++;
                    t1++;
                    t2++;
                    file.WriteLine("f " + t0 + "/" + t0 + "/" + t0 + " " + t1 + "/" + t1 + "/" + t1 + " " + t2 + "/" + t2 + "/" + t2);
                }
            }
            file.Close();
        }

    }

    // Update is called once per frame
    void Update ()
    {
	    if (SaveObj)
        {
            SaveObjFile(GetComponent<SkinnedMeshRenderer>(), GetComponent<MeshFilter>());
            SaveObj = false;
        }
	}
}
