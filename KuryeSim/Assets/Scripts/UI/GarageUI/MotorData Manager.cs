using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MotorDataManager : MonoBehaviour
{
    [Header("global settings")]
    public MotorData globalMotorData;
    public PlayerVars playerVars;
    public AudioSource audioSource;
    public AudioClip colorUpgradeClip;
    public AudioClip motorUpgradeClip;
    public AudioClip accelerationUpgradeClip;
    

    [Header("Motor Color Settings")]
    
    public int colorIndex = 0;
    public Texture2D[] textures;
    public Material motorMaterial;

    [Header("Texts")]
    public TextMeshProUGUI maxVelocityText;
    public TextMeshProUGUI accelerationText;

    public TextMeshProUGUI velocityPriceText;
    public TextMeshProUGUI accelerationPriceText;

    public TextMeshProUGUI currentPriceText;
    
    private int currentMotorPrice;
    private int currentAccelarationPrice;

    [Header("Buttons")]
    public GameObject motorButton;
    public GameObject accelerationButton;


    private void Awake() {
        motorMaterial.mainTexture = globalMotorData.motorTexture;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        motorMaterial.mainTexture = globalMotorData.motorTexture;
        setPrice();

    }

    public void rightButton() {
        colorIndex = (colorIndex + 1) % textures.Length;
        motorMaterial.mainTexture = textures[colorIndex];
        globalMotorData.motorTexture = textures[colorIndex];
        
        audioSource.PlayOneShot(colorUpgradeClip);
    }

    public void leftButton() {
        colorIndex = (colorIndex - 1 + textures.Length) % textures.Length;
        motorMaterial.mainTexture = textures[colorIndex];
        globalMotorData.motorTexture = textures[colorIndex];
        
        audioSource.PlayOneShot(colorUpgradeClip);
    }
    public void maxhizButton() {
        if(playerVars.totalMoney >= currentMotorPrice) {
            
            audioSource.PlayOneShot(motorUpgradeClip);
            globalMotorData.maxForwardVelocity += 1f;
            playerVars.totalMoney -= currentMotorPrice;
            playerVars.motorPowerLEVEL++;
            setPrice();
        }


    }
    public void hizlanmaButton() {
        if(playerVars.totalMoney >= currentAccelarationPrice) {
            
            audioSource.PlayOneShot(accelerationUpgradeClip);
            globalMotorData.accelerationMultiplier += 0.25f;
            playerVars.totalMoney -= currentAccelarationPrice;
            playerVars.accelerationLEVEL++;
            setPrice();
        }
    }

    void setPrice() 
    {
        switch(playerVars.motorPowerLEVEL) {
            case 1:
                currentMotorPrice = 500;
                velocityPriceText.text = "Ücret: " + currentMotorPrice.ToString();
            break;
            case 2:
                currentMotorPrice = 1000;
                velocityPriceText.text = "Ücret: " + currentMotorPrice.ToString();
                break;
            case 3:
                currentMotorPrice = 0;
                velocityPriceText.text = "Max Hız'a ulaşıldı: ";
                motorButton.SetActive(false);
                break;
        }

        switch(playerVars.accelerationLEVEL) {
            case 1:
                currentAccelarationPrice = 1000;
                accelerationPriceText.text = "Ücret: " + currentAccelarationPrice.ToString();
            break;
            case 2:
                currentAccelarationPrice = 2000;
                accelerationPriceText.text = "Ücret: " + currentAccelarationPrice.ToString();
                break;
            case 3:
                currentMotorPrice = 0;
                accelerationPriceText.text = "Max Hızlanma seviyesine Ulaşıldı";
                accelerationButton.SetActive(false);
                break;
        }
        currentPriceText.text = playerVars.totalMoney.ToString();
    }
    
}
