using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void LoadGameplay()
    {
        GameSceneManager.Instance.LoadScene("Gameplay");
    }

    public void LoadMainMenu()
    {
        GameSceneManager.Instance.LoadScene("MainMenu");
    }

    public void LoadSettings()
    {
        GameSceneManager.Instance.LoadScene("Settings");
    }

    public void LoadCredits()
    {
        GameSceneManager.Instance.LoadScene("Credits");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenPauseMenu(GameObject pauseMenu)
    {
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}