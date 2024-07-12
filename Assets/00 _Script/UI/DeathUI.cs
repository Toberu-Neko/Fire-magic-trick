using System;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public bool FinishedOpenAnim { get; private set; }

    private void OnEnable()
    {
        FinishedOpenAnim = false;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("open");
    }

    public void Deactivate()
    {
        animator.SetTrigger("close");
        FinishedOpenAnim = false;
    }

    public void StartAnimationComplete()
    {
        FinishedOpenAnim = true;
    }

    public void StopAnimationComplete()
    {
        gameObject.SetActive(false);
    }
}
