using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEdgeTest : MonoBehaviour
{
    public Material mat;

    void OnEnable()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (mat != null) Graphics.Blit(src, dest, mat);
        else Graphics.Blit(src, dest);
    }
}
