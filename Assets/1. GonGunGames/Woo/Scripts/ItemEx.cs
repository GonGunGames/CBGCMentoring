using System.Collections;
using UnityEngine;

public class ItemEx : MonoBehaviour
{
    private AudioSource audioSource;
    public int expAmount; // 아이템으로 얻는 경험치 양
    public GameObject getExpParticle;
    private BoxCollider boxcoll;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        boxcoll = GetComponent<BoxCollider>();
    }
    public void ExpGet()
    {
        //코루틴 실행 -> 코루틴 문 파괴까지
        StartCoroutine(ActiveExp());
    }

    public IEnumerator ActiveExp()
    {
        boxcoll.enabled = false;
        audioSource.Play();
        getExpParticle.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        Debug.Log("--------------------------");
        Destroy(gameObject);
    }
}