using UnityEngine;

public class AnimationComplete : MonoBehaviour
{
    private ProgressSystem _progressSystem;

    private void Start()
    {
        _progressSystem = GameManager.Instance.GetComponent<ProgressSystem>();
    }
    public void animationComplete()
    {
        _progressSystem.PlayerDeath();
    }
}
