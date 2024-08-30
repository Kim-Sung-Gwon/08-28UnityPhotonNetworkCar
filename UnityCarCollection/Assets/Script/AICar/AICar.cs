using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 1. ���� �߽�   2. pathTransforms �����ؼ� �̵�
// 3. Ÿ�̾� �� �ݶ��̴� ��ġ Ÿ�̾� �𵨸� ��ġ
// 4. ���ǵ�
public class AICar : MonoBehaviour
{
    [Header("CentOfMass")]
    [SerializeField] Rigidbody rb;
    public Vector3 CentOfMass = new Vector3(0f, -0.5f, 0f); // �����߽�

    [Header("Path")]
    [SerializeField] Transform path;
    [SerializeField] Transform[] pathTransforms; // �̵��ϴ� ��ġ
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
    public float currentSpeed = 0f;       // ���� ���ǵ�
    private int currentNode = 0;          // ���� ���
    private float maxSpeed = 150f;        // �ִ� �ӵ�
    public float maxMotorTorque = 700f;  // �� �ݶ��̴��� ȸ���ϴ� ��
    public float maxSteerAngle = 30f;     // �չ��� ȸ�� ��

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

    void ApplySteer()  // �� ������ �� �ݶ��̴��� path�� ���� ȸ���ϴ� �޼���
    {
        Vector3 relativeVector = transform.InverseTransformPoint(pathList[currentNode].position);
        //      �������� ����  = ���� ��ǥ(���ӻ��� ��ǥ)��(��) ���� ��ǥ�� ��ȯ.
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        //              (�н� Ʈ������.x�� / �н� Ʈ������.��üũ��  ) * 30      ;
        
        FL.steerAngle = newSteer;
        FR.steerAngle = newSteer;
        // �� �ݶ��̴� �ޱ� ���� �����ȴ�.
    }

    void Drive()
    {
        // ���� �ڳʸ� �Ҷ� �ӵ��� ���� �ϴ� �͵� ����
        currentSpeed = 2 * Mathf.PI * FL.radius * FL.rpm * 60 / 1000;
        #region currentSpeed = 2 * Mathf.PI * FL.radius * FL.rpm * 60 / 1000; ���� ����
        //currentSpeed: ���� ���� �ӵ��� ������ �����Դϴ�.
        //�Ϲ������� ����/ ��(m / s) ������ ǥ���˴ϴ�.

        //2 * Mathf.PI: ���� �ѷ��� ���ϴ� ������ �Ϻ��Դϴ�.
        //2��� ���� �ѷ��� ���������� ���� ��(������)�� �ǹ��մϴ�.

        //FL.radius: ���� ��� �������Դϴ�.

        //FL.rpm: �д� ȸ����(Revolutions Per Minute)�Դϴ�.
        //��, 1�� ���� �� ������ �������� ��Ÿ���ϴ�.

        //60: ���� �ʷ� ��ȯ�ϱ� ���� ���Դϴ�.rpm�� �ʴ� ȸ������ �ٲٱ� ���� ���˴ϴ�.

        //1000: ���ͷ� ��ȯ�ϱ� ���� ���Դϴ�.
        //�Ϲ������� �������� ���� ������ �����ǹǷ�,
        //����� ����/ �� ������ ����� ���� ���˴ϴ�.
        #endregion

        if (currentSpeed < maxSpeed)  // ���� ���ǵ� < �ְ� ���ǵ�
        {
            BL.motorTorque = maxMotorTorque;
            BR.motorTorque = maxMotorTorque;
        }
        else  // ���� ���ǵ� > �ְ� ���ǵ�
        {
            BL.motorTorque = 0f;
            BR.motorTorque = 0f;
        }
    }

    void CheckWayPointDistance()
    {
        //                                                   ���� �Ÿ�, �������� <= 3.5
        if (Vector3.Distance(transform.position, pathList[currentNode].position) <= 3.5f)
        {
            if (currentNode == pathList.Count - 1)  // �������� ���� �� �ٽ� 0���� �ʱ�ȭ
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
