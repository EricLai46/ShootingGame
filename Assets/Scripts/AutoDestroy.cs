using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroy_timer = 1.0f;
    void Start()
    {
        Destroy(this.gameObject, destroy_timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
