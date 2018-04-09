using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyRewardsInterface : MonoBehaviour
{

    // Prefab containing the daily reward
    public GameObject dailyRewardPrefab;

    // Rewards panel
    public GameObject panelReward;
    public Text txtReward;

    // Claim Button
    public GameObject btnClaim;

    // How long until next claim
    public Text txtTimeDue;

    // The Grid that contains the rewards
    public GridLayoutGroup dailyRewardsGroup;

    void Start()
    {
        DailyRewards.instance.CheckRewards();

        DailyRewards.instance.onClaimPrize += OnClaimPrize;
        DailyRewards.instance.onPrizeAlreadyClaimed += OnPrizeAlreadyClaimed;

        UpdateUI();
    }

    void OnDestroy()
    {
        DailyRewards.instance.onClaimPrize -= OnClaimPrize;
        DailyRewards.instance.onPrizeAlreadyClaimed -= OnPrizeAlreadyClaimed;
    }

    // Clicked the claim button
    public void OnClaimClick()
    {
        DailyRewards.instance.ClaimPrize(DailyRewards.instance.availableReward);
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (Transform child in dailyRewardsGroup.transform)
        {
            Destroy(child.gameObject);
        }

        bool isRewardAvailableNow = false;

        for (int i = 0; i < DailyRewards.instance.rewards.Count; i++)
        {
            int reward = DailyRewards.instance.rewards[i];
            int day = i + 1;

            GameObject dailyRewardGo = GameObject.Instantiate(dailyRewardPrefab) as GameObject;

            DailyRewardUI dailyReward = dailyRewardGo.GetComponent<DailyRewardUI>();
            dailyReward.transform.SetParent(dailyRewardsGroup.transform);
            dailyRewardGo.transform.localScale = Vector2.one;

            dailyReward.day = day;
            dailyReward.reward = reward;

            dailyReward.isAvailable = day == DailyRewards.instance.availableReward;
            dailyReward.isClaimed = day <= DailyRewards.instance.lastReward;

            if (dailyReward.isAvailable)
            {
                isRewardAvailableNow = true;
            }

            dailyReward.Refresh();
        }

        btnClaim.gameObject.SetActive(isRewardAvailableNow);
        txtTimeDue.gameObject.SetActive(!isRewardAvailableNow);
    }

    void Update()
    {
        if (txtTimeDue.IsActive())
        {
            TimeSpan difference = (DailyRewards.instance.lastRewardTime - DailyRewards.instance.timer).Add(new TimeSpan(0, 24, 0, 0));

            // Is the counter below 0? There is a new reward then
            if (difference.TotalSeconds <= 0)
            {
                DailyRewards.instance.CheckRewards();
                UpdateUI();
                return;
            }

            string formattedTs = string.Format("{0:D2}:{1:D2}:{2:D2}", difference.Hours, difference.Minutes, difference.Seconds);

            txtTimeDue.text = "Return in " + formattedTs + " to claim your reward";
        }
    }

    // Delegate
    private void OnClaimPrize(int day)
    {
        panelReward.SetActive(true);
        txtReward.text = "You got " + DailyRewards.instance.rewards[day - 1] + " coins!";
    }

    // Delegate
    private void OnPrizeAlreadyClaimed(int day)
    {
        // Do Something with the prize already claimed
    }

    // Close the Rewards panel
    public void OnCloseRewardsClick()
    {
        panelReward.SetActive(false);
    }

    // Resets player preferences
    public void OnResetClick()
    {
        DailyRewards.instance.Reset();
        DailyRewards.instance.lastRewardTime = System.DateTime.MinValue;
        DailyRewards.instance.CheckRewards();

        UpdateUI();
    }

    // Simulates the next day
    public void OnAdvanceDayClick()
    {
        DailyRewards.instance.timer = DailyRewards.instance.timer.AddDays(1);
        DailyRewards.instance.CheckRewards();
        UpdateUI();
    }

    // Simulates the next hour
    public void OnAdvanceHourClick()
    {
        DailyRewards.instance.timer = DailyRewards.instance.timer.AddHours(1);
        DailyRewards.instance.CheckRewards();
        UpdateUI();
    }

    public void ButtonClickAudio()
    {
        AudioManager.instance.ButtonClickAudio();
    }
}
