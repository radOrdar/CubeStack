using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LostUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI restartText;
    [SerializeField] private TextMeshProUGUI gameoverText;
    [SerializeField] private Button homeBtn;

    private void Awake()
    {
        homeBtn.onClick.AddListener(() => GameManager.Instance.GoHome());

        switch (YandexGame.EnvironmentData.language)
        {
            case "ru":
                restartText.text = "Рестарт";
                gameoverText.text = "Игра окончена";
                break;
            case "en":
                restartText.text = "Restart";
                gameoverText.text = "Game over";
                break;
        }
    }

    public void ShowLost(int score, int bestScore)
    {
        switch (YandexGame.EnvironmentData.language)
        {
            case "ru":
                scoreText.SetText($"Очки: {score.ToString()}");
                bestScoreText.SetText($"Лучший результат: {bestScore.ToString()}");
                break;
            case "en":
                scoreText.SetText($"Score: {score.ToString()}");
                bestScoreText.SetText($"Best: {bestScore.ToString()}");
                break;
        }
    }
}