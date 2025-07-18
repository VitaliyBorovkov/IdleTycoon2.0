using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnPlayClicked()
    {
        Debug.Log("[MainMenu] Play button clicked. Loading GameScene...");
        SceneManager.LoadScene("GameScene"); // убедись, что GameScene добавлена в Build Settings
    }

    public void OnExitClicked()
    {
        Debug.Log("[MainMenu] Exit button clicked. Quitting application...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}