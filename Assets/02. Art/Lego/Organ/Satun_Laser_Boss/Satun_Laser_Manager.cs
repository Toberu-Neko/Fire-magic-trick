using UnityEngine;

public class Satun_Laser_Manager : MonoBehaviour
{
    private Satun_Laser[] satuns;
    private Satun_Laser_New[] news;

    private void Awake()
    {
        satuns = GetComponentsInChildren<Satun_Laser>();
        news = GetComponentsInChildren<Satun_Laser_New>();
    }
    public void playLaser()
    {
        for (int i = 0; i < satuns.Length; i++)
        {
            news[i].PlayLaser();
        }
    }
    public void active(bool active)
    {
        for(int i=0; i<satuns.Length;i++)
        {
            satuns[i].active(active);
        }
    }
}
