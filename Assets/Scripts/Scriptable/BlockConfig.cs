using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Block Config", fileName = "BlockConfig.asset")]
public class BlockConfig : ScriptableObject
{
    public float[] dropSpeed;
    public Sprite[] basicBlockSprites;
    public GameObject[] explosion;

    public GameObject GetExplosionObject(Block block)
    {
        if(block.breed == BlockBreed.NA)
        {
            return Instantiate(explosion[5]);

        }
        return Instantiate(explosion[(int)block.breed]);
    }
}
