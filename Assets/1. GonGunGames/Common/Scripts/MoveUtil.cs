using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUtil
{
    private static float gravity = -50.81f; // 중력 상수
    private static float verticalSpeed = 0f; // 수직 속도 요소

    public static float MoveFrame(CharacterController cc, Transform target, float moveSpeed, float turnSpeed)
    {
        Transform t = cc.transform;
        Vector3 dir = target.position - t.position;
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);

        // 중력 적용
        if (cc.isGrounded)
        {
            verticalSpeed = 0f; // 지면에 있으면 수직 속도 초기화
        }
        else
        {
            verticalSpeed += gravity * Time.deltaTime; // 중력 적용
        }

        Vector3 moveDirection = dirXZ.normalized * moveSpeed + Vector3.up * verticalSpeed;
        cc.Move(moveDirection * Time.deltaTime);

        RotateDir(t, target, turnSpeed);

        return Vector3.Distance(cc.transform.position, target.position);
    }

    public static void RotateDir(Transform self, Transform target, float turnSpeed)
    {
        Vector3 dir = target.position - self.position;
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);

        if (dirXZ == Vector3.zero)
            return;

        self.rotation = Quaternion.RotateTowards(self.rotation, Quaternion.LookRotation(dirXZ), turnSpeed * Time.deltaTime);
    }

    public static void RotateToDirBurst(Transform self, Transform target)
    {
        Vector3 dir = target.position - self.position;
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);

        if (dirXZ == Vector3.zero)
            return;

        self.rotation = Quaternion.LookRotation(dirXZ);
    }
}
