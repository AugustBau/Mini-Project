using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarImage : MonoBehaviour
{
    public Health playerHealth;
    public Image fillImage; // assign the Fill image here

    void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealthBarImage: playerHealth not assigned");
            enabled = false;
            return;
        }
        if (fillImage == null)
        {
            Debug.LogError("PlayerHealthBarImage: fillImage not assigned");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        float t = (float)playerHealth.CurrentHealth / playerHealth.maxHealth;
        fillImage.fillAmount = t;
    }
}
