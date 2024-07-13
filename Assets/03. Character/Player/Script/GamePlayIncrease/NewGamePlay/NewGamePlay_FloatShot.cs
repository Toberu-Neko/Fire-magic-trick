using UnityEngine;
using UnityEngine.Playables;

public class NewGamePlay_FloatShot : NewGamePlay_Basic_FloatShot
{
    //Script
    private NGP_Shot shot;
    private PlayerState playerState;
    [SerializeField] private AudioSource S_floatShot;
    [Space(10)]
    [SerializeField] private int eachShotCount;
    [SerializeField] private float angle_X;
    [SerializeField] private float angle_Y;
    [SerializeField] private float bulletTime_time;

    private void Awake()
    {
        shot = GetComponent<NGP_Shot>();
    }
    protected override void Start()
    {
        base.Start();
        playerState = GameManager.Instance.Player.GetComponent<PlayerState>();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void floatShot()
    {
        base.floatShot();
        float x = Random.Range(-angle_X, angle_X);
        float y = Random.Range(-angle_Y, angle_Y);

        for (int i = 0; i < eachShotCount; i++)
        {
            shot.Shot(x, y);
        }
    }
    protected override void OnFloatShotStart()
    {
        base.OnFloatShotStart();

        BulletTimeManager.Instance.BulletTime_Slow();
        S_floatShot.Play();
    }

    protected override void OnFloatEnd()
    {
        base.OnFloatEnd();
        playerState.SetGravityToNormal();
        BulletTimeManager.Instance.TimeScaleOne();
        S_floatShot.Stop();
    }

    protected override void OnFloatShotStop()
    {
        base.OnFloatShotStop();

        BulletTimeManager.Instance.TimeScaleOne();
    }
}
