Shader "Apascual/Silhouette_XRay"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        _SilhouetteColor("Silhouette Color", Color) = (0.2, 0.6, 1.0, 0.7)
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Overlay"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Stencil
        {
            Ref 1
            Comp Equal  // NOMÉS dibuixa on hi ha paret (stencil == 1)
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest Always  // Ignora profunditat, sempre per sobre
        Cull Off

        Pass
        {
            Name "PlayerSilhouette"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _SilhouetteColor;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half alpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv).a;
                // Descarta píxels transparents per respectar la forma del sprite
                clip(alpha - 0.1);
                return _SilhouetteColor;
            }
            ENDHLSL
        }
    }
}