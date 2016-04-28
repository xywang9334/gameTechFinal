Shader "Custom/ballShader/Lambert" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
//		_MainTex ("Albedo (RGB)", 2D) = "white" {}
//		_Glossiness ("Smoothness", Range(0,1)) = 0.5
//		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Pass {
//			Tags { "RenderType"="Opaque" }
			Tags { "LightMode" = "ForwardBase"}
			LOD 200
		
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
//			#pragma surface surf Standard fullforwardshadows
			#pragma vertex vert
			#pragma fragment frag

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

//			sampler2D _MainTex;

			struct vertexInput {
//				float2 uv_MainTex;
				float3 normal : NORMAL;
				float4 vertex : POSITION;
			};

			struct vertexOutput {
				float4 pos : POSITION;
				float4 col : COLOR;
			};

//			half _Glossiness;
//			half _Metallic;
			// self defined variable
			uniform fixed4 _Color;
			// unity variable
			uniform fixed4 _LightColor0;

			vertexOutput vert(vertexInput v) {
				vertexOutput output;
				float3 normalDirection = normalize(mul(float4(v.normal, 0.0), _World2Object).xyz);
				float3 lightDirection;
				float atten = 1.0;
				lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				float3 diffuseReflection = atten * _LightColor0.xyz * max (0.0, dot(normalDirection, lightDirection));
				float3 lightFinal = diffuseReflection + UNITY_LIGHTMODEL_AMBIENT.xyz;
				output.col = float4(lightFinal * _Color.rgb, 1.0);
				output.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				return output;
			}


			float4 frag(vertexOutput i) : COLOR {
				return i.col;
			}

//			void surf (vertexInput IN, inout SurfaceOutputStandard o) {
//				// Albedo comes from a texture tinted by color
//				fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
//				o.Albedo = c.rgb;
//				// Metallic and smoothness come from slider variables
//				o.Metallic = _Metallic;
//				o.Smoothness = _Glossiness;
//				o.Alpha = c.a;
//			}
			ENDCG
		}

	} 
	// if it doesn't work
//	FallBack "Diffuse"
}
