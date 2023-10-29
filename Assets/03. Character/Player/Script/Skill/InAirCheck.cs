using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirCheck : MonoBehaviour
{
    [SerializeField] private float AirLengh;
    public bool InAir;

    private void Update()
    {
        Raycheck();
    }

    private void Raycheck()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, AirLengh))
        {
            Debug.Log("��?���������w��" + hit.collider.gameObject.name);
            InAir = false;
        }else
        {
            InAir = true;
        }
    }
    private void OnDrawGizmos()
    {
        // �O�� Gizmos ��?ɫ
        Gizmos.color = Color.yellow;

        // �L�u��?�����c�����wλ�ã����������
        Gizmos.DrawRay(transform.position, Vector3.down * AirLengh);
    }
}
