using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticle : MonoBehaviour
{
    public float destroyAfter = 3f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, destroyAfter);
    }
}
