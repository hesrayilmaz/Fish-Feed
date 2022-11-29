// Combined and opotimized Standard Image Effects tonemapper eye adaptation and Mobile Color Grading
// By ALIyerEdon
// Spring 2021

using System;
using UnityEngine;

    [ExecuteInEditMode]
    [RequireComponent(typeof (Camera))]
    [ImageEffectAllowedInSceneView]
    public class AutoMobileColorGrading : MonoBehaviour
{
   
    public enum ToneMapping
    {
        ACES,None
    };
    public enum AdaptiveTexSize
        {
            Square4 = 4,
            Square8 = 8,
            Square16 = 16,
            Square32 = 32,
            Square64 = 64,
            Square128 = 128,
            Square256 = 256,
            Square512 = 512,
            Square1024 = 1024,
        };
    public Material tonemapMaterial = null;
    public Shader shader;
    [Header("Auto Exposure")]
    [Space(5)]
    public bool useAutoExposureMode = false;
    public AdaptiveTexSize adaptiveTextureSize = AdaptiveTexSize.Square128;

        // REINHARD parameter
        
        public float middleGrey = 0.5f;
        public float white = 2.0f;
        public float adaptionSpeed = 3f;

        public bool validRenderTextureFormat = true;
        
        private RenderTexture rt = null;
        private RenderTextureFormat rtFormat = RenderTextureFormat.ARGBHalf;

    [Space(5)]
    [Header("Tone Mapping")]
        // Tonemapping type - ACES Filmic or None
        public ToneMapping toneMapping = ToneMapping.ACES;


        [Header("Image Settings")]
        [Range(0.3f, 1)]
        public float OveralExposure = 0.37f;

        [Range(1, 2)]
        public float Contrast = 1.23f;

        [Range(1, 0)]
        public float Saturation = 0;

        [Range(0.3f, 1)]
        public float Gamma = 0.87f;

        [Header("Vignette")]
        [Range(0, 0.5f)]
        public float vignetteIntensity = 0.37f;

        [Header("Color Balance")]
        [Range(-100, 100)]
        public float R;
        [Range(-100, 100)]
        public float G;
        [Range(-100, 100)]
        public float B;

        void OnEnable()
        {
            if (tonemapMaterial == null)
            {
                shader = Shader.Find("Hidden/AutoMobileColorGrading");
                tonemapMaterial = Resources.Load("AutoMobileColorGrading") as Material;
                tonemapMaterial.shader = shader;
            }
        }
        private void OnDisable()
        {
            if (rt)
            {
                DestroyImmediate(rt);
                rt = null;
            }
        }


