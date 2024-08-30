using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] Color lineColor;
    [SerializeField] List<Transform> Nodes;

    private void OnDrawGizmos()  // 좌표에 생상 선을 그린다. 아이콘, 이미지를 넣을 수있다.
    {
        Gizmos.color = lineColor;
        Transform[] pathTransforms = GetComponentsInChildren<Transform>();
        Nodes = new List<Transform>();  // 그릴때 동적 할당
        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != transform)  // 자기 자신을 빼고 하위 트랜스폼을 담는다.
            {
                Nodes.Add(pathTransforms[i]);
            }
        }

        for (int i = 0; i < Nodes.Count; i++)
        {
            Vector3 currentNode = Nodes[i].position;  // 현재 노드
            Vector3 previousNode = Vector3.zero;      //이전 노드
            if (i > 0)
            {
                previousNode = Nodes[i - 1].position;
            }
            else if (i == 0 && Nodes.Count > 1)  // i가 0과 같고 노드카운트가 1이상이면
            {
                previousNode = Nodes[Nodes.Count - 1].position;
            }

            // 좌표에 선은 이전 노드에서 현재 노드로
            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawSphere(currentNode, 1.0f);  // 현재 노드에 색상을 1만큼 넣는다.
        }
    }
}
