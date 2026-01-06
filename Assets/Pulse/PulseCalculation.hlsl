// Global 변수 선언 - C#에서 SetGlobal로 설정한 변수와 일치해야 함
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
void GetFinalMask_float(float3 WorldPos,float _PowerExponent, out float FinalMask)
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
        
        // 차이 계산 (Distance - Radius) = 파장과의 거리- 파장의 반지름
        float Difference = DistanceToCenter -  PulseRadius;

        // 마스크에 계산된 차이 적용
        float Mask = abs(Difference);
        
        // LineWidth 적용 = 두께 적용
        Mask = saturate(Mask - PulseLineWidth); // Substract LineWidth 후 Saturate
        Mask = 1.0 - Mask;                  // One Minus = 반전
        
        // Power를 이용한 날카로움 조절 = 잔상 제거
        Mask = pow(Mask, _PowerExponent); 

        //step적용
        Mask = step(0.5,Mask);

        //파장의 빛 세기 적용
        Mask = Mask * LightPower; //Multiply

        // 합산 = Max를 사용하여 가장 밝은 마스크를 취합
        // 두 개의 마스크가 겹치면 더 밝은 쪽이 최종 마스크
        FinalMask = max(FinalMask, Mask);
    }
}