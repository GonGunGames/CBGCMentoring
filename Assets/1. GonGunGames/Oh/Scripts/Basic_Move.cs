using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Move : MonoBehaviour
{
    public int Speed;
    public GameObject projectilePrefab;  // 발사할 프리펩을 참조하는 변수
    public Transform firePoint;          // 프리펩이 발사될 위치를 지정하는 변수
    public float projectileSpeed = 20f;  // 발사될 프리펩의 속도

    void Update()
    {
        // 플레이어 이동 코드
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * Speed * Time.deltaTime);
        }

        // CTRL 키를 눌렀을 때 프리펩 발사
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            // firePoint 위치에서 회전 없이 프리펩을 생성
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Rigidbody를 가져와 속도를 설정
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * projectileSpeed;
            }
            else
            {
                Debug.LogWarning("Projectile prefab does not have a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogWarning("Projectile prefab or fire point is not assigned.");
        }
    }
}
