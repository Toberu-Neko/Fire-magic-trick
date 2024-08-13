using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCount : MonoBehaviour
{
    [SerializeField] private GameObject windCard1;
    [SerializeField] private GameObject windCard2;
    [SerializeField] private GameObject windCard3;
    [SerializeField] private GameObject windCard4;
    [SerializeField] private GameObject windCard5;
    [SerializeField] private GameObject windCard6;

    [SerializeField] private GameObject fireCard1;
    [SerializeField] private GameObject fireCard2;
    [SerializeField] private GameObject fireCard3;
    [SerializeField] private GameObject fireCard4;
    [SerializeField] private GameObject fireCard5;
    [SerializeField] private GameObject fireCard6;

    private void Awake()
    {
        windCard1.SetActive(false);
        windCard2.SetActive(false);
        windCard3.SetActive(false);
        windCard4.SetActive(false);
        windCard5.SetActive(false);
        windCard6.SetActive(false);

        fireCard1.SetActive(false);
        fireCard2.SetActive(false);
        fireCard3.SetActive(false);
        fireCard4.SetActive(false);
        fireCard5.SetActive(false);
        fireCard6.SetActive(false);
    }

    public void SetWindCard(int count)
    {
        switch (count)
        {
            case 0:
                windCard1.SetActive(false);
                windCard2.SetActive(false);
                windCard3.SetActive(false);
                windCard4.SetActive(false);
                windCard5.SetActive(false);
                windCard6.SetActive(false);
                break;
            case 1:
                windCard1.SetActive(true);
                windCard2.SetActive(false);
                windCard3.SetActive(false);
                windCard4.SetActive(false);
                windCard5.SetActive(false);
                windCard6.SetActive(false);
                break;
            case 2:
                windCard1.SetActive(true);
                windCard2.SetActive(true);
                windCard3.SetActive(false);
                windCard4.SetActive(false);
                windCard5.SetActive(false);
                windCard6.SetActive(false);
                break;
            case 3:
                windCard1.SetActive(true);
                windCard2.SetActive(true);
                windCard3.SetActive(true);
                windCard4.SetActive(false);
                windCard5.SetActive(false);
                windCard6.SetActive(false);
                break;
            case 4:
                windCard1.SetActive(true);
                windCard2.SetActive(true);
                windCard3.SetActive(true);
                windCard4.SetActive(true);
                windCard5.SetActive(false);
                windCard6.SetActive(false);
                break;
            case 5:
                windCard1.SetActive(true);
                windCard2.SetActive(true);
                windCard3.SetActive(true);
                windCard4.SetActive(true);
                windCard5.SetActive(true);
                windCard6.SetActive(false);
                break;
            case 6:
                windCard1.SetActive(true);
                windCard2.SetActive(true);
                windCard3.SetActive(true);
                windCard4.SetActive(true);
                windCard5.SetActive(true);
                windCard6.SetActive(true);
                break;
        }
    }

    public void SetFireCard(int count)
    {
        switch (count)
        {
            case 0:
                fireCard1.SetActive(false);
                fireCard2.SetActive(false);
                fireCard3.SetActive(false);
                fireCard4.SetActive(false);
                fireCard5.SetActive(false);
                fireCard6.SetActive(false);
                break;
            case 1:
                fireCard1.SetActive(true);
                fireCard2.SetActive(false);
                fireCard3.SetActive(false);
                fireCard4.SetActive(false);
                fireCard5.SetActive(false);
                fireCard6.SetActive(false);
                break;
            case 2:
                fireCard1.SetActive(true);
                fireCard2.SetActive(true);
                fireCard3.SetActive(false);
                fireCard4.SetActive(false);
                fireCard5.SetActive(false);
                fireCard6.SetActive(false);
                break;
            case 3:
                fireCard1.SetActive(true);
                fireCard2.SetActive(true);
                fireCard3.SetActive(true);
                fireCard4.SetActive(false);
                fireCard5.SetActive(false);
                fireCard6.SetActive(false);
                break;
            case 4:
                fireCard1.SetActive(true);
                fireCard2.SetActive(true);
                fireCard3.SetActive(true);
                fireCard4.SetActive(true);
                fireCard5.SetActive(false);
                fireCard6.SetActive(false);
                break;
            case 5:
                fireCard1.SetActive(true);
                fireCard2.SetActive(true);
                fireCard3.SetActive(true);
                fireCard4.SetActive(true);
                fireCard5.SetActive(true);
                fireCard6.SetActive(false);
                break;
            case 6:
                fireCard1.SetActive(true);
                fireCard2.SetActive(true);
                fireCard3.SetActive(true);
                fireCard4.SetActive(true);
                fireCard5.SetActive(true);
                fireCard6.SetActive(true);
                break;
        }
    }
}
