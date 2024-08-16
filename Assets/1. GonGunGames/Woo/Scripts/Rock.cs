using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // Start is called before the first frame update
    public static float rockDamage;
    void Start()
    {
        rockDamage = 200f;
    }

}
