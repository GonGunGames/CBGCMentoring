using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageTextManager : MonoBehaviour
{
    public GameObject damageTextPrefab; // 데미지 텍스트 프리팹
    public Transform canvasTransform; // 캔버스 트랜스폼

    public void ShowDamageText(Vector3 position, float damage)
    {
        GameObject damageTextObj = Instantiate(damageTextPrefab, canvasTransform);
        damageTextObj.GetComponent<Text>().text = damage.ToString();
        damageTextObj.transform.position = Camera.main.WorldToScreenPoint(position); // 월드 좌표를 스크린 좌표로 변환

        StartCoroutine(HideDamageText(damageTextObj, 2f)); // 2초 후에 텍스트 제거
    }

    private IEnumerator HideDamageText(GameObject damageTextObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(damageTextObj);
    }
}