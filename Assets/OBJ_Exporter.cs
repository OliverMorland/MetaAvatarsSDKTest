using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_Exporter : MonoBehaviour
{
    [SerializeField] Mesh m_mesh;

    [ContextMenu("Export to OBJ")]
    void ExportToOBJ()
    {
        if (m_mesh == null)
        {
            Debug.LogError("No mesh selected, please connect one.");
            return;
        }
    }
}
