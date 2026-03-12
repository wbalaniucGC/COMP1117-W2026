using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image fader;
    [SerializeField] private GameObject menuPanel;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1.0f;

    private void Awake()
    {
        // Disable menu panel. Set fader to be transparent.
        menuPanel.SetActive(false);
        fader.color = new Color(0, 0, 0, 0);
    }

    public void ShowGameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        float elapsed = 0;
        Color targetColor = new Color(0.1f, 0.1f, 0.1f, 0.8f); // Dark grey

        // Fade to grey
        while(elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fader.color = Color.Lerp(new Color(0, 0, 0, 0), targetColor, elapsed);
            yield return null;
        }

        // Show buttons and enable cursor
        menuPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void QuitToMenu() => SceneManager.LoadScene("MainMenu");
}
