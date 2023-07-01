#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FireCheck))]
public class FireCheckEditor : Editor
{
    private void OnSceneGUI()
    {
        FireCheck fireCheck = (FireCheck)target;

        // �L�u�^�������λ��
        Handles.color = Color.white;
        Handles.DrawWireDisc(fireCheck.transform.position, fireCheck.transform.up, fireCheck.size);

        // Ӌ��^�����ʼ�ͽKֹ�Ƕ�
        float startAngle = fireCheck.transform.eulerAngles.y - fireCheck.angle / 2f;
        float endAngle = fireCheck.transform.eulerAngles.y + fireCheck.angle / 2f;

        // �D�Q�� Unity �Ļ�����
        startAngle *= Mathf.Deg2Rad;
        endAngle *= Mathf.Deg2Rad;

        // Ӌ����ʼ�ͽKֹ�c��λ��
        Vector3 startPoint = fireCheck.transform.position + Quaternion.Euler(0f, startAngle * Mathf.Rad2Deg, 0f) * fireCheck.transform.forward * fireCheck.distance;
        Vector3 endPoint = fireCheck.transform.position + Quaternion.Euler(0f, endAngle * Mathf.Rad2Deg, 0f) * fireCheck.transform.forward * fireCheck.distance;

        // �L�u�^�����ʼ�ͽKֹ�c
        Handles.color = Color.yellow;
        Handles.DrawLine(fireCheck.transform.position, startPoint);
        Handles.DrawLine(fireCheck.transform.position, endPoint);

        // ���¾��x����
        EditorGUI.BeginChangeCheck();
        float newDistance = Handles.RadiusHandle(Quaternion.identity, fireCheck.transform.position, fireCheck.distance);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(fireCheck, "Change Distance");
            fireCheck.distance = newDistance;
        }
    }
}
#endif