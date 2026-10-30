using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SpaceBossRestartButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            var game = FindFirstObjectByType<SpaceBossGame>();
            if (game != null) game.Restart();
        });
    }
}
