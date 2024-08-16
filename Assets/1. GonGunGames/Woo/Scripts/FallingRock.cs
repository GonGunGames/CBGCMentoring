using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FallingRock : MonoBehaviour
{
    public GameObject player;  // 플레이어 오브젝트
    public GameObject rock;    // 돌 오브젝트
    public GameObject range;   // 돌이 떨어질 범위 오브젝트
    public float delayTime = 3f;  // 돌이 떨어질 시간 딜레이
    public float disappearTime = 5f;  // 돌이 사라질 시간
    public float repeatInterval = 10f;  // 돌이 다시 떨어질 간격
    public float currentDamage = 200f;

    void Start()
    {
        // 처음에 돌과 범위를 비활성화
        rock.SetActive(false);
        range.SetActive(false);
        currentDamage = Rock.rockDamage;
        // 반복적으로 돌을 떨어뜨리는 코루틴 시작
        StartCoroutine(RepeatDropRock());
    }

    IEnumerator RepeatDropRock()
    {
        while (true)  // 무한 반복
        {
            // 돌이 떨어지기 전 일정 시간 대기
            yield return new WaitForSeconds(delayTime);

            // 돌과 범위 활성화
            rock.SetActive(true);
            range.SetActive(true);

            // 플레이어의 현재 위치 가져오기
            Vector3 playerPosition = player.transform.position;

            // 돌의 위치를 플레이어의 Y축 위로 이동
            Vector3 dropPosition = new Vector3(playerPosition.x + 1f, playerPosition.y + 10f, playerPosition.z);
            Vector3 rangePosition = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
            rock.transform.position = dropPosition;
            range.transform.position = rangePosition;

            // 돌을 떨어뜨리기 위해 중력을 활성화
            Rigidbody rb = rock.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;  // 중력 활성화
            }

            // 돌이 사라지기 전까지 대기
            yield return new WaitForSeconds(disappearTime);

            // 돌과 범위를 비활성화하여 사라지게 함
            rock.SetActive(false);
            range.SetActive(false);

            // 다음 반복을 위해 대기
            yield return new WaitForSeconds(repeatInterval);
        }
    }
}
