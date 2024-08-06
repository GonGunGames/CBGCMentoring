using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerExp : MonoBehaviour
{
    public int currentLevel;
    public double currentExp;
    public double expToLevelUp; // 레벨업에 필요한 경험치
    public Slider expSlider; // 경험치 슬라이더 UI
    private PlayerGold playergold; // PlayerGold 인스턴스
    private LevelManager levelManager;
    private bool isLevelingUp = false; // 레벨업 중인지 여부를 체크하는 변수
    public GameObject particlePrefab; // 파티클 프리펩
    public float particleDuration = 1.5f; // 파티클 지속 시간
    public AudioSource audioSource;
    private void Start()
    {
        particlePrefab.SetActive(false);
        // 초기화
        currentLevel = DataBase.Instance.playerData.currentLevel;
        currentExp = DataBase.Instance.playerData.currentExp;
        expToLevelUp = DataBase.Instance.playerData.expToLevelUp;
        levelManager = FindObjectOfType<LevelManager>();

        // PlayerGold 스크립트 찾기
        playergold = FindObjectOfType<PlayerGold>();

        if (playergold == null)
        {
            Debug.LogError("PlayerGold instance not found.");
        }

        // 슬라이더 초기화
        expSlider.maxValue = (float)expToLevelUp;
        expSlider.value = (float)currentExp;
    }

    // 경험치 획득 메서드
    public void GainExp(int exp)
    {
        if (!isLevelingUp) // 레벨업 중일 때는 경험치를 추가하지 않음
        {
            currentExp += exp;
            // 경험치 슬라이더 업데이트
            UpdateExpSlider();
            // 레벨업 체크
            CheckLevelUp();
        }
    }

    // 경험치 슬라이더 업데이트 메서드
    private void UpdateExpSlider()
    {
        expSlider.maxValue = (float)expToLevelUp;
        expSlider.value = (float)currentExp;
    }

    // 레벨업 체크 메서드
    private void CheckLevelUp()
    {
        while (currentExp >= expToLevelUp)
        {
            LevelUp();
            if (isLevelingUp) // 레벨업 중이면 루프 탈출
            {
                break;
            }
        }
    }

    // 레벨업 메서드
    private void LevelUp()
    {
        isLevelingUp = true; // 레벨업 시작
        Time.timeScale = 0f; // 게임 일시 정지
        particlePrefab.SetActive(true);
        currentLevel++;
        currentExp -= expToLevelUp;
        expToLevelUp = CalculateNextLevelExp(); // 다음 레벨에 필요한 경험치 계산

        // 경험치 슬라이더 업데이트
        UpdateExpSlider();

        // 레벨업 시 선택지 제공
        levelManager.SetOnUpgradeOptionsClosedCallback(OnUpgradeOptionsClosed);
        levelManager.ShowUpgradeOptions();
    }

    private void OnUpgradeOptionsClosed()
    {
        isLevelingUp = false; // 레벨업 완료
        Time.timeScale = 1f; // 게임 재개
        audioSource.Play();
        // 파티클 활성화
        StartCoroutine(ActivateParticleEffect());

        // 레벨업 후에도 경험치가 충분하면 다시 체크
        CheckLevelUp();
    }

    // 다음 레벨에 필요한 경험치 계산
    private double CalculateNextLevelExp()
    {
        // 간단히 다음 레벨이 더 많은 경험치가 필요하다고 가정
        return expToLevelUp * 1.5;
    }

    // 선택한 옵션에 따라 능력치 업그레이드
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ItemEx itemEx = other.GetComponent<ItemEx>();

            if (itemEx != null)
            {
                GainExp(itemEx.expAmount); // 아이템에서 정의된 경험치 양을 플레이어의 경험치에 추가
                itemEx.ExpGet();//아이템 먹는 이펙트 효과 및 사운드 재생
            }
        }
    }

    private IEnumerator ActivateParticleEffect()
    {
        GameObject particleInstance = Instantiate(particlePrefab, transform.position, Quaternion.identity, transform); // 플레이어의 자식으로 설정
        ParticleSystem particleSystem = particleInstance.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            particleSystem.Play();
            Debug.Log("Particle started");
            yield return new WaitForSeconds(particleDuration);
            particleSystem.Stop();
            Destroy(particleInstance.gameObject);
            Debug.Log("Particle stopped");
        }
        else
        {
            Debug.LogError("No ParticleSystem component found on the particlePrefab.");
        }
    }
}