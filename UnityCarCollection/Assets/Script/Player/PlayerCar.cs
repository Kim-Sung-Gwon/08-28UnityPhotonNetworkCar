using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCar : MonoBehaviourPun, IPunObservable
{
    [Header("WheelCollider")]
    public WheelCollider frontLeft_Col;
    public WheelCollider frontRight_Col;
    public WheelCollider backLeft_Col;
    public WheelCollider backRight_Col;
    
    [Header("Wheel Models")]
    public Transform frontLeft_M;
    public Transform frontRight_M;
    public Transform backLeft_M;
    public Transform backRight_M;

    [Header("Mass Blance")] // ���� �뷱�� ���
    public Vector3 CentOfMass = new Vector3(0f, -0.5f, 0f);
    // �ڵ����� ������ ������ �ִ� ���� ���� �߽��� y������ �׻� -0.5�� ��ƾ� ��
    public Rigidbody rb;

    [Header("�չ��� �ִ� ȸ����")]
    public float maxSteer = 35f;

    [Header("�ִ� ������")]
    public float maxTorque = 3000f;  // �丣ũ��� �Ҹ���.

    [Header("�ִ� �극��ũ")]
    public float maxBrake = 500000000f;

    [Header("���� ���ǵ�")]
    public float currentSpeed = 0f;
    private float Steer = 0f;    // A, D Ű���� ���� ���� ������� �뵵
    private float forward = 0f;  // WŰ�� �޾Ƽ� �����ϴ� ����
    private float back = 0f;     // SŰ�� �޾Ƽ� �����ϴ� ����
    bool isrevers = false;       // �������� �������� �Ǵ��ϴ� ����
    private float motor = 0f;    // �����̸� ���� ����̸� ����
    private float brake = 0f;    // �극��ũ

    //private GetInOut inOut;

    private Transform tr;

    private Vector3 pos = Vector3.zero;
    private Quaternion rot = Quaternion.identity;

    void Start()
    {
        photonView.Synchronization = ViewSynchronization.ReliableDeltaCompressed;
        photonView.ObservedComponents[0] = this;  // �ڽ� ��ũ��Ʈ�� ã�� ����ȭ

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        if (photonView.IsMine)
        {
            rb.centerOfMass = CentOfMass;
        }
        //inOut = transform.GetChild(5).GetComponent<GetInOut>();
    }

    // ���� ������ ��Ȯ�� �������̳� ��Ȯ�� �ð��� ���� �����̴�
    // ������ ���� �Ϸ��� Update >> FixedUpdate ����
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.B))
            {
                Brake();
            }
            else
                Carmove();
        }
    }

    public void Brake()
    {
        frontLeft_Col.motorTorque = 0f;
        frontRight_Col.motorTorque = 0f;
        frontLeft_Col.brakeTorque = 500000000000f;
        frontRight_Col.brakeTorque = 500000000000f;
        backLeft_Col.brakeTorque = 500000000000f;
        backRight_Col.brakeTorque = 500000000000f;
    }

    private void Carmove()
    {
        frontLeft_Col.brakeTorque = 0f;
        frontRight_Col.brakeTorque = 0f;
        backLeft_Col.brakeTorque = 0f;
        backRight_Col.brakeTorque = 0f;
        currentSpeed = rb.velocity.sqrMagnitude;
        // �� �ݶ��̴��� �����¿� ���� ������ �ٵ��� ��ü �ӵ�(�̷�)�� ����

        Steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1f, 1f);

        // WŰ�� ������ �Ѵ�.
        forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0f, 1f);

        // SŰ�� ������ �Ѵ�.
        back = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1f, 0f);

        if (Input.GetKey(KeyCode.W))
        {
            StartCoroutine(ForWardCar());
        }
        if (Input.GetKey(KeyCode.S))
        {
            StartCoroutine(BackWardCar());
        }

        if (isrevers)  // ������ �̶��
        {
            motor = -1 * back;
            brake = forward;
        }
        else
        {
            motor = forward;
            brake = back;
        }

        frontLeft_Col.motorTorque = motor * maxTorque;
        frontRight_Col.motorTorque = motor * maxTorque;

        // �޹��� �丣ũ ȸ����
        backLeft_Col.motorTorque = motor * maxTorque;
        backRight_Col.motorTorque = motor * maxTorque;
        // �޹��� �극��ũ
        backLeft_Col.brakeTorque = brake * maxBrake;
        backRight_Col.brakeTorque = brake * maxBrake;

        // �չ��� y�� ȸ�� ����
        frontLeft_Col.steerAngle = maxSteer * Steer;
        frontRight_Col.steerAngle = maxSteer * Steer;

        // A, D ������ �� y�� ȸ��
        frontLeft_M.localEulerAngles =
            new Vector3(frontLeft_M.localEulerAngles.x, Steer * maxSteer,
            frontLeft_M.localEulerAngles.z);
        frontRight_M.localEulerAngles =
            new Vector3(frontRight_M.localEulerAngles.x, Steer * maxSteer,
            frontRight_M.localEulerAngles.z);

        // �𵨸� ȸ�� ���ݶ��̴��� ȸ�� �丣ũ ���� �޾Ƽ� ���� ȸ��
        frontLeft_M.Rotate(frontLeft_Col.rpm * Time.deltaTime, 0f, 0f);
        frontRight_M.Rotate(frontRight_Col.rpm * Time.deltaTime, 0f, 0f);
        backLeft_M.Rotate(backLeft_Col.rpm * Time.deltaTime, 0f, 0f);
        backRight_M.Rotate(backRight_Col.rpm * Time.deltaTime, 0f, 0f);
    }

    IEnumerator ForWardCar()
    {
        yield return new WaitForSeconds(0.1f);
        currentSpeed = 0f;
        if (back > 0f) isrevers = true;
        if (forward > 0f) isrevers = false;
    }

    IEnumerator BackWardCar()
    {
        yield return new WaitForSeconds(0.1f);
        currentSpeed = 0.1f;
        if (back > 0f) isrevers = true;
        if (forward > 0f) isrevers = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            pos = (Vector3)stream.ReceiveNext();
            rot = (Quaternion)stream.ReceiveNext();
        }
    }
}
