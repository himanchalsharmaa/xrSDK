using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.XR.XRDisplaySubsystem;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
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




        StartSubsystem();



        cam = GetComponent<Camera>();
        colorTex = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        colorTex.autoGenerateMips = false;
        colorTex.anisoLevel = 0;
        colorTex.name = "ColorTexture";
        //colorTex1 = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        //colorTex1.autoGenerateMips = false;
        //colorTex1.anisoLevel = 0;
        //colorTex1.name = "ColorTexture1";
        textinfo.text = "" + XRSettings.enabled;
        //if (XRSettings.enabled)
        //{
        //    colorTex.vrUsage = VRTextureUsage.TwoEyes;
        //  //  colorTex.width *= 2;
        SubsystemManager.GetSubsystems(displays);
       // textinfo.text = "" + displays[0].supportedTextureLayouts;
        displays[0].singlePassRenderingDisabled = false;
       //colorTex.vrUsage = VRTextureUsage.TwoEyes;
        //    if (displays.Count > 0)
        //    {
        //       // displays[0].textureLayout = TextureLayout.Texture2DArray;
        //           Debug.Log(displays[0].singlePassRenderingDisabled);
        //       // textinfo.text = ""+ displays[0].textureLayout;
        //    }

        //}

        colorTex.Create();
        //colorTex1.Create();

        //  cam.depthTextureMode |= DepthTextureMode.Depth;
        //cam.targetTexture = colorTex;
        //cam.SetTargetBuffers(colorTex.colorBuffer, depthTex.depthBuffer);

        //PostprocessShader = Shader.Find("GR/ExtractDepth");

        //if (PostprocessShader == null)
        //    PostprocessShader = Resources.Load<Shader>("ExtractDepth.shader");

        //if (PostprocessShader)
        //{
        //    PostprocessMaterial = new Material(PostprocessShader);
        //    PostprocessMaterial.enableInstancing = true;
        //    Debug.Log("[GR]: Found Shader for Depth Post Processing!");
        //}
        //else
        //{
        //    Debug.LogError("[GR]: Did not find Shader for Depth Post Processing!");
        //}
        StartCoroutine(createtexture());
    }

    void Update()
    {
        //CommandBuffer.SetSinglePassStereo(SinglePassStereoMode.SideBySide)
        // elapsed += Time.deltaTime;
        // if (elapsed>1) {
        // elapsed = elapsed % 1f;
        //  Debug.Log(displays[0].GetRenderPassCount());
        //colorTex = displays[0].GetRenderTextureForRenderPass(0);
        // RenderTexture.active = displays[0].GetRenderTextureForRenderPass(0);
        //Texture2D tex = new Texture2D(Screen.width/2, Screen.height/2, TextureFormat.ARGB32, false);
        //  tex.ReadPixels(new Rect(0, 0, Screen.width/2, Screen.height/2), 0, 0);
        //RenderTexture.active = null;
        //   tex.Apply();
        //   Graphics.Blit(tex,colorTex);
        //  Destroy(tex);
        //byte[] data = tex.EncodeToPNG();
        //Debug.Log(data.Length);
        //Destroy(tex);
        //count += 1;
        //File.WriteAllBytes(Application.persistentDataPath + "/sourceTexture_" + count + ".png", data);
        //  }
        //  colorTex1 = displays[0].GetRenderTextureForRenderPass(0);

        //cam.Render();


    }
    IEnumerator createtexture()
    {
        yield return new WaitForEndOfFrame();
        elapsed += Time.deltaTime;
        if (elapsed > 1)
        {
            elapsed = elapsed % 1f;
            XRRenderPass xrp;
            displays[0].GetRenderPass(0, out xrp);
            textinfo.text= ""+xrp.GetRenderParameterCount();
/*            XRRenderParameter xrpr1, xrpr2;
            xrp.GetRenderParameter(cam, 0, out xrpr1);
            xrp.GetRenderParameter(cam, 1, out xrpr2);
            Debug.Log(xrpr1.projection);
            Debug.Log(xrpr2.projection);*/

            //colorTex1 = displays[0].GetRenderTextureForRenderPass(1);
            RenderTexture lastActive = RenderTexture.active;
            RenderTexture.active= displays[0].GetRenderTextureForRenderPass(0);
            Texture2D texy = new Texture2D(RenderTexture.active.width, RenderTexture.active.height);
            texy.ReadPixels(new Rect(0, 0, RenderTexture.active.width, RenderTexture.active.height), 0, 0);
            texy.Apply();
            byte[] dat = texy.EncodeToPNG();
            File.WriteAllBytes(UnityEngine.Application.persistentDataPath + "/sourceTexture0_" + count + ".png", dat);
            Destroy(texy);
            RenderTexture.active = lastActive;
            //RenderTexture.active = displays[0].GetRenderTextureForRenderPass(1);
            //Texture2D texy1 = new Texture2D(RenderTexture.active.width, RenderTexture.active.height);
            //texy.ReadPixels(new Rect(0, 0, RenderTexture.active.width, RenderTexture.active.height), 0, 0);
            //texy.Apply();
            //byte[] dat1 = texy1.EncodeToPNG();
            //textinfo.text = "" + displays[0].GetRenderPassCount();
            //File.WriteAllBytes(UnityEngine.Application.persistentDataPath + "/sourceTexture1_" + count + ".png", dat);

            ////  Graphics.Blit(texy, colorTex1);
            //Destroy(texy);
            //RenderTexture.active = lastActive;
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
        //RenderTexture.active = displays[0].GetRenderTextureForRenderPass(0);
        //Texture2D tex = new Texture2D(Screen.width / 2, Screen.height / 2, TextureFormat.ARGB32, false);
        //tex.ReadPixels(new Rect(0, 0, Screen.width / 2, Screen.height / 2), 0, 0);
        //tex.Apply();
        //byte[] dat = tex.EncodeToPNG();
        //Debug.Log(dat.Length);
        //img.texture = (Texture)tex;
        //Graphics.Blit(tex, colorTex);
        //Destroy(tex);
        //colorTex1 = RenderTexture.active;
        //RenderTexture.active = displays[0].GetRenderTextureForRenderPass(0);
        //elapsed += Time.deltaTime;
        //if (elapsed > 1)
        //{
        //    elapsed = elapsed % 1f;
        //    count += 1;
/*            colorTex = displays[0].GetRenderTextureForRenderPass(0);
            RenderTexture lastActive = RenderTexture.active;
            RenderTexture.active = displays[0].GetRenderTextureForRenderPass(0);
            Texture2D texy = new Texture2D(Screen.width / 2, Screen.height / 2, TextureFormat.ARGB32, false);
            texy.ReadPixels(new Rect(0, 0, Screen.width / 2, Screen.height / 2), 0, 0);
            texy.Apply();
            byte[] dat = texy.EncodeToPNG();
            Debug.Log(dat.Length);
            File.WriteAllBytes(Application.persistentDataPath + "/sourceTexture_" + count + ".png", dat);
            Graphics.Blit(texy, colorTex1);
            Destroy(texy);*/
        //    RenderTexture.active = lastActive;
        //}
        colorTex = displays[0].GetRenderTextureForRenderPass(0);
        Graphics.Blit(source, destination);

        //  colorTex1 = displays[0].GetRenderTextureForRenderPass(0);
    }
}