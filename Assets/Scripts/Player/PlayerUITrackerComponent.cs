using TMPro;
using UnityEngine;

internal class PlayerUITrackerComponent
{
    private TMP_Text _goldText;
    private TMP_Text _hourText;
    private TMP_Text _dayText;

    public PlayerUITrackerComponent(TMP_Text goldText, TMP_Text hourText, TMP_Text dayText)
    {
        _goldText = goldText;
        _hourText = hourText;
        _dayText = dayText;
    }

    public void OnFixedUpdate(ref PlayerInventoryState inventoryState, ref DayState dayState)
    {
        _goldText.SetText(inventoryState.GoldCount.ToString());

        // if (dayState.HasChanged)
        {
            // 15.8
            var hour = (int)Mathf.Floor(dayState.currentHour);
            var remainingHour = dayState.currentHour - hour;
            var minutes = (int)(remainingHour * 60);
            _hourText.SetText($"{hour:00}:{minutes:00}");
            _dayText.SetText(dayState.currentDay.ToString());
        }
    }
}