using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class OrganStar : MonoBehaviour
{
    [MMReadOnly] public bool isCatapult;
    [MMReadOnly][SerializeField][Range(0, 1)] private float timer;

    [Header("Star")]
    [SerializeField] private GameObject Star;
    [SerializeField] private float speed;
    [Header("Transform")]
    [MMReadOnly][SerializeField] private Transform target;
    [SerializeField] private Transform origin;
    [SerializeField] private Transform end;
    [Header("Breaking")]
    [SerializeField] private ShieldSystem shieldSystem;
    [SerializeField] private OrganCan organCan;
    [Header("Feedback")]
    [SerializeField] private MMF_Player hitShield;

    //value
    private Vector3 direction;
    private Vector3 go;
    private Vector3 back;
    private bool isMove;

    private void Start()
    {
        origin = this.transform;

        go = (end.position - origin.position).normalized;
        back = (origin.position - end.position).normalized;
        direction = go;
    }
    public void Update()
    {
        move();
        stop();
    }
    public void Catapult()
    {
        setIsMove(true);
        setTarget(end);
        direction = go;
    }
    public void Recycle()
    {
        setIsMove(true);
        setTarget(origin);
        direction = back;

        shieldSystem.OpenShield();
    }
    private void move()
    {
        if(isMove)
        {
            Star.transform.position += direction * speed * Time.deltaTime;
        }
    }
    private void stop()
    {
        if(isMove)
        {
            float length = (Star.transform.position - target.position).magnitude;

            if (length < 1.5f)
            {
                setIsMove(false);
                
                if(!isCatapult)
                {
                    shieldSystem.CloseShield();
                    setIsCatapult(true);
                    hitShield.PlayFeedbacks();
                }
                else
                {
                    organCan.Initialization();
                    setIsCatapult(false);
                }
            }
        }
    }
    private void setTarget(Transform target)
    {
        this.target = target;
    }
    private void setIsMove(bool active)
    {
        isMove = active;
    }
    private void setIsCatapult(bool active)
    {
        isCatapult = active;
    }
}
