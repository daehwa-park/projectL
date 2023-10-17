using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelfDestroy : MonoBehaviour
{
    float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time >= 0.3f)
        {
            Destroy(this.gameObject);
        }
    }
}
