using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;// List를 배열로 변환하기 위해 필요

public class PulseManager : MonoBehaviour
{
    // 셰이더 배열의 최대 크기 (GPU 오버헤드를 줄이기 위해 제한 = 성능저하방지)
    public const int MAX_PULSES = 16;

    // 셰이더 프로퍼티 ID
    private int _PulseCountID;
    private int _PulsePositionsID;
    private int _PulseRadiiID;
    private int _PulseLineWidthID;

    //셰이더 프로퍼티에 전달할 내용
    private Vector4[] _pulsePositions = new Vector4[MAX_PULSES];
    private float[] _pulseRadii = new float[MAX_PULSES];
    private float[] _pulseLineWidth = new float[MAX_PULSES];


    void Start()
    {
        // 셰이더 프로퍼티 ID는 HLSL 코드와 일치
        _PulseCountID = Shader.PropertyToID("_PulseCount");
        _PulsePositionsID = Shader.PropertyToID("_PulsePositions");
        _PulseRadiiID = Shader.PropertyToID("_PulseRadii");
        _PulseLineWidthID = Shader.PropertyToID("_PulseLineWidth");
    }

    // Update is called once per frame
    void Update()
    {
        // 씬의 모든 활성화된 PulseEffectController를 찾기
        PulseEffectController[] pulses = FindObjectsOfType<PulseEffectController>();
        int count = Mathf.Min(pulses.Length, MAX_PULSES); // 최대 개수 제한

        for (int i = 0; i < count; i++)
        {
            // Vector4의 XYZ에 파장의 시작 위치, w는 빛의 세기
            _pulsePositions[i] = pulses[i].transform.position;
            _pulsePositions[i].w = pulses[i].LightPower;
            //파장의 반지름
            _pulseRadii[i] = pulses[i].CurrentScale;
            //파장의 두께
            _pulseLineWidth[i] = pulses[i].LineWidth;
        }

        // 셰이더의 전역 배열 프로퍼티에 정보를 전달
        Shader.SetGlobalInt(_PulseCountID, count);
        Shader.SetGlobalVectorArray(_PulsePositionsID, _pulsePositions);
        Shader.SetGlobalFloatArray(_PulseRadiiID, _pulseRadii);
        Shader.SetGlobalFloatArray(_PulseLineWidthID, _pulseLineWidth);
    }
}
