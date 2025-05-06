using UnityEngine;
using UnityEngine.UI;

public class DreamMeterUI : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public Image dreamImage;

    public Sprite dream1; // 0–20%
    public Sprite dream2; // 21–40%
    public Sprite dream3; // 41–60%
    public Sprite dream4; // 61–80%
    public Sprite dream5; // 81–100%

    private void Start()
    {
        UpdateDreamUI();
        playerStatus.onDreamMeterChanged.AddListener(UpdateDreamUI);
    }

    void UpdateDreamUI()
    {
        float percentage = playerStatus.DreamMeter / playerStatus.MaxDreamMeter;

        if (percentage > 0.8f)
            dreamImage.sprite = dream5;
        else if (percentage > 0.6f)
            dreamImage.sprite = dream4;
        else if (percentage > 0.4f)
            dreamImage.sprite = dream3;
        else if (percentage > 0.2f)
            dreamImage.sprite = dream2;
        else
            dreamImage.sprite = dream1;
    }
}
