using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.XR.XRDisplaySubsystem;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class RenderToTexture : MonoBehaviour
{
    [HideInInspector]
    public RenderTexture colorTex;
    [HideInInspector]
    public RenderTexture colorTex1;

    [HideInInspector]
    public RenderTexture depthTex;
    public GameObject forviewmatr;
    private bool once=true;
    int count = 0;
    Camera cam;
    Shader PostprocessShader;
    Material PostprocessMaterial;
    float elapsed = 0;
    List<XRDisplaySubsystem> displays = new List<XRDisplaySubsystem>();
    public RawImage img;
    public TMP_Text textinfo;

    void Start()
    {
        cam = GetComponent<Camera>();
        
        StartSubsystem();
        colorTex = new RenderTexture(1067, 269, 32);
        colorTex.autoGenerateMips = false;
        colorTex.anisoLevel = 0;
        colorTex.name = "ColorTexture";
        colorTex.useDynamicScale = true;
/*        colorTex.dimension = TextureDimension.Tex2DArray;
        colorTex.enableRandomWrite = true;
        colorTex.volumeDepth = 2;*/ 
        colorTex.vrUsage = VRTextureUsage.TwoEyes;
        depthTex = new RenderTexture(1067,269,32);
        depthTex.autoGenerateMips = false;
        depthTex.anisoLevel = 0;
        depthTex.name = "DepthTexture";
        depthTex.useDynamicScale = true;
/*        depthTex.dimension = TextureDimension.Tex2DArray;
        depthTex.enableRandomWrite = true;
        depthTex.volumeDepth = 2;*/
      //  depthTex.vrUsage = VRTextureUsage.TwoEyes;
      //  depthTex.width *= 2;

        SubsystemManager.GetSubsystems(displays);
        Debug.Log(UnityEngine.Application.persistentDataPath);
        displays[0].textureLayout = TextureLayout.SingleTexture2D;
/*        Debug.Log(Screen.height);
        Debug.Log(Screen.width);*/

        colorTex.Create();
        depthTex.Create();

       //  cam.depthTextureMode |= DepthTextureMode.Depth;
        //cam.targetTexture = colorTex;
        //cam.SetTargetBuffers(colorTex.colorBuffer, depthTex.depthBuffer);

        PostprocessShader = Shader.Find("GR/ExtractDepth");

        if (PostprocessShader == null)
            PostprocessShader = Resources.Load<Shader>("ExtractDepth.shader");

        if (PostprocessShader)
        {
            PostprocessMaterial = new Material(PostprocessShader);
            PostprocessMaterial.enableInstancing = false;
            Debug.Log("[GR]: Found Shader for Depth Post Processing!");
        }
        else
        {
            Debug.LogError("[GR]: Did not find Shader for Depth Post Processing!");
        }



        StartCoroutine(createtexture());
    }

    void Update()
    {


        //cam.Render();


    }
    IEnumerator createtexture()
    {
        yield return new WaitForEndOfFrame();
        if (once)
        {

            //   setMatrix();
            once = false;
        }
        elapsed += Time.deltaTime;
      //  Graphics.CopyTexture(displays[0].GetRenderTextureForRenderPass(0),0,0,colorTex,0,0);
         colorTex = displays[0].GetRenderTextureForRenderPass(0);
      //  depthTex = displays[0].GetRenderTextureForRenderPass(0);
        //depthTex = displays[0].GetRenderTextureForRenderPass(1);
        // Graphics.Blit(displays[0].GetRenderTextureForRenderPass(0), colorTex, PostprocessMaterial);
         Graphics.Blit(colorTex, depthTex, PostprocessMaterial);
        if (elapsed > 1)
        {
            elapsed = elapsed % 1f;
            //     Debug.Log(displays[0].GetRenderPassCount());
            /*            Debug.Log(displays[0].GetRenderTextureForRenderPass(0).width);
                        Debug.Log(displays[0].GetRenderTextureForRenderPass(0).height);
                        Debug.Log(displays[0].GetRenderTextureForRenderPass(0).volumeDepth);
                        Debug.Log(colorTex.width);
                        Debug.Log(colorTex.height);
                        Debug.Log(colorTex.volumeDepth);*/
            //  Debug.Log(displays[0].GetRenderTextureForRenderPass(0).useDynamicScale);
            //     Debug.Log(displays[0].GetRenderTextureForRenderPass(1).width);
            //Debug.Log(cam.GetStereoViewMatrix(Camera.StereoscopicEye.Left));
            //Debug.Log(cam.GetStereoViewMatrix(Camera.StereoscopicEye.Right));

            /* { 1.00000    0.00000 0.00000 1.00000
0.00000 1.00000 0.00000 -1.00000
0.00000 0.00000 -1.00000    -13.91000       // for left eye, alternating between this and identity like
0.00000 0.00000 0.00000 1.00000};*/

            /* 1.00000 0.00000 0.00000 - 1.00000
 0.00000 1.00000 0.00000 - 1.00000
 0.00000 0.00000 - 1.00000 - 13.91000
 0.00000 0.00000 0.00000 1.00000*/

            /*          Matrix4x4 viewleft = CreateViewMatrix(forviewmatr.transform.position, forviewmatr.transform.rotation);
                      Debug.Log(viewleft);
                      cam.SetStereoViewMatrix(Camera.StereoscopicEye.Left, viewleft);*/
            //    Debug.Log(cam.GetStereoViewMatrix(Camera.StereoscopicEye.Left));
            //   Debug.Log(cam.GetStereoViewMatrix(Camera.StereoscopicEye.Right));
            //  cam.SetStereoViewMatrix(Camera.StereoscopicEye.Left,Matrix4x4.identity);
            //  Debug.Log(cam.GetStereoViewMatrix(Camera.StereoscopicEye.Left));
            /*                        XRRenderPass xrp1,xrp2;
                                    displays[0].GetRenderPass(0, out xrp1);
                                    displays[0].GetRenderPass(1, out xrp2);
                        //textinfo.text = "" + xrp.GetRenderParameterCount();
                                    XRRenderParameter xrpr1, xrpr2;
                                    xrp1.GetRenderParameter(cam, 0, out xrpr1);
                                    xrp2.GetRenderParameter(cam, 0, out xrpr2);
                        Debug.Log(xrpr1.projection);
                        Debug.Log(xrpr2.projection);*/
            //xrp.GetRenderParameter(cam, 1, out xrpr2);
            //Debug.Log(xrpr1.view);
            // Debug.Log(cam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left)); They are the same
            //Debug.Log(xrpr2 .view);
            // Debug.Log(cam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right));
           // Debug.Log("HERE1");
/*            RenderTexture lastActive = RenderTexture.active;
            RenderTexture.active = colorTex;
            Texture2D texy = new Texture2D(RenderTexture.active.width, RenderTexture.active.height);
            texy.ReadPixels(new Rect(0, 0, RenderTexture.active.width, RenderTexture.active.height), 0, 0);
            texy.Apply();
            byte[] dat = texy.EncodeToPNG();
            File.WriteAllBytes(UnityEngine.Application.persistentDataPath + "/colortex0" + count + ".png", dat);
            Destroy(texy);
          //  Debug.Log("HERE2");
            RenderTexture.active = depthTex;
            texy = new Texture2D(RenderTexture.active.width, RenderTexture.active.height);
            texy.ReadPixels(new Rect(0, 0, RenderTexture.active.width, RenderTexture.active.height), 0, 0);
            texy.Apply();
            dat = texy.EncodeToPNG();
           // Debug.Log("HERE3");
            File.WriteAllBytes(UnityEngine.Application.persistentDataPath + "/depth0" + count + ".png", dat);
            Debug.Log("HERE4");
            Destroy(texy);
            RenderTexture.active = lastActive;*/
          //  Debug.Log("WITTEN");
           // count += 1;
        }
        StartCoroutine(createtexture());
    }
    void StartSubsystem()
    {
        string match = "Display Sample";
        List<XRDisplaySubsystemDescriptor> displays = new List<XRDisplaySubsystemDescriptor>();
        SubsystemManager.GetSubsystemDescriptors(displays);

        //Unity.XR.SDK.DisplaySampleXRLoader.Start()
        foreach (var d in displays)
        {
            if (d.id.Contains(match))
            {
                XRDisplaySubsystem dispInst = d.Create();

                if (dispInst != null)
                {
                    Debug.Log("Starting display " + d.id);
                    dispInst.Start();
                }
                else
                {
                    Debug.Log("Can' start subsystem: "+d.id);
                }
            }
        }
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //if (cam.name != Camera.current.name) return;
       // Graphics.Blit(source, depthTex, PostprocessMaterial);
        Graphics.Blit(source, destination);
    }
    void setMatrix()
    {
        Matrix4x4 leftviewmatrix = new Matrix4x4();
        leftviewmatrix.SetColumn(0, new Vector4(1, 0, 0, 1));
        leftviewmatrix.SetColumn(1, new Vector4(0, 1, 0, -1));
        leftviewmatrix.SetColumn(2, new Vector4(0, 0, -1, (float)-13.91));
        leftviewmatrix.SetColumn(3, new Vector4(0, 0, 0, 1));

        Matrix4x4 rightviewmatrix = new Matrix4x4();
        rightviewmatrix.SetColumn(0, new Vector4(1, 0, 0, -1));
        rightviewmatrix.SetColumn(1, new Vector4(0, 1, 0, -1));
        rightviewmatrix.SetColumn(2, new Vector4(0, 0, -1, (float)-13.91));
        rightviewmatrix.SetColumn(3, new Vector4(0, 0, 0, 1));
        cam.SetStereoProjectionMatrix(Camera.StereoscopicEye.Left, leftviewmatrix);
        cam.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right, rightviewmatrix);
    }
}