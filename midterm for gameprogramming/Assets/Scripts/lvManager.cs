using UnityEngine;
using UnityEngine.SceneManagement;

public class lvManager : MonoBehaviour
{
    public string nextLevelName;

    void Update()
    {
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        
        if (enemies.Length == 0)
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }
}
