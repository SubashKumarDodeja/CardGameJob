using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public float currentPoints;
    public int earnPairs;
    public List<CardData> cardDataList = new List<CardData>();
}


[System.Serializable]
public class CardData
{
    public int cardIndex;  
    public int cardValue;  
    public bool isHidden; 
    public bool isFlipped; 
}

public class SaveLoadData : MonoBehaviour
{
    public static SaveLoadData Instance;

    private string saveFilePath;


    private void Awake()
    {
        Instance = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public void SaveGame(List<Card> allCards)
    {
        SaveData data = new SaveData();
        data.currentPoints = GameManager.Instance.currentPoints;
        data.earnPairs = GameManager.Instance.earnPairs;

        for (int i = 0; i < allCards.Count; i++)
        {
            Card card = allCards[i];
            CardData cardData = new CardData
            {
                cardIndex = i,
                cardValue = card.GetCardValue(),
                isHidden = !card.gameObject.activeSelf,  
                isFlipped = card.IsFlipped() 
            };
            data.cardDataList.Add(cardData);
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved.");
    }

    public void LoadGame(List<Card> allCards)
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            GameManager.Instance.currentPoints = data.currentPoints;
            GameManager.Instance.earnPairs = data.earnPairs;
            GameManager.Instance.UpdateScore();
            for (int i = 0; i < data.cardDataList.Count; i++)
            {
                CardData cardData = data.cardDataList[i];
                Card card = allCards[cardData.cardIndex];

                card.SetCardValue(cardData.cardValue);  
                card.SetCardSprite(CardManager.Instance.cardSpawner.cardSprites[cardData.cardValue]);  

                if (cardData.isHidden)
                {
                    card.gameObject.SetActive(false);  
                }

                if (cardData.isFlipped && card.gameObject.activeSelf)
                {
                    card.ShowCard(); 
                }
                else
                {
                    card.HideCard();
                }
            }

            Debug.Log("Game loaded.");
        }
        else
        {
            Debug.Log("No save file found.");
        }
    }


    [ContextMenu("Delete File")]
    public void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted.");
        }
    }
}
