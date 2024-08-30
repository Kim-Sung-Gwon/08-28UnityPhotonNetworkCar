using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 1. 무게 중심   2. pathTransforms 참조해서 이동
// 3. 타이어 휠 콜라이더 배치 타이어 모델링 배치
// 4. 스피드
public class AICar : MonoBehaviour
{
    [Header("CentOfMass")]
    [SerializeField] Rigidbody rb;
    public Vector3 CentOfMass = new Vector3(0f, -0.5f, 0f); // 무게중심

    [Header("Path")]
    [SerializeField] Transform path;
    [SerializeField] Transform[] pathTransforms; // 이동하는 위치
    [SerializeField] List<Transform> pathList;

    [Header("WheelCollider")]
    [SerializeField] WheelCollider FL;
    [SerializeField] WheelCollider FR;
    [SerializeField] WheelCollider BL;
    [SerializeField] WheelCollider BR;

    [Header("Modeling")]
    [SerializeField] Transform FLTr;
    [SerializeField] Transform FRTr;
    [SerializeField] Transform BLTr;
    [SerializeField] Transform BRTr;

    [Header("Speed")]
    public float currentSpeed = 0f;       // 현재 스피드
    private int currentNode = 0;          // 현재 노드
    private float maxSpeed = 150f;        // 최대 속도
    public float maxMotorTorque = 700f;  // 휠 콜라이더가 회전하는 힘
    public float maxSteerAngle = 30f;     // 앞바퀴 회전 각

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CentOfMass;
        path = GameObject.Find("PathTransforms").transform;
        pathTransforms = path.GetComponentsInChildren<Transform>();
        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path)
                pathList.Add(pathTransforms[i]);
        }
    }

    void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWayPointDistance();
    }

    void ApplySteer()  // 앞 바퀴의 휠 콜라이더가 path에 따라서 회전하는 메서드
    {
        Vector3 relativeVector = transform.InverseTransformPoint(pathList[currentNode].position);
        //      실제적인 방향  = 윌드 좌표(게임상의 좌표)을(를) 로컬 좌표로 변환.
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        //              (패스 트랜스폼.x값 / 패스 트랜스폼.개체크기  ) * 30      ;
        
        FL.steerAngle = newSteer;
        FR.steerAngle = newSteer;
        // 휠 콜라이더 앵글 각이 결정된다.
    }

    void Drive()
    {
        // 차가 코너링 할때 속도를 감속 하는 것도 포함
        currentSpeed = 2 * Mathf.PI * FL.radius * FL.rpm * 60 / 1000;
        #region currentSpeed = 2 * Mathf.PI * FL.radius * FL.rpm * 60 / 1000; 공식 설명
        //currentSpeed: 계산된 순간 속도를 저장할 변수입니다.
        //일반적으로 미터/ 초(m / s) 단위로 표현됩니다.

        //2 * Mathf.PI: 원의 둘레를 구하는 공식의 일부입니다.
        //2π는 원의 둘레를 반지름으로 나눈 값(원주율)을 의미합니다.

        //FL.radius: 원형 운동의 반지름입니다.

        //FL.rpm: 분당 회전수(Revolutions Per Minute)입니다.
        //즉, 1분 동안 몇 바퀴를 도는지를 나타냅니다.

        //60: 분을 초로 변환하기 위한 값입니다.rpm을 초당 회전수로 바꾸기 위해 사용됩니다.

        //1000: 미터로 변환하기 위한 값입니다.
        //일반적으로 반지름은 미터 단위로 설정되므로,
        //결과를 미터/ 초 단위로 만들기 위해 사용됩니다.
        #endregion

        if (currentSpeed < maxSpeed)  // 현재 스피드 < 최고 스피드
        {
            BL.motorTorque = maxMotorTorque;
            BR.motorTorque = maxMotorTorque;
        }
        else  // 현재 스피드 > 최고 스피드
        {
            BL.motorTorque = 0f;
            BR.motorTorque = 0f;
        }
    }

    void CheckWayPointDistance()
    {
        //                                                   현재 거리, 도착지점 <= 3.5
        if (Vector3.Distance(transform.position, pathList[currentNode].position) <= 3.5f)
        {
            if (currentNode == pathList.Count - 1)  // 마지막에 왔을 때 다시 0으로 초기화
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }
}
