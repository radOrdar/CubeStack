using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LostUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button homeBtn;

    private void Awake()
    {
        homeBtn.onClick.AddListener(() => GameManager.Instance.GoHome());
    }

    public void ShowLost(int score, int bestScore)
    {
        scoreText.SetText($"Score: {score.ToString()}");
        bestScoreText.SetText($"Best: {bestScore.ToString()}");
    }
}