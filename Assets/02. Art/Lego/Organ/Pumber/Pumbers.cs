using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumbers : MonoBehaviour
{
    public bool toGetComponent;
    public bool noAuto;
    public Pumber[] pumbers;
    private void OnValidate()
    {
        if(noAuto)
        {
            return;
        }
        // �����e���骺�����l����ƶq
        int childCount = transform.childCount;
        pumbers = new Pumber[childCount];
        // �M���Ҧ������l����
        for (int i = 0; i < childCount; i++)
        {
            pumbers[i] = this.transform.GetChild(i).GetComponent<Pumber>();
        }
    }
    public void SetPumbersState(int state)
    {
        for (int i = 0;i < pumbers.Length;i++)
        {
            Animator animator = pumbers[i].GetComponent<Animator>();
            animator.SetInteger("State", state);
        }
    }
}
