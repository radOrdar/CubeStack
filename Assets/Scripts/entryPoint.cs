using UnityEngine;
using UnityEngine.SceneManagement;

public class entryPoint : MonoBehaviour
{
    [SerializeField] private GameObject yg;

    [SerializeField] private GameObject lb;
    [SerializeField] private TimerForAd timerForAd;


    // Start is called before the first frame update
    private void Awake()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(yg);
        DontDestroyOnLoad(lb);
        DontDestroyOnLoad(timerForAd);
        SceneManager.LoadScene("Game");
    }
}