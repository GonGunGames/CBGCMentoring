using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUtil
{
    public static float MoveFrame(CharacterController cc, Transform target, float moveSpeed, float turnSpeed)
    {
        Transform t = cc.transform;
        Vector3 dir = target.position - t.position;
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);
        Vector3 targetPos = t.position + dirXZ;
        Vector3 framePos = Vector3.MoveTowards(t.position, targetPos, moveSpeed * Time.deltaTime);

        cc.Move(framePos - t.position);

        RotateDir(t, target, turnSpeed);

        return Vector3.Distance(framePos, targetPos);
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

        self.rotation = Quaternion.LookRotation(dirXZ * Time.deltaTime);
    }

}
