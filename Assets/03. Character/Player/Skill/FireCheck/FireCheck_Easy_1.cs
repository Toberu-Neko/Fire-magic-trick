using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCheck : MonoBehaviour
{
    public float distance = 10f; // �^����x���C�ľ��x
    public float angle = 180f; // ��A�εĽǶ�
    public float size = 5f; // �^��Ĵ�С
    public LayerMask detectionMask; // �z�y�����LayerMask

    private List<Renderer> highlightedRenderers = new List<Renderer>();

    private void Update()
    {
        // ȡ�����C��������g�е�λ�ú���ǰ������
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;

        // Ӌ���A�΅^�������λ��
        Vector3 center = cameraPosition + cameraForward * distance;

        // ֻ�z�y�^��ȵ����
        Collider[] colliders = Physics.OverlapSphere(center, size, detectionMask);

        // ���殔ǰ��ͻ���@ʾ�����
        List<Renderer> currentHighlightedRenderers = new List<Renderer>();

        // �z��ÿ���z�y�������
        foreach (Collider collider in colliders)
        {
            // �������λ���D�Q�����C���g�е�����
            Vector3 colliderPosition = Camera.main.WorldToViewportPoint(collider.transform.position);

            // �z�������Ƿ��ڰ�A�ι�����
            if (colliderPosition.y <= 0.5f && colliderPosition.x >= 0.25f && colliderPosition.x <= 0.75f)
            {
                // �z��˻`�Ƿ�� "FirePoint"
                if (collider.CompareTag("FirePoint"))
                {
                    Debug.Log("FirePoint detected!");

                    // �@ȡ����� Renderer �M��
                    Renderer renderer = collider.GetComponent<Renderer>();

                    // �z���Ƿ���� Renderer �M���K���п��O�õĲ��|
                    if (renderer != null && renderer.material != null)
                    {
                        // �����|�ɫ�O�Þ�tɫ
                        renderer.material.color = Color.red;

                        // ����ͻ���@ʾ����������б���
                        currentHighlightedRenderers.Add(renderer);
                    }
                }
            }
        }

        // ���x�_�z�y����������֏͞��ɫ
        foreach (Renderer highlightedRenderer in highlightedRenderers)
        {
            if (!currentHighlightedRenderers.Contains(highlightedRenderer))
            {
                // �z���Ƿ���� Renderer �M���K���п��O�õĲ��|
                if (highlightedRenderer != null && highlightedRenderer.material != null)
                {
                    // �����|�ɫ�O�Þ��ɫ
                    highlightedRenderer.material.color = Color.white;
                }
            }
        }

        // ���®�ǰ��ͻ���@ʾ������б�
        highlightedRenderers = currentHighlightedRenderers;
    }
}
