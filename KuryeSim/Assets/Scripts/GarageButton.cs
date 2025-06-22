using UnityEngine;
using UnityEngine.SceneManagement;

public class GarageLoader : MonoBehaviour
{
    public void LoadGarage()
    {
        SceneManager.LoadScene("GarageUI");
    }
}