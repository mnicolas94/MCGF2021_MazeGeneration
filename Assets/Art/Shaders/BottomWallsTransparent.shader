Shader "Unlit/BottomWallsTransparent"
{
    Properties
    {
        _AlternativeTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", Float) = 1.0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "UnitySprites.cginc"
            
            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct vert2frag
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float insideRadius : TEXCOORD1;
                float belowAbsolute : TEXCOORD2;
                float4 worldPos : TEXCOORD3;
            };

            //sampler2D _MainTex;
            sampler2D _AlternativeTex;
            float2 _PlayerPos;
            float _Radius;
            
            float2 insideRadiusAndBelowAbsolute(float4 vertexWorld)
            {
                float2 diff = vertexWorld - _PlayerPos;
                float sqrDist = diff.x * diff.x + diff.y * diff.y;
                float sqrRadius = _Radius * _Radius;
                float radDiff = sqrRadius - sqrDist;
                float insideRadius = radDiff < 0 ? 0 : 1;
                float belowAbsolute = diff.y < -abs(diff.x) / 2 + 1.5;
                return float2(insideRadius, belowAbsolute);
            }
            
            float2 isometricInsideRadiusAndBelowAbsolute(float4 vertexWorld)
            {
                float left = round((_PlayerPos.x - _Radius) / 0.5) * 0.5;
                float right = round((_PlayerPos.x + _Radius) / 0.5) * 0.5;
                float up = round(_PlayerPos.y / 0.25) * 0.25;
                float f1 = vertexWorld.y < (vertexWorld.x - left) / 2  + up + 0.01;
                float f2 = vertexWorld.y < (-vertexWorld.x + right) / 2  + up + 0.01;
                float f3 = vertexWorld.y > (vertexWorld.x - right) / 2  + up;
                float f4 = vertexWorld.y > (-vertexWorld.x + left) / 2  + up;
                
                float2 diff = vertexWorld - _PlayerPos;
                float sqrDist = diff.x * diff.x + diff.y * diff.y;
                float sqrRadius = _Radius * _Radius;
                float radDiff = sqrRadius - sqrDist;
                //float insideRadius = radDiff < 0 ? 0 : 1;
                float insideRadius = f1 * f2 * f3 * f4;
                float belowAbsolute = diff.y < -abs(diff.x) / 2 + 1.5;
                return float2(insideRadius, belowAbsolute);
            }
            
            vert2frag vert (MeshData v)
            {
                vert2frag o;
                
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float2 res = insideRadiusAndBelowAbsolute(worldPos);
                float insideRadius = res.x;
                float belowAbsolute = res.y;
                
                //insideRadius *= belowAbsolute;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.insideRadius = insideRadius;
                o.belowAbsolute = belowAbsolute;
                o.worldPos = worldPos;
                return o;
            }
            
            fixed4 CustomSampleSpriteTexture (sampler2D tex, float2 uv)
            {
                fixed4 color = tex2D (tex, uv);
            
            #if ETC1_EXTERNAL_ALPHA
                fixed4 alpha = tex2D (_AlphaTex, uv);
                color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
            #endif
            
                return color;
            }

            fixed4 frag (vert2frag i) : SV_Target
            {
                float2 res = isometricInsideRadiusAndBelowAbsolute(i.worldPos);
                float insideRadius = res.x;
                float belowAbsolute = res.y;
                // sample the texture
                //float insideRadius = i.insideRadius;
                //float insideRadius = round(i.insideRadius);
                //float insideRadius = ceil(i.insideRadius);
                //float insideRadius = floor(i.insideRadius);
                //float belowAbsolute = floor(i.belowAbsolute);
                //insideRadius *= belowAbsolute;
                
                

                fixed4 colOutside = CustomSampleSpriteTexture (_MainTex, i.uv) * (1 - insideRadius);
                colOutside.rgb *= colOutside.a;
                fixed4 colInside = CustomSampleSpriteTexture(_AlternativeTex, i.uv) * insideRadius;
                colInside.rgb *= colInside.a;
                
                //return float4(insideRadius, insideRadius, insideRadius, 1) * colOutside.a;
                return colOutside + colInside;
            }
            ENDCG
        }
    }
}