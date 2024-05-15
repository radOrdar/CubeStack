using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameManager : MonoBehaviour
{
    enum Direction
    {
        X,
        Z
    }

    public static GameManager Instance;

    [SerializeField] private GameObject blockPf;
    [SerializeField] private Transform baseBlock;
    [SerializeField] private Transform cameraParent;

    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private float cameraSpeed = 2;
    [SerializeField] private float matchEps = 0.05f;

    [SerializeField] private float period;
    [SerializeField] private float amplitude;

    [SerializeField] private Material skyMaterial;
    [SerializeField] private Material fogMaterial;
    [SerializeField] private Material cubeMaterial;
    [SerializeField] private float hueStep = 1.7f;
    [SerializeField] private BorderFx borderFx;
    [SerializeField] private UI ui;

    private AudioManager _audioManager;
    private MaterialPropertyBlock _materialPropertyBlock;
    private Transform _currentBlock;
    private Path2Points _currentPath2Points;
    private int _hueMultiplier;

    private int _score;
    private int _streak;

    private Direction _direction = Direction.X;
    private Vector3 _cameraTargetPos;

    private bool _gameStarted;
    private bool _gameOver;


    [SerializeField] private WarningAdCanvas WarningAdCanvasPrefab;


    private void Awake()
    {
        Instance = this;

        Color colorA = Color.HSVToRGB(Random.Range(0f, 1f), 0.6f, 0.9f);
        Color colorB = Color.HSVToRGB(Random.Range(0f, 1f), 0.6f, 0.9f);
        skyMaterial.SetColor("_ColorA", colorA);
        skyMaterial.SetColor("_ColorB", colorB);
        fogMaterial.SetColor("_ColorA", colorA);
        fogMaterial.SetColor("_ColorB", colorB);
        _cameraTargetPos = cameraParent.position;
        _hueMultiplier = Random.Range(1, 200);
        cubeMaterial.SetColor("_BaseColor", Color.HSVToRGB(hueStep * _hueMultiplier % 360 / 360, 0.6f, 0.95f));
        _materialPropertyBlock = new();
    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        ui.SetScore(_score);
        ui.ShowLogo();
    }

    private void Update()
    {
        if (_gameStarted == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _gameStarted = true;
                _currentBlock = Instantiate(blockPf).transform;
                _currentBlock.localScale = new Vector3(width, height, width);

                // _materialPropertyBlock = new();
                // _materialPropertyBlock.SetColor("_BaseColor", Color.HSVToRGB(hueStep * _hueMultiplier/360, 0.5f, 1f));
                _currentBlock.GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);
                _currentPath2Points = new Path2Points(Vector3.left * amplitude, Vector3.right * amplitude);
                ui.ShowScore();
            }

            return;
        }

        if (_gameOver)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 currPos = _currentBlock.position;
            Vector3 basePos = baseBlock.position;
            Vector3 currScale = _currentBlock.localScale;

            if (Mathf.Abs(currPos.x - basePos.x) < matchEps && Mathf.Abs(currPos.z - basePos.z) < matchEps)
            {
                //PERFECT
                _currentBlock.position = new Vector3(basePos.x, _currentBlock.position.y, basePos.z);
                borderFx.Show(currScale.x, currScale.z, _currentBlock.position - Vector3.up * height / 2);
                NewPath();
                _streak++;
                UpdateScore();
                _audioManager.PlayPlace();
                _audioManager.PlayPerfect();

                return;
            }

            _streak = 0;

            float blockWidth = _direction == Direction.X ? currScale.x : currScale.z;
            Vector3 blockToBase = Vector3.ProjectOnPlane(baseBlock.position - _currentBlock.position, Vector3.up);
            float distance = blockToBase.magnitude;
            if (distance >= blockWidth)
            {
                //LOST
                if (_score > YandexGame.savesData.bestScore)
                {
                    YandexGame.savesData.bestScore = _score;
                    YandexGame.NewLeaderboardScores("CubeLb", _score);
                }

                // PlayerPrefs.SetInt("Record", Mathf.Max(PlayerPrefs.GetInt("Record"), _score));
                _currentBlock.gameObject.AddComponent<Rigidbody>();
                _currentBlock = null;
                ui.ShowLost(_score, YandexGame.savesData.bestScore);
                _gameOver = true;
                _audioManager.PlayLost();
                return;
            }

            //CUT THE BLOCK
            float newWidth = blockWidth - distance;
            _currentBlock.localScale = _direction == Direction.X
                ? new Vector3(newWidth, height, currScale.z)
                : new Vector3(currScale.x, height, newWidth);
            _currentBlock.position += blockToBase.normalized * (blockWidth - newWidth) / 2;
            _audioManager.PlayPlace();

            Transform splinter = Instantiate(blockPf).transform;
            splinter.GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);
            splinter.gameObject.AddComponent<Rigidbody>();
            splinter.gameObject.AddComponent<DestroyAfterSeconds>().delay = 20f;
            splinter.localScale = _direction == Direction.X
                ? new Vector3(blockWidth - newWidth, height, currScale.z)
                : new Vector3(currScale.x, height, blockWidth - newWidth);
            splinter.position = _currentBlock.position - blockToBase.normalized * blockWidth / 2;

            NewPath();
            UpdateScore();
        }

        cameraParent.position = Vector3.Lerp(cameraParent.position, _cameraTargetPos, Time.deltaTime * cameraSpeed);
        Vector3 newPos = _currentPath2Points.NewPoint(Time.deltaTime / period);

        _currentBlock.position = newPos;
    }

    public async void GoHome()
    {
        if (TimerForAd.Instance.time >= 61)
        {
            TimerForAd.Instance.time = 0;
            Time.timeScale = 0;
            await ShowWarningAd();
        }
        else if (YandexGame.savesData.bestScore > 120)
        {
            YandexGame.ReviewShow(false);
        }

        SceneManager.LoadScene("Game");


        Time.timeScale = 1;
    }

    private async UniTask ShowWarningAd()
    {
        WarningAdCanvas warningAdCanvas = Instantiate(WarningAdCanvasPrefab);
        await UniTask.Delay(1800, true);

        YandexGame.FullscreenShow();
        if (warningAdCanvas != null)
        {
            Destroy(warningAdCanvas.gameObject);
        }
    }

    private void UpdateScore()
    {
        _score += 1 + _streak;
        ui.SetScore(_score);
    }

    private void NewPath()
    {
        _hueMultiplier++;
        _cameraTargetPos += Vector3.up * height;
        baseBlock = _currentBlock;
        _currentBlock = Instantiate(baseBlock).transform;
        _currentBlock.localScale = baseBlock.localScale;

        _materialPropertyBlock.SetColor("_BaseColor", Color.HSVToRGB(hueStep * _hueMultiplier % 360 / 360, 0.6f, 0.95f));
        _currentBlock.GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);

        Vector3 newP = baseBlock.position + Vector3.up * height;
        _direction = _direction == Direction.X ? Direction.Z : Direction.X;
        Vector3 v1 = _direction == Direction.X ? Vector3.left : Vector3.forward;
        Vector3 v2 = _direction == Direction.X ? Vector3.right : Vector3.back;
        _currentPath2Points = new Path2Points(v1 * amplitude + newP, v2 * amplitude + newP);
    }
}