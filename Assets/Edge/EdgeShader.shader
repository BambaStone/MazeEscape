Shader "Custom/SelectiveEdgeDetection"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _EdgeColor("Edge Color", Color) = (0, 0, 0, 1)
        _Sensitivity("Sensitivity", Float) = 3.0
        _EdgeWidth("Edge Width", Float) = 1.5
    }
        SubShader
        {
            ZTest Always Cull Off ZWrite Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert_img
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                sampler2D _ScreenCopyTexture;
                sampler2D _TempOutlineTexture;
                sampler2D _CameraDepthNormalsTexture;

                float4 _MainTex_TexelSize;
                float4 _EdgeColor;
                float _Sensitivity;
                float _EdgeWidth;

                float GetEdge(float2 uv, float2 offset)
                {
                    float4 centerND = tex2D(_CameraDepthNormalsTexture, uv);
                    float4 sampleND = tex2D(_CameraDepthNormalsTexture, uv + offset);

                    float3 centerN, sampleN;
                    float centerD, sampleD;

                    DecodeDepthNormal(centerND, centerD, centerN);
                    DecodeDepthNormal(sampleND, sampleD, sampleN);

                    // 법선과 깊이 차이 계산
                    float3 normalDiff = centerN - sampleN;
                    float edgeNormal = sqrt(dot(normalDiff, normalDiff));
                    float edgeDepth = abs(centerD - sampleD);

                    // 구체에서 너무 민감하게 반응하지 않도록 감도 조절
                    return (edgeNormal + edgeDepth * 10.0) * _Sensitivity;
                }

                fixed4 frag(v2f_img i) : SV_Target
                {
                    // 원본 화면 가져오기
                    fixed4 original = tex2D(_ScreenCopyTexture, i.uv);

                // 마스크 정보 확인 (오브젝트가 있는 영역인지)
                fixed4 mask = tex2D(_TempOutlineTexture, i.uv);

                // [수정] 마스크가 아예 없는 배경 영역은 바로 원본 반환
                if (mask.r < 0.01) return original;

                float2 delta = _MainTex_TexelSize.xy * _EdgeWidth;

                // 4방향 샘플링 (구체는 8방향 시 너무 하얗게 변할 수 있어 4방향 권장)
                float edge = 0;
                edge += GetEdge(i.uv, float2(delta.x, 0));
                edge += GetEdge(i.uv, float2(-delta.x, 0));
                edge += GetEdge(i.uv, float2(0, delta.y));
                edge += GetEdge(i.uv, float2(0, -delta.y));

                // [중요] 에지 강도를 제한하여 물체 내부가 하얗게 타는 것 방지
                edge = saturate(edge);

                // [핵심 수정] 마스크 영역 내에서 '선'인 부분만 에지 컬러를 적용하고, 
                // 선이 아닌 부분은 원본(original)을 유지하도록 합성
                fixed4 finalEdge = lerp(original, _EdgeColor, edge);

                // 마스크 강도에 따라 최종 합성 (안전장치)
                return lerp(original, finalEdge, mask.r);
            }
            ENDCG
        }
        }
}