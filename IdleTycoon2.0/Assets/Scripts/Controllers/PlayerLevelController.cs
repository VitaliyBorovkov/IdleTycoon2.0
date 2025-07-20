using UnityEngine;

public class PlayerLevelController : MonoBehaviour
{
    [SerializeField] private PlayerLevelView view;
    [SerializeField] private MonoBehaviour playerLevelServiceSource;

    private IPlayerLevelService playerLevelService;

    private void Start()
    {
        playerLevelService = playerLevelServiceSource as IPlayerLevelService;

        view.SetLevel(playerLevelService.CurrentLevel);

        playerLevelService.OnLevelUp += HandleLevelUp;
    }

    private void HandleLevelUp(int newLevel)
    {
        view.SetLevel(newLevel);
    }

    private void OnDestroy()
    {
        playerLevelService.OnLevelUp -= HandleLevelUp;
    }
}