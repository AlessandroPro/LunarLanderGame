using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Utility functions for managing the GUI for the game scene
public class GameUIManager : MonoBehaviour
{
    public Text scoreText;
    public Text fuelText;
    public Text messageText;
    public Button retryButton;
    public Button menuButton;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = "SCORE\n" + score.ToString();
    }

    public void UpdateFuelText(float fuel)
    {
        float fuelTextVal = Mathf.Round(fuel * 100f) / 100f;
        fuelText.text = "FUEL\n" + fuelTextVal.ToString();
    }

    public void UpdateMessageText(string message)
    {
        messageText.text = message;
        ShowMessageText(true);
    }

    public void ShowMessageText(bool show)
    {
        messageText.gameObject.SetActive(show);
    }

    public void SetupEndGame()
    {
        UpdateMessageText("GAME OVER!");
        retryButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        scoreText.text = "FINAL " + scoreText.text;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
