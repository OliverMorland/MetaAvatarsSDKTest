using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;

public class GGMetaAvatarEntity : OvrAvatarEntity
{
    [SerializeField] int m_avatarToUseInZipFolder = 2;

    private void Start()
    {
        if (IsLocal)
        {
            string[] zipPaths = new string[] { m_avatarToUseInZipFolder + "_rift.glb" };
            LoadAssetsFromZipSource(zipPaths);
        }
        else
        {
            string[] zipPaths = new string[] { m_avatarToUseInZipFolder + 1 + "_rift.glb" };
            LoadAssetsFromZipSource(zipPaths);
        }
    }

    
}
