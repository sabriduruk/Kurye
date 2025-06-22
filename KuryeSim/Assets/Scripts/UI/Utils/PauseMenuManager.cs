using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject gameCanvas;
    public GameObject pauseCanvas;
    public TextMeshProUGUI countdownText;
    public GameObject countDownObject;

    public GameObject sfxSlider;
    public GameObject musicSlider;

    public GameObject devamButonu;
    public GameObject returnToMainMenuButton;
    public GameObject title;

    public AudioSource motorSound;


    private bool isPaused = false;
    private bool isCountingDown = false;
    void Start()
    {
        Time.timeScale = 1f;
        pauseCanvas.SetActive(false);
        gameCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else if (!isCountingDown)
            {
                StartCoroutineResume();
            }
        }
    }
    public void StartCoroutineResume() {
        StartCoroutine(ResumeCountdown());
    }
    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        motorSound.Pause();
        countDownObject.SetActive(false);
        devamButonu.SetActive(true);
        returnToMainMenuButton.SetActive(true);
        musicSlider.SetActive(true);
        sfxSlider.SetActive(true);
    }
    public IEnumerator ResumeCountdown()
    {
        countDownObject.SetActive(true);
        isCountingDown = true;
        devamButonu.SetActive(false);
        musicSlider.SetActive(false);
        sfxSlider.SetActive(false);
        returnToMainMenuButton.SetActive(false);
        title.SetActive(false);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        
        ResumeGame();
        isCountingDown = false;
        
    }
    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        motorSound.UnPause();
    }
}
