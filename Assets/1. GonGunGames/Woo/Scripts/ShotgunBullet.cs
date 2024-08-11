using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public float destroyDelay = 2.0f; // 기본값을 더 길게 설정
    public float damage;
    void Start()
    {
        damage = 0f; // 무기 공격력 설정
        StartCoroutine(DestroyAfterDelay());
    }

    public IEnumerator DestroyAfterDelay()
    {
        Debug.Log("Destroying bullet in " + destroyDelay + " seconds.");
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    public void SetDestroyDelay(float delay)
    {
        destroyDelay = delay;
        Debug.Log("Updated destroy delay to " + destroyDelay + " seconds.");
    }
}
