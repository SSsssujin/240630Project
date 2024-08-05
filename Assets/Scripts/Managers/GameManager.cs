using System.Collections;
using System.Diagnostics;
using INeverFall;
using INeverFall.Manager;
using INeverFall.Player;
using INeverFall.Util;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GameManager : Singleton<GameManager>
{
    public static bool IsLoadCompleted;
    public static bool IsPlayerLoadCompleted;
    
    private bool _isGameOver;
    private bool _isPlayerInBossRoom;
    private bool _isSettingWindowOpened;
    
    // Character
    private PlayerCharacter _playerCharacter;

    // Environment
    private PortalTextController _portalTextController; 
    private BossRoomDoor _bossRoomDoor; 
    
    
    public PortalTextController PortalTextController => _portalTextController; 
    public BossRoomDoor BossRoomDoor => _bossRoomDoor; 
    public Text LowerText => _text;

    [SerializeField] private Text _text;
    [SerializeField] private GameObject _settingWindow;
    [SerializeField] private GameObject _winWindow;
    [SerializeField] private GameObject _loseWindow;
    [SerializeField] private GameObject _bossHpBar;

    public bool IsBossRoomOpened;

    private void Start()
    {
        Debug.Log("Load");
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        SoundManager.Instance.StopAudio("Battle");
        SoundManager.Instance.PlayAudio("Normal");

        // _boss = ResourceManager.Instance.Instantiate("Boss");
        // _boss.transform.position = new Vector3(1.6f, -4.47f, 68.82f);
        // _boss.transform.rotation = Quaternion.Euler(0, -180, 0);
        // _boss.transform.localScale = Vector3.one * 5;

        _isGameOver = false;

        // Caching
        _playerCharacter = FindFirstObjectByType<PlayerCharacter>(FindObjectsInactive.Include);
        _portalTextController = FindFirstObjectByType<PortalTextController>(FindObjectsInactive.Include);
        _bossRoomDoor = FindFirstObjectByType<BossRoomDoor>(FindObjectsInactive.Include);

        // Add listener
        PlayerChecker.Instance.PlayerEntered += _OnPlayerEntered;
    }

    //private GameObject _boss;

    void OnDestroy()
    {
        _isPlayerInBossRoom = false;
        PlayerChecker.Instance.IsPlayerInBossRoom = false;
        PlayerChecker.Instance.PlayerEntered -= _OnPlayerEntered;
        //ResourceManager.Instance.Destroy(_boss);
    }
    
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.visible == false ? CursorLockMode.Locked : CursorLockMode.None;
        }
#endif

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _OpenSettingWindow();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SoundManager.Instance.PlayAudio("Battle", "Normal");
        }
    }

    private void _OnPlayerEntered()
    {
        _bossRoomDoor ??= FindFirstObjectByType<BossRoomDoor>(FindObjectsInactive.Include);
        
        _bossRoomDoor.Close();
        _bossHpBar.SetActive(true);
        
        SoundManager.Instance.PlayAudio("Battle", "Normal");
    }

    private void _OpenSettingWindow()
    {
        _isSettingWindowOpened = !_isSettingWindowOpened;

        if (_isSettingWindowOpened)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        Time.timeScale = _isSettingWindowOpened ? 0 : 1;
        _settingWindow.SetActive(_isSettingWindowOpened);
    }

    public void GameOver(bool isPlayerWin)
    {
        if (_isGameOver) return;
        
        _isGameOver = true;
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        FindFirstObjectByType<CinemachineCamera>().enabled = false;
        
        if (isPlayerWin)
        {
            StartCoroutine((_cActivateResultWindow(_winWindow)));
        }
        else
        {
            StartCoroutine((_cActivateResultWindow(_loseWindow)));
        }
    }

    private IEnumerator _cActivateResultWindow(GameObject window)
    {
        yield return new WaitForSeconds(1);
        window.SetActive(true);
    }

    
    #region [ Button events ]

    public void OnCancelButtonClicked()
    {
        _OpenSettingWindow();
    }
    
    public async void OnRestartButtonClicked()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        await SceneManager.LoadSceneAsync(currentScene.name);
        Time.timeScale = 1;
    }

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    #endregion

    public bool IsGamePlaying => !_isGameOver;
}
