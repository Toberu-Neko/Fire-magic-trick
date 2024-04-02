using UnityEngine;

public class TeachFloat : MonoBehaviour
{
    [SerializeField] private GameObject UI_SuperJump;
    [SerializeField] private GameObject DashState;
    [SerializeField] private GameObject JumpWeakest;
    [SerializeField] private GameObject ElementSkill;

    private void Start()
    {
        UI_SuperJump.SetActive(false);
        DashState.SetActive(false);
        JumpWeakest.SetActive(false);
        ElementSkill.SetActive(false);
    }
    public enum types
    {
        SuperJump,
        DashState,
        JumpWeakest,
        ElementSkill,
    }
    public void Open(types type)
    {
        GameObject obj = ChooseObj(type);
        obj.SetActive(true);
    }
    public void Close(types type)
    {
        GameObject obj = ChooseObj(type);
        obj.SetActive(false);
    }
    private GameObject ChooseObj(types type)
    {
        GameObject obj = null;

        switch (type)
        {
            case types.SuperJump:
                obj = UI_SuperJump;
                break;
            case types.DashState:
                obj = DashState;
                break;
            case types.JumpWeakest:
                obj = JumpWeakest;
                break;
            case types.ElementSkill:
                obj = ElementSkill;
                break;
        }

        return obj;
    }
}
