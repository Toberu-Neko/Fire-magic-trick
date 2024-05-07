using MoreMountains.Tools;
using UnityEngine;

public class OrganStar : MonoBehaviour
{
    [MMReadOnly] public bool isCatapult;
    [MMReadOnly][SerializeField][Range(0, 1)] private float timer;

    [Header("Star")]
    [SerializeField] private GameObject Star;
    [Header("AnimationCurve")]
    [SerializeField] private float duration = 1;
    [SerializeField] private AnimationCurve go;
    [SerializeField] private AnimationCurve back;
    [Header("Transform")]
    [SerializeField] private Transform origin;
    [SerializeField] private Transform end;

    //value
    private bool isTimer;

    private void Start()
    {
        origin = this.transform;
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            catapult();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            recycle();
        }
        animationTimer();
        move();
    }
    private void animationTimer()
    {
        
        if(isTimer)
        {
            if(isCatapult)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    timer = 0;
                    isTimer = false;
                }
            }
            else
            {
                timer += Time.deltaTime;

                if (timer >= duration)
                {
                    timer = duration;
                    isTimer = false;
                }
            }
        }
    }
    private void move()
    {
        Vector3 _direction = end.position - origin.position;
        float lengh = _direction.magnitude;
        Vector3 direction = _direction.normalized;

        float speed = lengh / duration;
        float muti = 1 / duration;
        Vector3 target = Vector3.zero;

        if (isCatapult)
        {
            target = origin.position + direction * speed * back.Evaluate(timer * muti);
        }
        else
        {
            target = origin.position + direction * speed * go.Evaluate(timer * muti);
        }
        Star.transform.position = target;
    }
    private void catapult()
    {
        isCatapult = true;
        isTimer = true;
    }
    private void recycle()
    {
        isTimer = true;
        isCatapult = false;
    }
}
