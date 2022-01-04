using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private string sceneName = "";

    [SerializeField]
    GameObject MainPanel = null;

    [SerializeField]
    GameObject MultiplayerPanel = null;

    [SerializeField]
    GameObject DifficultyPanel = null;

    [SerializeField]
    GameObject ControlsPanel = null;

    public int NumberExchanges = 0;

    public bool MultiplayerActivated = false;

    AudioSource audioSource = null;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        MainPanel.SetActive(true);
        MultiplayerPanel.SetActive(false);
        DifficultyPanel.SetActive(false);
        ControlsPanel.SetActive(false);

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenMultiplayerMenu()
    {
        MainPanel.SetActive(false);
        MultiplayerPanel.SetActive(true);
        DifficultyPanel.SetActive(false);
    }

    public void OpenDifficultyMenu()
    {
        MultiplayerPanel.SetActive(false);
        DifficultyPanel.SetActive(true);
    }

    public void OnePlayerOption()
    {
        MultiplayerActivated = false;
        OpenDifficultyMenu();
    }

    public void TwoPlayersOption()
    {
        MultiplayerActivated = true;
        OpenDifficultyMenu();
    }
    public void EasyOption()
    {
        NumberExchanges = 3;
        LoadScene();
    }

    public void NormalOption()
    {
        NumberExchanges = 6;
        LoadScene();
    }

    public void HardOption()
    {
        NumberExchanges = 9;
        LoadScene();
    }

    public void LoadScene()
    {
        audioSource.Pause();

        SceneManager.LoadScene(sceneName);
    }

    public void OpenControlsMenu()
    {
        MainPanel.SetActive(false);
        ControlsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        MainPanel.SetActive(true);
        MultiplayerPanel.SetActive(false);
        DifficultyPanel.SetActive(false);
        ControlsPanel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
