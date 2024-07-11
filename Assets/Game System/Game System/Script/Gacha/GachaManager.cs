using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    [SerializeField] GachaRate[] gacha;
    [SerializeField] Transform cardParent;
    [SerializeField] List<TextMeshProUGUI> UIRate;
    [SerializeField] GameObject rewardGO;
    InventoryItem item = new InventoryItem();

    [SerializeField] Button oneTimeButton;
    [SerializeField] Button tenTimeButton;

    GameObject[] rewardGOs = new GameObject[10];

    /* Question: Why don't we use the On Click(), which is function of Button, in the Inspector?
     * Answer: Put everything in the code will be easy to manage => U should use onClick.AddListener().
     *        You can see that we immediately know that we have 2 buttons for this system. Instead of searching them in the hierachy.

     * Question: Why put it in Awake() but not Start()?
     * Answer: Later, when you use Event, all the event will be put in the Awake(), which happens before Start().
     *         If you put onClick.AddListener() in Start(), there will be a chance that it miss the event.
     */
    private void Awake()
    {
        
        oneTimeButton.onClick.AddListener(() =>
        {
            GachaOneTime();
        });

        tenTimeButton.onClick.AddListener(() =>
        {
            GachaTenTime();
        });
    }

    private void Start()
    {
        // 아이템 등급별 획득 확률 표시
        for(int i = 0; i < gacha.Length; i++ )
        {
            UIRate[i].text = gacha[i].rate.ToString() + "%";
        }
    }

    // 가챠 1번
    public void GachaOneTime()
    {
        for (int i = 1; i < rewardGOs.Length; i++) //Set active false for all other reward
        {
            if (rewardGOs[i] != null)
                rewardGOs[i].SetActive(false);
        }
        if (rewardGOs[0] == null)
        {
            rewardGOs[0] = Instantiate(rewardGO, cardParent.position, Quaternion.identity);
            rewardGOs[0].transform.SetParent(cardParent);
            rewardGOs[0].transform.localScale = Vector3.one;
        }

        item.data = rewardGOs[0].GetComponent<InventoryItem>().data;


        //Check prize
        int rnd = UnityEngine.Random.Range(1, 101);
        int totalRate = 0;
        for (int i = 0; i < gacha.Length; i++)
        {
            totalRate += gacha[i].rate;
            if (rnd <= totalRate)
            {
                item.data.info = Reward(gacha[i].rarity).info;
                return;
            }
            
        }
    }

    // 가챠 10번
    public void GachaTenTime()
    {
        for (int i = 0; i < 10; i++)
        {
            if (rewardGOs[i] != null)
                rewardGOs[i].SetActive(true);

            if (rewardGOs[i] == null)
            {
                rewardGOs[i] = Instantiate(rewardGO, cardParent.position, Quaternion.identity);
                rewardGOs[i].transform.SetParent(cardParent);
                rewardGOs[i].transform.localScale = Vector3.one;
            }


            item.data = rewardGOs[i].GetComponent<InventoryItem>().data;


            //Check prize
            int rnd = UnityEngine.Random.Range(1, 101);
            int totalRate = 0;
            for (int j = 0; j < gacha.Length; j++)
            {
                totalRate += gacha[j].rate;
                if (rnd <= totalRate)
                {
                    item.data.info = Reward(gacha[j].rarity).info;
                    break;
                }
            }
        }
    }


    [System.Serializable]
    public class GachaRate                  // 각 아이템의 희귀도와 확률 설정 클래스
    {
        public string rarity;               // 등급
        [Range(1, 100)]                     // To Show the Slider in Inspector
        public int rate;                    // 확률
        public ItemBase.ItemData[] rewards; // 해당 등급의 아이템 종류
    }

    // 확률에 따른 희귀도 선택하고 해당 등급의 무작위 아이템 선택한 뒤 인벤토리 추가
    ItemBase.ItemData Reward(string rarity)
    {
        GachaRate gr = Array.Find(gacha, rt => rt.rarity == rarity); // Find the rarity of reward that matches with given rarity

        ItemBase.ItemData[] rewards = gr.rewards;

        int rnd = UnityEngine.Random.Range(0, rewards.Length);

        ItemBase.ItemData finalReward = rewards[rnd];

        InventoryManager.Instance.AddAmountOfItem(finalReward, 1);

        return finalReward;
    }
}
