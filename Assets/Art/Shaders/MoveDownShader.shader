Shader "Unlit/MoveDownShader"
{
    Properties
    {
        _Radius ("Radius", Float) = 1.0
        _MaxVerticalDisplacement ("Max Vertical Displacement", Float) = 0.125
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
            #pragma fragment SpriteFrag
            //#pragma target 2.0
            //#pragma multi_compile_instancing
            //#pragma multi_compile_local _ PIXELSNAP_ON
            //#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            //struct MeshData
            //{
            //    float4 vertex : POSITION;
            //    float2 uv : TEXCOORD0;
            //};

            //struct v2f
           // {
             //   float2 uv : TEXCOORD0;
             //   float4 vertex : SV_POSITION;
            //};

            //sampler2D _MainTex;
            float2 _PlayerPos;
            float _MaxVerticalDisplacement;
            float _Radius;

            v2f vert (appdata_t v)
            {
                v2f o;
                
                float2 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float2 diff = worldPos - _PlayerPos;
                float sqrDist = diff.x * diff.x + diff.y * diff.y;
                float sqrRadius = _Radius * _Radius;
                float radDiff = max(0, sqrRadius - sqrDist);
                float radRatio = radDiff / sqrRadius;
                float verticalDisplacement = radRatio * _MaxVerticalDisplacement;
                v.vertex.y = v.vertex.y - verticalDisplacement;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }
            ENDCG
        }
    }
}
