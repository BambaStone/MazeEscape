using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EdgeCommandBuffer : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask outlineLayer;
    public Material edgeMaterial;
    public Shader maskShader;
    public bool isShowing = true;
    public List<Renderer> renderers1;
    public float LinePower=1f;
    private CommandBuffer _cb;
    private Camera _cam;

    void OnEnable()
    {
        _cam = GetComponent<Camera>();
        _cam.depthTextureMode |= DepthTextureMode.DepthNormals;

        if (_cb == null)
        {
            _cb = new CommandBuffer();
            _cb.name = "Selective Outline Buffer";
        }
        _cam.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, _cb);
        _cam.AddCommandBuffer(CameraEvent.AfterForwardOpaque, _cb);
    }

    void OnDisable()
    {
        if (_cb != null && _cam != null)
            _cam.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, _cb);
    }

    void OnPreRender()
    {
        
        if (!isShowing || edgeMaterial == null || maskShader == null)
        {
            _cb.Clear();
            return;
        }

        _cb.Clear();

        int maskID = Shader.PropertyToID("_TempOutlineTexture");
        int screenID = Shader.PropertyToID("_ScreenCopyTexture");

        _cb.GetTemporaryRT(maskID, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
        _cb.GetTemporaryRT(screenID, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.Default);

        _cb.Blit(BuiltinRenderTextureType.CameraTarget, screenID);

        _cb.SetRenderTarget(maskID);
        _cb.ClearRenderTarget(true, true, Color.clear);
        _cb.SetViewProjectionMatrices(_cam.worldToCameraMatrix, _cam.projectionMatrix);

        if (renderers1 != null)
        {
            for (int i = 0; i < renderers1.Count; i++)
            {
                _cb.DrawRenderer(renderers1[i], new Material(maskShader));
                if(renderers1[i].gameObject.CompareTag("Enemy"))
                {
                    renderers1[i].gameObject.GetComponent<Outline>().OutlineColor = new Color(1,1,1,LinePower);
                }
            }
        }
        
        _cb.SetGlobalTexture("_TempOutlineTexture", maskID);
        _cb.SetGlobalTexture("_ScreenCopyTexture", screenID);
        edgeMaterial.SetColor("_EdgeColor", new Color(LinePower, LinePower, LinePower, LinePower));
        _cb.Blit(screenID, BuiltinRenderTextureType.CameraTarget, edgeMaterial);

        _cb.ReleaseTemporaryRT(maskID);
        _cb.ReleaseTemporaryRT(screenID);
    }

}