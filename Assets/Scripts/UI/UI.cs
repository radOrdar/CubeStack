using Cysharp.Threading.Tasks;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameUI gameUI;
    [SerializeField] private LostUI lostUIPf;

    public void SetScore(int score)
    {
        gameUI.SetScore(score);
    }

    public async UniTask ShowLogo()
    {
        await gameUI.AnimateToShowLogo(0.01f);
    }

    public async UniTask ShowScore()
    {
        await gameUI.AnimateToShowScore(.7f);
    }

    public void ShowLost(int score, int bestScore)
    {
        Destroy(gameUI.gameObject);
        Instantiate(lostUIPf).ShowLost(score, bestScore);
    }
}