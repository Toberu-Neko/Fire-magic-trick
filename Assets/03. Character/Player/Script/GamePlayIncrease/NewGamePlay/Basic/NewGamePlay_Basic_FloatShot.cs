using UnityEngine;

public class NewGamePlay_Basic_FloatShot : MonoBehaviour
{
    private FireFloat fireFloat;
    private ControllerInput input;

    public bool isFloat = false;
    private bool isShot = false;
    private bool isFloatShot = false;

    [Header("Setting")]
    [SerializeField] private float shotInterval = 0.1f;

    private float timer = 0f;
    protected virtual void Start()
    {
        fireFloat = GameManager.Instance.EnergySystem.GetComponent<FireFloat>();
        input = GameManager.Instance.Player.GetComponent<ControllerInput>();

        fireFloat.OnFloatStart += OnFloatStart;
        fireFloat.OnFloatEnd += OnFloatEnd;
        
    }
    protected virtual void Update()
    {
        shotTimer();
        floatShotSystem();
    }
    private void shotTimer()
    {
        if (isShot && isFloat)
        {
            timer += Time.deltaTime;
        }

        if(timer >= shotInterval)
        {
            SetIsShot(false);
            timer = 0;
        }
    }
    private void floatShotSystem()
    {
        if (input.RT && isFloat)
        {
            if(!isShot)
            {
                floatShot();

                if(!isFloatShot)
                {
                    OnFloatShotStart();
                    SetIsFloatShot(true);
                }
            }
        }else
        {
            if(isFloatShot)
            {
                SetIsFloatShot(false);
                OnFloatShotStop();
            }
        }

        if(!input.RT)
        {
            if(isFloatShot)
            {
                SetIsFloatShot(false);
                OnFloatShotStop();
            }
        }
    }
    protected virtual void OnFloatShotStart() { }
    protected virtual void OnFloatShotStop() { }
    protected virtual void floatShot()
    { 
        SetIsShot(true);
    }
    protected virtual void OnFloatStart()
    {
        SetIsFloat(true);
    }
    protected virtual void OnFloatEnd()
    {
        SetIsFloat(false);
    }
    private void SetIsFloat(bool value)
    {
        isFloat = value;
    }
    private void SetIsShot(bool Value)
    {
        isShot = Value;
    }
    private void SetIsFloatShot(bool value)
    {
        isFloatShot = value;
    }
}
