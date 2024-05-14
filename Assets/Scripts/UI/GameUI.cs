using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameLogo;
    [SerializeField] private TextMeshProUGUI scoreText;

    // private void Awake()
    // {
    //     LMotion.Create(1f, 1f, 0f).BindToColorA(gameLogo);
    //     LMotion.Create(0f, 0f, 0f).BindToColorA(scoreText);
    // }
    
    public async UniTask AnimateToShowLogo(float duration)
    {
        await LMotion.Create(0f, 0.9f, duration).BindToColorA(gameLogo);
        await LMotion.Create(1f, 0f, duration).BindToColorA(scoreText);
    } 
    public async UniTask AnimateToShowScore(float duration)
    {
        await LMotion.Create(1f, 0f, duration).BindToColorA(gameLogo);
        await LMotion.Create(0f, 0.7f, duration).BindToColorA(scoreText);
    }

    public void SetScore(int score)
    {
        scoreText.SetText(score.ToString());
    }
}