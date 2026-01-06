Shader "Hidden/SelectiveEdgeDetection" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _EdgeColor("Edge Color", Color) = (0,0,0,1)
        _Threshold("Threshold", Float) = 0.5
    }
        SubShader{
            Pass {
                CGPROGRAM
                #pragma vertex vert_img
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                sampler2D _CameraDepthNormalsTexture; // 카메라의 깊이/법선 텍스처
                float4 _EdgeColor;
                float _Threshold;

                fixed4 frag(v2f_img i) : SV_Target {
                    fixed4 col = tex2D(_MainTex, i.uv);

                // 현재 픽셀과 주변 픽셀의 Normal/Depth 차이 계산 (Sobel Filter)
                float4 n1; float d1;
                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv), d1, n1);

                // 단순화된 로직: 주변 픽셀과의 차이가 크면 검은색 선 출력
                // (실제 구현 시 Sobel 샘플링 로직이 추가됩니다)

                return col; // 결과값 합성
            }
            ENDCG
        }
        }
}