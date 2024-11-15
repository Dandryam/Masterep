using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadLevelByName(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void EndLevel()
    {
        // Сохраняем индекс текущей сцены
        PlayerProgress.SaveProgress(SceneManager.GetActiveScene().buildIndex);
        
        // Загружаем следующую сцену или возвращаемся в меню
        SceneManager.LoadScene("MainMenu");
    }
}
