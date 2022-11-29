Shader "Hidden/AutoMobileColorGrading" {
	Properties {
		_MainTex ("", 2D) = "black" {}
		_SmallTex ("", 2D) = "grey" {}
		_Color("Color",Vector) = (1,0,0,1)
		_Contrast("Contrast",Float) = 1
		_Exposure("Exposure",Float) = 1
		_Gamma("Gamma",Float) = 1
		_VignetteIntensity("_VignetteIntensity",Float) = .3
		_Saturation("Saturatiob",Float) = 1
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	 
	#pragma multi_compile ACES_ON ACES_OFF
	#pragma multi_compile SaturN_ON SaturN_OFF
	#pragma multi_compile Vignette_ON Vignette_OFF

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;
	sampler2D _SmallTex;
	
	float4 _HdrParams;
	float4 _MainTex_TexelSize;
	half4 _MainTex_ST;
	float _AdaptionSpeed;
	half4 _Color;
	//////////////////////////////////////////////////////////////////////
	// Mobile color grading
			float _Contrast;
			float _Exposure;
			float _Gamma;
			float _VignetteIntensity;
			float _Saturation;

			half3 AdjustContrast(half3 color, half contrast) 
			{
				#if !UNITY_COLORSPACE_GAMMA
				    color = LinearToGammaSpace(color);
				#endif
				    color = saturate(lerp(half3(0.5, 0.5, 0.5), color, contrast));
				#if !UNITY_COLORSPACE_GAMMA
				    color = GammaToLinearSpace(color);
				#endif
				    return color;
			}

			float3 ACESFilm( float3 x )
			{
			    float a = 2.51f;
			    float b = 0.03f;
			    float c = 2.43f;
			    float d = 0.59f;
			    float e = 0.14f;
			    return saturate((x*(a*x+b))/(x*(c*x+d)+e));
			}
	//////////////////////////////////////////////////////////////////////

	v2f vert( appdata_img v ) 
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	} 

	float4 fragLog(v2f i) : SV_Target 
	{
		const float EPSILON = 1e-4h;
 
		float fLogLumSum = 0.0f;
 
		fLogLumSum += log( max( EPSILON, Luminance(tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + _MainTex_TexelSize.xy * float2(-1,-1), _MainTex_ST)).rgb)));
		fLogLumSum += log( max( EPSILON, Luminance(tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + _MainTex_TexelSize.xy * float2( 1, 1), _MainTex_ST)).rgb)));
		fLogLumSum += log( max( EPSILON, Luminance(tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + _MainTex_TexelSize.xy * float2(-1, 1), _MainTex_ST)).rgb)));
		fLogLumSum += log( max( EPSILON, Luminance(tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + _MainTex_TexelSize.xy * float2( 1,-1), _MainTex_ST)).rgb)));

		float avg = fLogLumSum / 4.0;
		return float4(avg, avg, avg, avg);
	}

	float4 fragExp(v2f i) : SV_Target 
	{
		float2 lum = float2(0.0f, 0.0f);
		
		lum += tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv  + _MainTex_TexelSize.xy * float2(-1,-1), _MainTex_ST)).xy;
		lum += tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv  + _MainTex_TexelSize.xy * float2(1,1), _MainTex_ST)).xy;
		lum += tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + _MainTex_TexelSize.xy * float2(1,-1), _MainTex_ST)).xy;
		lum += tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv  + _MainTex_TexelSize.xy * float2(-1,1), _MainTex_ST)).xy;

		lum = exp(lum / 4.0f);
		
		return float4(lum.x, lum.y, lum.x, saturate(0.0125 * _AdaptionSpeed));
	}
			
	float3 ToCIE(float3 FullScreenImage)
	{
		// RGB -> XYZ conversion 
		// http://www.w3.org/Graphics/Color/sRGB 
		// The official sRGB to XYZ conversion matrix is (following ITU-R BT.709)
		// 0.4125 0.3576 0.1805
		// 0.2126 0.7152 0.0722 
		// 0.0193 0.1192 0.9505 
		 
		float3x3 RGB2XYZ = {0.5141364, 0.3238786, 0.16036376, 0.265068, 0.67023428, 0.06409157, 0.0241188, 0.1228178, 0.84442666};
		 
		float3 XYZ = mul(RGB2XYZ, FullScreenImage.rgb); 
		 
		// XYZ -> Yxy conversion 
		 
		float3 Yxy; 
		 
		Yxy.r = XYZ.g; 
		 
		// x = X / (X + Y + Z) 
		// y = X / (X + Y + Z) 
		 
		float temp = dot(float3(1.0,1.0,1.0), XYZ.rgb); 
		 
		Yxy.gb = XYZ.rg / temp;	
		
		return Yxy;	
	}		
	
	float3 FromCIE(float3 Yxy)
	{	
		float3 XYZ;
		// Yxy -> XYZ conversion 
		XYZ.r = Yxy.r * Yxy.g / Yxy. b; 
		 
		// X = Y * x / y 
		XYZ.g = Yxy.r;
		 
		// copy luminance Y 
		XYZ.b = Yxy.r * (1 - Yxy.g - Yxy.b) / Yxy.b;
		 
		// Z = Y * (1-x-y) / y 
		 
		// XYZ -> RGB conversion 
		// The official XYZ to sRGB conversion matrix is (following ITU-R BT.709) 
		// 3.2410 -1.5374 -0.4986
		// -0.9692 1.8760 0.0416 
		// 0.0556 -0.2040 1.0570 
 
 		float3x3 XYZ2RGB = { 2.5651,-1.1665,-0.3986, -1.0217, 1.9777, 0.0439, 0.0753, -0.2543, 1.1892};		

		return mul(XYZ2RGB, XYZ);
	}
			
	// NOTE/OPTIMIZATION: we're not going the extra CIE detour anymore, but
	// scale with the OUT/IN luminance ratio,this is sooooo much faster 
	
	float4 fragAdaptive(v2f i) : SV_Target 
	{
		///////////////////////////////////////////////////////////
		////////////Eye adaptation auto exposure
		///////////////////////////////////////////////////////////
		float avgLum = tex2D(_SmallTex, i.uv).x;
		float4 color = tex2D (_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));
		
		float cieLum = max(0.000001, Luminance(color.rgb)); //ToCIE(color.rgb);
		
		float lumScaled = cieLum * _HdrParams.z / (0.001 + avgLum.x);
		
		lumScaled = (lumScaled * (1.0f + lumScaled / (_HdrParams.w)))/(1.0f + lumScaled);
		
		//cie.r = lumScaled; 
		
		color.rgb = color.rgb * (lumScaled / cieLum);
		///////////////////////////////////////////////////////////
		////////////Mobile Color Grading 
		///////////////////////////////////////////////////////////
		float4 fColor = float4(1,1,1,1) ;

        // Contrast
        fColor.rgb = AdjustContrast(color, _Contrast);

        #ifdef Vignette_ON

			// Vignette
			half2 coords = i.uv;
			half2 uv = i.uv;
		
			coords = (coords - 0.5) * 2.0;		
			half coordDot = dot (coords,coords);
		 		 
			float mask = 1.0 - coordDot * _VignetteIntensity; 

			fColor *= mask;

		#endif

        #ifdef SaturN_ON

		    // Saturation Calculation 
			float lum = color.r*.3 + color.g*.59 + color.b*.11;
			float3 bw = float3( lum, lum, lum ); 

            fColor.rgb = lerp(fColor.rgb, bw, _Saturation);

        #endif              

			// Gamma
			fColor.rgb = pow(fColor, 1.0 / _Gamma) ;

            // Color
            fColor.rgb  = float4(fColor.r + _Color.r,fColor.g + _Color.g,fColor.b + _Color.b,fColor.a + _Color.a);

         #ifdef ACES_ON

			// ACES Tonemapping

            float3 f = ACESFilm(fColor.rgb * 5 * _Exposure);

            fColor = fixed4(f.r,f.g,f.b,fColor.a);

         #endif

		//color.rgb = FromCIE(cie);		
		return fColor;
	}
	float4 fragMobileColorGrading(v2f i) : SV_Target 
	{
		
		float4 color = tex2D (_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));
		
		///////////////////////////////////////////////////////////
		////////////Mobile Color Grading 
		///////////////////////////////////////////////////////////
		float4 fColor = float4(1,1,1,1) ;

        // Contrast
        fColor.rgb = AdjustContrast(color, _Contrast);

        #ifdef Vignette_ON

			// Vignette
			half2 coords = i.uv;
			half2 uv = i.uv;
		
			coords = (coords - 0.5) * 2.0;		
			half coordDot = dot (coords,coords);
		 		 
			float mask = 1.0 - coordDot * _VignetteIntensity; 

			fColor *= mask;

		#endif

        #ifdef SaturN_ON

		    // Saturation Calculation 
			float lum = color.r*.3 + color.g*.59 + color.b*.11;
			float3 bw = float3( lum, lum, lum ); 

            fColor.rgb = lerp(fColor.rgb, bw, _Saturation);

        #endif              

			// Gamma
			fColor.rgb = pow(fColor, 1.0 / _Gamma) ;

            // Color
            fColor.rgb  = float4(fColor.r + _Color.r,fColor.g + _Color.g,fColor.b + _Color.b,fColor.a + _Color.a);

         #ifdef ACES_ON

			// ACES Tonemapping

            float3 f = ACESFilm(fColor.rgb * 5 * _Exposure);

            fColor = fixed4(f.r,f.g,f.b,fColor.a);

         #endif

		//color.rgb = FromCIE(cie);		
		return fColor;
	}
	
	ENDCG 
	
Subshader {
 // adaptive reinhhard apply
 Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment fragAdaptive
      ENDCG
  }

  // 1
 Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment fragLog
      ENDCG
  }  
  // 2
 Pass {
	  ZTest Always Cull Off ZWrite Off
	  Blend SrcAlpha OneMinusSrcAlpha

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment fragExp
      ENDCG
  }  
  // 3 
 Pass {
	  ZTest Always Cull Off ZWrite Off

	  Blend Off   

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment fragExp
      ENDCG
  }  
  // 4 Mobile  Color Grading Only
   Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment fragMobileColorGrading
      ENDCG
  }
  
}

Fallback off
	
} // shader
