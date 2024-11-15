using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Update()
    {
        
        if (Input.GetButtonDown("Cancel"))
        {
            LoadMainMenu();  
        }
    }

    public void LoadMainMenu()
    {
        
        SceneManager.LoadScene(0); 
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(1); // Загружаем игровую сцену (сцену, которая указана для новой игры)
        Time.timeScale = 1; // Важно, чтобы игра продолжалась без остановки
        PlayerProgress.ResetProgress(); // Если нужно, сбрасываем прогресс
    }

    public void ContinueGame()
    {
        if (PlayerProgress.HasProgress())
        {
            int savedSceneIndex = PlayerProgress.GetSavedSceneIndex();
            SceneManager.LoadScene(savedSceneIndex);  // Загружаем сохраненную сцену
        }
        else
        {
            Debug.Log("Нет сохранённого прогресса.");
            StartNewGame();  // Если нет сохранений, запускаем новую игру
        }
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Для редактора
        #else
            Application.Quit(); // Для игры
        #endif
    }
}
