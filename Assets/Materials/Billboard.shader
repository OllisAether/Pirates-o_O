Shader "OllisAether/Billboard"
{
    Properties
    {
        [MainTexture] _MainTex("Texture", 2D) = "transparent" {}
        [Scale] _Scale("Scale", Float) = 1.0
        [MaterialToggle] _ConsistentSize("Consistent Size", Float) = 0
    }

    SubShader
    {
        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "DisableBatching" = "True"
        }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            const float3 vect3Zero = float3(0.0, 0.0, 0.0);

            sampler2D _MainTex;

            CBUFFER_START(UnityPerMaterial)
                float _Scale;
                float _ConsistentSize;
            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;

                float4 camPos = float4(UnityObjectToViewPos(vect3Zero).xyz, 1.0);    // UnityObjectToViewPos(pos) is equivalent to mul(UNITY_MATRIX_MV, float4(pos, 1.0)).xyz,
                                                                                    // This gives us the camera's origin in 3D space (the position (0,0,0) in Camera Space)
                float distance = length(camPos);
                float scale = _Scale * distance * _ConsistentSize + (1 - _ConsistentSize) * _Scale;    // If consistent size is enabled, scale the billboard based on distance to camera, otherwise use the base scale value

                float4 viewDir = float4(v.pos.x * scale, v.pos.y * scale, 0.0, 0.0);            // Since w is 0.0, in homogeneous coordinates this represents a vector direction instead of a position
                float4 outPos = mul(UNITY_MATRIX_P, camPos + viewDir);            // Add the camera position and direction, then multiply by UNITY_MATRIX_P to get the new projected vert position

                o.pos = outPos;
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Don't need to do anything special, just render the texture
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}