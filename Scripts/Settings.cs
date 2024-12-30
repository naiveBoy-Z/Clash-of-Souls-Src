using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public TMP_InputField playerNameInputField;
    public Slider volume;
    public AudioSource mainMenuBGM;

    private void Start()
    {
        playerNameInputField.text = PlayerPrefs.GetString("PlayerName", "");
        playerNameInputField.onEndEdit.AddListener(OnFinishTypingName);

        volume.value = PlayerPrefs.GetInt("Volume", 100);
        volume.onValueChanged.AddListener(OnVolumeChanged);

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        volume.value = PlayerPrefs.GetInt("Volume", 100);
    }

    private void Update()
    {
        if (playerNameInputField.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            playerNameInputField.DeactivateInputField();
        }
    }

    private void OnFinishTypingName(string text)
    {
        PlayerPrefs.SetString("PlayerName", text);
    }

    private void OnVolumeChanged(float value)
    {
        mainMenuBGM.volume = (float) value / 100;
        PlayerPrefs.SetInt("Volume", Mathf.RoundToInt(value));
    }

    public void OpenSettings()
    {
        gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }
}
