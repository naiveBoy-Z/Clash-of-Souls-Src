using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefeatPanelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI panelTitle, replayBtnText;
    [SerializeField] private GameObject pauseBtn, closePanelBtn;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource battleBGM;

    private void Start()
    {
        playerNameText.text = PlayerPrefs.GetString("PlayerName", "Player");
        if (playerNameText.text == "") playerNameText.text = "Player";
        volumeSlider.value = PlayerPrefs.GetInt("Volume", 100);
        battleBGM.volume = (float) volumeSlider.value / 100;

        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        gameObject.SetActive(false);
    }


    #region Panel's elements' Methods
    public void ChangeVolume(float value)
    {
        battleBGM.volume = (float)value / 100;
        PlayerPrefs.SetInt("Volume", Mathf.RoundToInt(value));
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void BackToMainMenu()
    {

    }
    #endregion

    #region Methods to adjust the panel for pause/stop mode
    public void DisplayPausePanel()
    {
        panelTitle.text = "PAUSE";
        replayBtnText.text = "Restart";

        gameObject.SetActive(true);
        closePanelBtn.SetActive(true);
        pauseBtn.SetActive(false);

        Time.timeScale = 0f;
    }

    public void HidePausePanel()
    {
        closePanelBtn.SetActive(false);
        gameObject.SetActive(false);
        pauseBtn.SetActive(true);

        Time.timeScale = 1f;
    }

    public void DisplayVictoryPanel()
    {
        panelTitle.text = "VICTORY";
        replayBtnText.text = "Replay";

        gameObject.SetActive(true);
        pauseBtn.SetActive(false);

        Time.timeScale = 0f;
    }

    public void DisplayDefeatPanel()
    {
        panelTitle.text = "DEFEAT";
        replayBtnText.text = "Replay";

        gameObject.SetActive(true);
        pauseBtn.SetActive(false);

        Time.timeScale = 0f;
    }
    #endregion
}
