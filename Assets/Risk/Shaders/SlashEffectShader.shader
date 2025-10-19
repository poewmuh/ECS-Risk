Shader "Custom/SlashEffect"
{
    Properties
    {
        _MainTex ("Slash Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Color ("Slash Color", Color) = (1,1,1,1)
        _EmissionStrength ("Emission Strength", Range(1, 10)) = 3
        
        _Spread ("Spread (0=center, 1=full)", Range(0, 1)) = 0.5
        _SpreadSoftness ("Spread Edge Softness", Range(0.01, 0.5)) = 0.1
        
        _RevealProgress ("Reveal Progress", Range(0, 1)) = 1
        _RevealSoftness ("Reveal Edge Softness", Range(0.01, 0.3)) = 0.05
        
        _FadeProgress ("Fade Progress", Range(0, 1)) = 1
        _NoiseSpeed ("Noise Speed", Vector) = (1, 0, 0, 0)
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
            "IgnoreProjector"="True"
        }
        
        Blend SrcAlpha One
        ZWrite Off
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_FOG_COORDS(1)
            };
            
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _NoiseTex_ST;
            
            float4 _Color;
            float _EmissionStrength;
            float _Spread;
            float _SpreadSoftness;
            float _RevealProgress;
            float _RevealSoftness;
            float _FadeProgress;
            float4 _NoiseSpeed;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                float revealMask = smoothstep(
                    _RevealProgress - _RevealSoftness,
                    _RevealProgress + _RevealSoftness,
                    uv.x
                );
                
                revealMask = 1.0 - revealMask;
                
                float uvX = (uv.x - 0.5) * 2.0;
                float spreadMask = 1.0 - abs(uvX);
                
                float spreadThreshold = lerp(1.0, 0.0, _Spread);
                spreadMask = smoothstep(
                    spreadThreshold - _SpreadSoftness, 
                    spreadThreshold + _SpreadSoftness, 
                    spreadMask
                );
                
                float4 mainTex = tex2D(_MainTex, uv);
                
                float2 noiseUV = uv + _Time.y * _NoiseSpeed.xy;
                float4 noise = tex2D(_NoiseTex, noiseUV);
                
                float coreGlow = 1.0 - abs(uvX);
                coreGlow = pow(coreGlow, 3.0);
                
                float alpha = mainTex.a * spreadMask * revealMask * _FadeProgress;
                
                alpha *= lerp(0.8, 1.2, noise.r);
                
                float3 finalColor = _Color.rgb * _EmissionStrength;
                
                finalColor = lerp(finalColor, float3(1,1,1), coreGlow * 0.5);
                
                finalColor *= mainTex.rgb;
                
                finalColor *= i.color.rgb;
                alpha *= i.color.a;
                
                alpha = saturate(alpha);
                
                float4 col = float4(finalColor, alpha);
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    
    FallBack "Particles/Additive"
}