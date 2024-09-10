using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

  
    private void Awake()
    {
        Instance = this;
    }

    public float pointOnMatch;
    public float pointLossOnUnMatch;
    [HideInInspector] public float currentPoints;

    [HideInInspector] public int totalPair;
    [HideInInspector] public int earnPairs;
    float comboScore;

    [SerializeField]
    AudioSource winSound;
    [SerializeField]
    Text currentScoreText;
    [SerializeField]
    GameObject restartBtn;


    public void UpdateScore()
    {
        currentScoreText.text = currentPoints.ToString();
    }
    public void DeductPoints()
    {
        currentPoints -= pointLossOnUnMatch;
        comboScore = 0;
        UpdateScore();
    }
    public bool EarnPoints()
    {
        currentPoints = currentPoints + pointOnMatch + (comboScore*pointOnMatch);
        comboScore += 0.5f;
        UpdateScore();
        return CheckGameOver();
    }
    bool CheckGameOver()
    {
        earnPairs++;
        if (earnPairs == totalPair)
        {
            winSound.Play();
            SaveLoadData.Instance.DeleteSaveFile();
            restartBtn.SetActive(true);
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Restart()
    {
        SaveLoadData.Instance.DeleteSaveFile();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
