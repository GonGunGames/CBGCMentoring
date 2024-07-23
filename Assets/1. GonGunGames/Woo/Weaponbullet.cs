using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllUnits;

public class Weaponbullet :MonoBehaviour
{
    private float destroyDelay = 2f; // 자동 파괴 지연 시간

    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 레이어를 확인하여 처리
        if (collision.collider.CompareTag("Enemy"))
        {
            Destroy(gameObject); // 총알 파괴
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        Debug.Log("Destroying bullet in " + destroyDelay + " seconds.");
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