        private bool CreateInternalRenderTexture()
        {
            if (rt)
            {
                return false;
            }
            rtFormat = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf) ? RenderTextureFormat.RGHalf : RenderTextureFormat.ARGBHalf;
            rt = new RenderTexture(1, 1, 0, rtFormat);
            rt.hideFlags = HideFlags.DontSave;
            return true;
        }


        // attribute indicates that the image filter chain will continue in LDR
        [ImageEffectTransformsToLDR]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
        if(!useAutoExposureMode)
        {


            // Mobile Color Grading
            tonemapMaterial.SetVector("_Color", new Vector4(R / 1000, G / 1000, B / 1000, 1f));
            tonemapMaterial.SetFloat("_Contrast", Contrast);
            tonemapMaterial.SetFloat("_Gamma", Gamma);
            tonemapMaterial.SetFloat("_Exposure", OveralExposure);
            tonemapMaterial.SetFloat("_VignetteIntensity", vignetteIntensity);
            tonemapMaterial.SetFloat("_Saturation", Saturation);

            if (Saturation > 0)
            {
                tonemapMaterial.DisableKeyword("SaturN_OFF");
                tonemapMaterial.EnableKeyword("SaturN_ON");
            }
            else
            {
                tonemapMaterial.DisableKeyword("SaturN_ON");
                tonemapMaterial.EnableKeyword("SaturN_OFF");
            }

            if (vignetteIntensity != 0)
            {
                tonemapMaterial.DisableKeyword("Vignette_OFF");
                tonemapMaterial.EnableKeyword("Vignette_ON");
            }
            else
            {
                tonemapMaterial.DisableKeyword("Vignette_ON");
                tonemapMaterial.EnableKeyword("Vignette_OFF");
            }

            if (toneMapping == ToneMapping.ACES)
            {
                tonemapMaterial.DisableKeyword("ACES_OFF");
                tonemapMaterial.EnableKeyword("ACES_ON");
            }
            if (toneMapping == ToneMapping.None)
            {

                tonemapMaterial.DisableKeyword("ACES_ON");
                tonemapMaterial.EnableKeyword("ACES_OFF");
            }


            Graphics.Blit(source, destination, tonemapMaterial, 4);

        }
            else
        { 
#if UNITY_EDITOR
        validRenderTextureFormat = true;
            if (source.format != RenderTextureFormat.ARGBHalf)
            {
                validRenderTextureFormat = false;
            }
#endif

            // still here?
            // =>  adaptive tone mapping:
            // builds an average log luminance, tonemaps according to
            // middle grey and white values (user controlled)

            // AdaptiveReinhardAutoWhite will calculate white value automagically

            bool freshlyBrewedInternalRt = CreateInternalRenderTexture(); // this retrieves rtFormat, so should happen before rt allocations

            RenderTexture rtSquared = RenderTexture.GetTemporary((int) adaptiveTextureSize, (int) adaptiveTextureSize, 0, rtFormat);
            Graphics.Blit(source, rtSquared);

            int downsample = (int) Mathf.Log(rtSquared.width*1.0f, 2);
            // Debug.Log(rtSquared.width + " / " + rtSquared.height + " : " + downsample);
            int div = 2;
            var rts = new RenderTexture[downsample];
            for (int i = 0; i < downsample; i++)
            {
                rts[i] = RenderTexture.GetTemporary(rtSquared.width/div, rtSquared.width/div, 0, rtFormat);
                div *= 2;
            }

            // downsample pyramid

            var lumRt = rts[downsample - 1];
            Graphics.Blit(rtSquared, rts[0], tonemapMaterial, 1);
           
            for (int i = 0; i < downsample - 1; i++)
            {
               Graphics.Blit(rts[i], rts[i + 1]);
               lumRt = rts[i + 1];
            }

            // we have the needed values, let's apply adaptive tonemapping

            adaptionSpeed = adaptionSpeed < 0.001f ? 0.001f : adaptionSpeed;
            tonemapMaterial.SetFloat("_AdaptionSpeed", adaptionSpeed);

            rt.MarkRestoreExpected(); // keeping luminance values between frames, RT restore expected

#if UNITY_EDITOR
            if (Application.isPlaying && !freshlyBrewedInternalRt)
                Graphics.Blit(lumRt, rt, tonemapMaterial, 2);
            else
                Graphics.Blit(lumRt, rt, tonemapMaterial, 3);
#else
			Graphics.Blit (lumRt, rt, tonemapMaterial, freshlyBrewedInternalRt ? 3 : 2);
#endif
            // Auto Exposure
            middleGrey = middleGrey < 0.001f ? 0.001f : middleGrey;
            tonemapMaterial.SetVector("_HdrParams", new Vector4(middleGrey, middleGrey, middleGrey, white*white));
            tonemapMaterial.SetTexture("_SmallTex", rt);

            // Mobile Color Grading
            tonemapMaterial.SetVector("_Color", new Vector4(R / 1000, G / 1000, B / 1000, 1f));
            tonemapMaterial.SetFloat("_Contrast", Contrast);
            tonemapMaterial.SetFloat("_Gamma", Gamma);
            tonemapMaterial.SetFloat("_Exposure", OveralExposure);
            tonemapMaterial.SetFloat("_VignetteIntensity", vignetteIntensity);
            tonemapMaterial.SetFloat("_Saturation", Saturation);

            if (Saturation > 0)
            {
                tonemapMaterial.DisableKeyword("SaturN_OFF");
                tonemapMaterial.EnableKeyword("SaturN_ON");
            }
            else
            {
                tonemapMaterial.DisableKeyword("SaturN_ON");
                tonemapMaterial.EnableKeyword("SaturN_OFF");
            }

            if (vignetteIntensity != 0)
            {
                tonemapMaterial.DisableKeyword("Vignette_OFF");
                tonemapMaterial.EnableKeyword("Vignette_ON");
            }
            else
            {
                tonemapMaterial.DisableKeyword("Vignette_ON");
                tonemapMaterial.EnableKeyword("Vignette_OFF");
            }

            if (toneMapping == ToneMapping.ACES)
            {
                tonemapMaterial.DisableKeyword("ACES_OFF");
                tonemapMaterial.EnableKeyword("ACES_ON");
            }
            if (toneMapping == ToneMapping.None)
            {

                tonemapMaterial.DisableKeyword("ACES_ON");
                tonemapMaterial.EnableKeyword("ACES_OFF");
            }



            Graphics.Blit(source, destination, tonemapMaterial, 0);

            for (int i = 0; i < downsample; i++)
            {
                RenderTexture.ReleaseTemporary(rts[i]);
            }
            RenderTexture.ReleaseTemporary(rtSquared);
        }
    }
}
