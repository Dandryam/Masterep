using UnityEngine;

public static class PlayerProgress
{
    private const string ProgressKey = "savedSceneIndex";

    // Проверка наличия сохранённого прогресса
    public static bool HasProgress()
    {
        return PlayerPrefs.HasKey(ProgressKey);
    }

    // Получение сохранённого индекса сцены
    public static int GetSavedSceneIndex()
    {
        return PlayerPrefs.GetInt(ProgressKey);
    }

    // Сохранение текущего индекса сцены
    public static void SaveProgress(int sceneIndex)
    {
        PlayerPrefs.SetInt(ProgressKey, sceneIndex);
        PlayerPrefs.Save();
    }

    // Сброс прогресса (например, для новой игры)
    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(ProgressKey);
        PlayerPrefs.Save();
    }
}
