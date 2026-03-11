using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image fader;
    [SerializeField] private GameObject menuPanel;

    [Header("Checkpoint References")]
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private GameObject player;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1.0f;

    private void Awake()
    {
        // Disable menu panel. Set fader to be transparent.
        menuPanel.SetActive(false);
        fader.color = new Color(0, 0, 0, 0);
    }

    public void LoadFromCheckpoint()
    {
        // 1. Read the data from the Save Manager
        Vector3 playerPos = saveManager.LoadGame();

        // Safety check!
        if(playerPos != null)
        {
            // 2. Reactivate the player.
            player.SetActive(true);

            // 3. Set the player back to the checkpoint position and reset values
            player.transform.position = playerPos;

            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.ResetState();
            }

            // 4. Reset velocity so they don't spawn with "death" physics
            Rigidbody2D rBody = player.GetComponent<Rigidbody2D>();
            if (rBody != null)
            {
                rBody.linearVelocity = Vector2.zero;
                rBody.gravityScale = 5f;
                rBody.simulated = true;
            }

            // 5. Reset collisions
            Collider2D col = player.GetComponent<Collider2D>();
            if(col != null)
            {
                col.enabled = true;
            }

            // 6. Hide the game over menu and reset values
            menuPanel.SetActive(false);
            fader.color = new Color(0, 0, 0, 0);
        }
        else
        {
            Debug.LogError("No checkpoint file found");
        }
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
