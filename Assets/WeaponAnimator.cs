using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] GameObject blasterHolster;
    [SerializeField] GameObject blasterHand;
    [SerializeField] GameObject BHGHolster;
    [SerializeField] GameObject BHGHand;
    [SerializeField] GameObject KnifeHolster;
    [SerializeField] GameObject knifeHand;



    public void Swap(weapons obj, float time, bool swap)
    {
        StartCoroutine(Wait(obj, time, swap));
    }

    public IEnumerator Wait(weapons obj, float time, bool swap)
    {
        yield return new WaitForSeconds(time);

        switch(obj)
        {
            case weapons.blasterHand:
                blasterHand?.SetActive(swap);
                break;
            case weapons.blasterHolster:
                blasterHolster?.SetActive(swap);
                break;

            case weapons.BHGHand:
                BHGHand?.SetActive(swap);
                break;
            case weapons.BHGHolster:
                BHGHolster?.SetActive(swap);
                break;

            case weapons.knifeHand:
                knifeHand?.SetActive(swap);
                break;
            case weapons.KnifeHolster:
                KnifeHolster?.SetActive(swap);
                break;

            default:
                Debug.LogError("Incorrect weapon selected " + obj);
                break;
        }
    }
}

public enum weapons
{
    blasterHolster,
    blasterHand,
    BHGHolster,
    BHGHand,
    KnifeHolster,
    knifeHand,
}
