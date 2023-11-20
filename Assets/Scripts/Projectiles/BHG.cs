using System.Collections;
using UnityEngine;

public class BHG : MonoBehaviour
{   
    [HideInInspector] public ThirdPersonShooterController tpsc;

    void Update()
    {
            gameObject.transform.position = tpsc.spawnBlackHoleBulletPosition.position;
            gameObject.transform.rotation = tpsc.spawnBlackHoleBulletPosition.rotation;
    }
}
