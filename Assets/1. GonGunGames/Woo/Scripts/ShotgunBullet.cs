using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllUnits;

public class ShotgunBullet : MonoBehaviour 
{ 

    public static float destroyDelay = 0.2f; // 자동 파괴 지연 시간
       void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    public IEnumerator DestroyAfterDelay()
    {
        Debug.Log("Destroying bullet in " + destroyDelay + " seconds.");
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
