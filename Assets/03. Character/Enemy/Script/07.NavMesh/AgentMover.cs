using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AgentMover : MonoBehaviour
{
    public event Action<float> OnSpeedChanged;

    private bool _onNavMeshLink = false;
    private NavMeshAgent _Agent;

    public UnityEvent OnLand, OnStartJump;

    private void Start()
    {
        if(GetComponent<NavMeshAgent>() != null)_Agent = GetComponent<NavMeshAgent>();
        else Debug.Log(gameObject.name + "Without Nav Mesh Agent");

        _Agent.autoTraverseOffMeshLink = false;
    }


    public void SetDestination(Vector3 destination)
    {
        if (_onNavMeshLink)
            return;

        _Agent.destination = destination;
    }

    private void Update()
    {
        OnSpeedChanged?.Invoke(Mathf.Clamp01(_Agent.velocity.magnitude / _Agent.speed));

        if (_Agent.isOnOffMeshLink && _onNavMeshLink == false)
        {
            StartNavMeshLinkMovement();
        }
        if (_onNavMeshLink)
        {
            FaceTarget(_Agent.currentOffMeshLinkData.endPos);
        }
    }

    private void StartNavMeshLinkMovement()
    {
        _onNavMeshLink = true;
        NavMeshLink link = (NavMeshLink)_Agent.navMeshOwner;
        Spline spline = link.GetComponentInChildren<Spline>();
        float jumpDuration = link.GetComponentInChildren<NavMeshLinkSpline>().jumpDuration;

        PerformJump(link, spline, jumpDuration);
    }

    private void PerformJump(NavMeshLink link, Spline spline, float jumpDuration)
    {
        bool reverseDirection = CheckIfJumpingFromEndToStart(link);
        StartCoroutine(MoveOnOffMeshLink(spline, reverseDirection, jumpDuration));

        OnStartJump?.Invoke();
    }

    private bool CheckIfJumpingFromEndToStart(NavMeshLink link)
    {
        Vector3 startPosWorld
            = link.gameObject.transform.TransformPoint(link.startPoint);
        Vector3 endPosWorld
            = link.gameObject.transform.TransformPoint(link.endPoint);

        float distancePlayerToStart
            = Vector3.Distance(_Agent.transform.position, startPosWorld);
        float distancePlayerToEnd
            = Vector3.Distance(_Agent.transform.position, endPosWorld);


        return distancePlayerToStart > distancePlayerToEnd;
    }

    private IEnumerator MoveOnOffMeshLink(Spline spline, bool reverseDirection, float jumpDuration)
    {
        float currentTime = 0;
        Vector3 agentStartPosition = _Agent.transform.position;

        while (currentTime < jumpDuration)
        {
            currentTime += Time.deltaTime;

            float amount = Mathf.Clamp01(currentTime / jumpDuration);
            amount = reverseDirection ? 1 - amount : amount;

            _Agent.transform.position =
                reverseDirection ?
                spline.CalculatePositionCustomEnd(amount, agentStartPosition)
                : spline.CalculatePositionCustomStart(amount, agentStartPosition);

            yield return new WaitForEndOfFrame();
        }

        _Agent.CompleteOffMeshLink();

        OnLand?.Invoke();
        yield return new WaitForSeconds(0.1f);
        _onNavMeshLink = false;

    }


    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation
            = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation
            = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

}