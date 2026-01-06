// Global 변수 선언 - C#에서 설정한 변수와 일치해야 함
//현재 파장 갯수
int _PulseCount;
//파장의 시작위치,w값은 파장의 빛 세기
float4 _PulsePositions[16]; // 배열의 수는 MAX_PULSES와 일치해야 함
//파장의 반지름
float _PulseRadii[16];
//파장의 두께
float _PulseLineWidth[16];


// 커스텀 펑션의 이름은 GetFinalMask_float 등으로 자동 생성
//WorldPos = 쉐이더가 적용된 오브젝트의 월드포지션 입력
//PowerExponent = Power 변수 입력
//FinalMask =최종 합산 마스크
void GetFinalMask_float(float3 WorldPos,float _PowerExponent,float _TaliLength, out float FinalMask)
{
    // 최종 마스크를 0으로 초기화 (Emission 합산의 기본값)
    FinalMask = 0.0;
    
    // 파장 개수만큼 반복
    for (int i = 0; i < _PulseCount; i++)
    {
        //현재 파장 정보
        float3 PulseCenter = _PulsePositions[i].xyz;//위치
        float LightPower = _PulsePositions[i].w;//파장의 빛 세기
        float PulseRadius = _PulseRadii[i];//반지름
        float PulseLineWidth = _PulseLineWidth[i];//두께

        // 파장과의 거리 계산 = 쉐이더적용오브젝트에서 파장중심까지의 거리
        float DistanceToCenter = distance(WorldPos, PulseCenter);
        
        // 차이 계산 (Distance - Radius) = 파장의 반지름 - 파장과의 거리 = 양수(구체안쪽),음수(구체바깥쪽)
        float Difference = PulseRadius - DistanceToCenter;

        // 마스크 생성 및 초기화
        float Mask = 0;

        //파장 안쪽 = 잔상 생성
        if(Difference>=0)        
        {
                //Difference가 0(경계선)일 때 1,Difference가 커질수록(안으로 들어올수록) 0에 수렴
                Mask = saturate(1.0-(Difference / _TaliLength));
                Mask = pow(Mask,_PowerExponent);
        }
        else //파장 바깥쪽 = 외곽이 선명하게
        {
                //바깥쪽은 아주 짧게 깎기
                Mask = saturate(1.0-(abs(Difference)/PulseLineWidth));
                Mask = pow(Mask,_PowerExponent * 2.0); //파워 적용으로 더 선명하게
        }
        

        //각 파장의 빛 세기 적용
        Mask = Mask * LightPower; //Multiply

        // 합산 = Max를 사용하여 가장 밝은 마스크를 취합
        // 두 개의 마스크가 겹치면 더 밝은 쪽이 최종 마스크
        FinalMask = max(FinalMask, Mask);
    }
    
}