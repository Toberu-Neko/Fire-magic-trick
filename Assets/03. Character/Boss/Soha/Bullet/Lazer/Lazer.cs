using UnityEngine;

public class Lazer : MonoBehaviour
{
    [SerializeField] private Transform aimmingLinePoint;
    [SerializeField] private float maxLength;
    [SerializeField] private LayerMask obstacleLayer;
    private Collider collier;

    private void Awake()
    {
        collier = GetComponent<Collider>();
    }
    private void Update()
    {
        LazerRangeSetting();
    }
    private void LazerRangeSetting()
    {
        // �g�u����������ê����
        Ray ray = new Ray(aimmingLinePoint.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxLength, obstacleLayer))
        {
            //Debug.Log(hit.collider.gameObject);
            // �p��������ê�����I�P�o�g��m�Z��
            Vector3 hitPoint = hit.point;
            float distanceToPoint = Vector3.Distance(aimmingLinePoint.position, hitPoint);

            // �վ�I�������
            collier.transform.position = aimmingLinePoint.position + aimmingLinePoint.forward * distanceToPoint / 2;
            collier.transform.localScale = new Vector3(collier.transform.localScale.x, collier.transform.localScale.y, distanceToPoint);
        }else
        {
            collier.transform.position = aimmingLinePoint.position + aimmingLinePoint.forward * maxLength / 2;
            collier.transform.localScale = new Vector3(collier.transform.localScale.x, collier.transform.localScale.y, maxLength);
        }
    }
}
