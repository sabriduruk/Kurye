using UnityEngine;

[CreateAssetMenu(fileName = "MotorData", menuName = "Motor/Datas", order = 1)]
public class MotorData : ScriptableObject
{
    [Header("Hareket Ayarları")]
    public float maxForwardVelocity = 35f;
    public float maxSteerVelocity = 2.5f;
    public float accelerationMultiplier = 3.5f;
    public float brakeMultiplier = 15f;
    public float steeringMultiplier = 10f;


    [Header("Yatma Efekti")]
    public float maxLeanAngle = 35f; // Maksimum yatma açısı
    public float leanSpeed = 5f; // Yatma hızı
    [Tooltip("Sürücünün yatma açısını motorsikletle aynı tutmak için 1, farklı yapmak için değeri değiştirin")]
    public float riderLeanMultiplier = 1.0f; // Sürücünün yatma çarpanı
    [Tooltip("Sürücü yatma efektini kaydetmek veya düzeltmek için offset")]
    public Vector3 riderLeanOffset = Vector3.zero; // Sürücü yatma offset'i
    
    [Header("Tekerlek Ayarları")]
    public float wheelRadius = 0.35f; // tekerlek yarıçapı
    public float maxSteeringAngle = 30f; // ön tekerleğin maksimum dönüş açısı
    public float handlebarRotationMultiplier = 0.8f; // gidon dönüş çarpanı

    
    public Texture2D motorTexture;


}
