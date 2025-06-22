using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MotorUIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public MotorData motorData;
    public TextMeshProUGUI speedText;
    public Button increaseButton;

    public Button playButton;

    public Button colorChange;
    
    public Image showedMotorImage;
    public Vector2 selectedOffset;
    public int motorIndex = 0;

    public Sprite[] motorImages;
    public Vector2[] colorOffsets;

    void Start()
    {
        UpdateSpeedText();
        increaseButton.onClick.AddListener(OnIncreaseClicked);
        colorChange.onClick.AddListener(OnColorChange);
        playButton.onClick.AddListener(loadScreen);


        showedMotorImage.sprite = motorImages[0];
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnIncreaseClicked() {
        motorData.maxForwardVelocity += 1f;
        UpdateSpeedText();
    }
    void UpdateSpeedText() {
        speedText.text = "MaxSpeed: " + motorData.maxForwardVelocity.ToString();
    }
    void OnColorChange() 
    {
        showedMotorImage.sprite = motorImages[++motorIndex % motorImages.Length];
        
        
    }
    void loadScreen() {
        SceneManager.LoadScene("Main");
    }
   
}
