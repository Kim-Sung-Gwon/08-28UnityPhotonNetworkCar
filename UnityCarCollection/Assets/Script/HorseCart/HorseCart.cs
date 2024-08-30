using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseCart : MonoBehaviour
{
    [SerializeField] Transform path;
    [SerializeField] Transform tr;
    [SerializeField] Transform[] pathTransforms;
    [SerializeField] List<Transform> pathList;
    private int currentNode = 0;
    private float timePrew = 0f;

    void Start()
    {
        tr = transform;
        path = GameObject.Find("PathTransforms").transform;
        pathTransforms = path.GetComponentsInChildren<Transform>();
        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path)
                pathList.Add(pathTransforms[i]);
        }
        timePrew = Time.time;
    }

    void Update()
    {
        if (Time.time - timePrew > 3f)
        {
            HorseMove();
        }
        HorseCarWayPoint();
    }

    void HorseMove()
    {
        Quaternion rot = Quaternion.LookRotation(pathList[currentNode].position - path.position);
        path.rotation = Quaternion.Slerp(path.rotation, rot, Time.deltaTime * 15f);
        path.Translate(Vector3.forward * Time.deltaTime * 5f);
    }

    void HorseCarWayPoint()
    {
        if (Vector3.Distance(path.position, pathList[currentNode].position) <= 3.0f)
        {
            if (currentNode == pathList.Count - 1)
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
