using UnityEngine;

public class TeachFloat : MonoBehaviour
{
    [SerializeField] private GameObject UI_SuperJump;
    [SerializeField] private GameObject DashState;
    [SerializeField] private GameObject JumpWeakest;
    [SerializeField] private GameObject ElementSkill;

    public enum types
    {
        SuperJump,
        DashState,
        JumpWeakest,
        ElementSkill,
    }
    public void Open(types type)
    {
        gameObject.SetActive(true);
        ChooseObj(type, true);
    }
    public void Close(types type)
    {
        gameObject.SetActive(false);
        ChooseObj(type, false);
    }
    private void ChooseObj(types type, bool activate)
    {
        switch (type)
        {
            case types.SuperJump:
                UI_SuperJump.SetActive(activate);
                DashState.SetActive(false);
                JumpWeakest.SetActive(false);
                ElementSkill.SetActive(false);
                break;
            case types.DashState:
                UI_SuperJump.SetActive(false);
                DashState.SetActive(activate);
                JumpWeakest.SetActive(false);
                ElementSkill.SetActive(false);
                break;
            case types.JumpWeakest:
                UI_SuperJump.SetActive(false);
                DashState.SetActive(false);
                JumpWeakest.SetActive(activate);
                ElementSkill.SetActive(false);
                break;
            case types.ElementSkill:
                UI_SuperJump.SetActive(false);
                DashState.SetActive(false);
                JumpWeakest.SetActive(false);
                ElementSkill.SetActive(activate);
                break;
        }
    }
}
