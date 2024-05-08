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
        // 射線偵測打中障礙物時
        Ray ray = new Ray(aimmingLinePoint.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxLength, obstacleLayer))
        {
            //Debug.Log(hit.collider.gameObject);
            // 計算擊中障礙物的點與發射位置距離
            Vector3 hitPoint = hit.point;
            float distanceToPoint = Vector3.Distance(aimmingLinePoint.position, hitPoint);

            // 調整碰撞體長度
            collier.transform.position = aimmingLinePoint.position + aimmingLinePoint.forward * distanceToPoint / 2;
            collier.transform.localScale = new Vector3(collier.transform.localScale.x, collier.transform.localScale.y, distanceToPoint);
        }else
        {
            collier.transform.position = aimmingLinePoint.position + aimmingLinePoint.forward * maxLength / 2;
            collier.transform.localScale = new Vector3(collier.transform.localScale.x, collier.transform.localScale.y, maxLength);
        }
    }
}
