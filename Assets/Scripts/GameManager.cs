using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    public GameState currentState;
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Current Stat Display")]
    public TMP_Text currentHealthDisplay;
    public TMP_Text currentRecoveryDisplay;
    public TMP_Text currentMoveSpeedDisplay;
    public TMP_Text currentMightDisplay;
    public TMP_Text currentProjectileSpeedDisplay;
    public TMP_Text currentMagnetDisplay;

    [Header("Results Screen Displays")]
    public Image chosenCharacterImage;
    public TMP_Text chosenCharacterName;
    public TMP_Text levelReachedDisplay;
    public TMP_Text timeSurvivedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(InventoryManager.MAX_SLOTS);
    public List<Image> chosenPassiveItemsUI = new List<Image>(InventoryManager.MAX_SLOTS);

    [Header("Stopwatch")]
    public float timeLimit; // The time limit in seconds
    float stopwatchTime; // The current time elapsed since the stopwatch started
    public TMP_Text stopwatchDisplay;

    public bool isGameOver = false;
    public bool choosingUpgrades = false;

    public delegate void TimeUpHandler();
    public static event TimeUpHandler OnTimeUp;
    
    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else {
            Debug.LogWarning("EXTRA " + this + " DELETED");
            Destroy(gameObject);
        }
        DisableScreens();
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;
            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                if(!isGameOver) {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                Time.timeScale = 0f;
                levelUpScreen.SetActive(true);
                break;
            default:
                Debug.Log("Invalid Current State");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if(currentState != GameState.Paused) {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        } 
    }

    public void ResumeGame()
    {
        if(currentState == GameState.Paused) {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }
    }

    void CheckForPauseAndResume()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(currentState == GameState.Paused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.CharName;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenWeaponsAndPassiveItemsUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponsData.Count != chosenWeaponsUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemsUI.Count) {
            Debug.Log("Chosen weapons and passive iems data lists have different lengths");
            return;
        }
        
        for (int i = 0; i < chosenWeaponsUI.Count; i++) {
            if (chosenWeaponsData[i].sprite) {
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;
            }
            else {
                chosenWeaponsUI[i].enabled = false;
            }   
        }

        for (int i = 0; i < chosenPassiveItemsUI.Count; i++) {
            if (chosenPassiveItemsData[i].sprite) {
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else {
                chosenPassiveItemsUI[i].enabled = false;
            }
            
        }
        
    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if(stopwatchTime >= timeLimit) {
            if(OnTimeUp != null) {
                OnTimeUp();
            }
        }
    }

    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        choosingUpgrades = true;
        ChangeState(GameState.LevelUp);
        Debug.Log("Start Level Up");
    }

    public void EndLevelUp()
    {
        choosingUpgrades = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);

        Debug.Log("End Level Up");
    }

}