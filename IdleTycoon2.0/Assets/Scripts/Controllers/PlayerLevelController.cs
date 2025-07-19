using UnityEngine;

public class PlayerLevelController : MonoBehaviour
{
    [SerializeField] private PlayerLevelView view;
    [SerializeField] private PlayerLevelSystem playerLevelSystem;

    private void Start()
    {
        view.SetLevel(playerLevelSystem.CurrentLevel);

        playerLevelSystem.OnLevelUp += HandleLevelUp;
    }

    private void HandleLevelUp(int newLevel)
    {
        view.SetLevel(newLevel);
    }

    private void OnDestroy()
    {
        playerLevelSystem.OnLevelUp -= HandleLevelUp;
    }
}