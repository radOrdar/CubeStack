using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using YG;

public class WarningAdCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI warningText;

    private void Awake()
    {
        switch (YandexGame.EnvironmentData.language)
        {
            case "ru":
                warningText.text = "Сейчас появится реклама";
                break;
            case "en":
                warningText.text = "Ad is coming";
                break;
        }

        DestroySelf();
    }

    private async UniTask DestroySelf()
    {
        await UniTask.Delay(5000, true);
        Time.timeScale = 1;
    }
}