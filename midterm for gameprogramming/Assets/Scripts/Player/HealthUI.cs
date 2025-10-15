using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Image[] hearts;

    private PlayerHealth playerHealth;

    void Update()
    {
        
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth == null)
                {
                    playerHealth = player.GetComponentInChildren<PlayerHealth>();
                    if (playerHealth == null)
                        return; 
                }
            }
            else
            {
                return; 
            }
            health = playerHealth.health;
            maxHealth = playerHealth.maxHealth;
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i] == null) continue;
                if (fullHeart == null || emptyHeart == null) continue; 
                if (i < health)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }
                if (i < maxHealth)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }
            }

        }
    }
}
