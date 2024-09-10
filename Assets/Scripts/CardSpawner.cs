using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;  
    public RectTransform gridTransform;  
    public List<Sprite> cardSprites; 
    public int rows = 4;  
    public int columns = 4;  
    public float spacingPercentage = 0.05f;

    private List<int> cardValues = new List<int>();
    internal List<Card> spawnedCards = new List<Card>();

    void Start()
    {
        SpawnCards(rows, columns);

        SaveLoadData.Instance.LoadGame(spawnedCards);
    }

    public void SpawnCards(int rows, int columns)
    {
       
        GenerateCardValues(rows, columns);

        ShuffleList(cardValues);

       
        float gridWidth = gridTransform.rect.width;
        float gridHeight = gridTransform.rect.height;


        float cardWidth = gridWidth / columns;
        float cardHeight = gridHeight / rows;


        float spacingX = cardWidth * spacingPercentage;
        float spacingY = cardHeight * spacingPercentage;


        cardWidth = ( (gridWidth - (columns - 1) * spacingX) / columns) * (0.99f);
        cardHeight =( (gridHeight - (rows - 1) * spacingY) / rows ) * (0.99f);

        Vector2 startPos = new Vector2(-gridWidth / 2f + cardWidth / 2f, gridHeight / 2f - cardHeight / 2f) + new Vector2(5,-5);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                
                GameObject cardObject = Instantiate(cardPrefab, gridTransform);

               
                RectTransform cardTransform = cardObject.GetComponent<RectTransform>();
                cardTransform.sizeDelta = new Vector2(cardWidth, cardHeight);  

                
                cardTransform.anchoredPosition = new Vector2(startPos.x + j * (cardWidth + spacingX), startPos.y - i * (cardHeight + spacingY));

                
                Card card = cardObject.GetComponent<Card>();

               
                int cardValue = cardValues[i * columns + j];
                card.SetCardValue(cardValue);
                card.SetCardSprite(cardSprites[cardValue]);

                
                spawnedCards.Add(card);
            }
        }
    }

    private void GenerateCardValues(int rows, int columns)
    {
        cardValues.Clear();

        int totalCards = rows * columns;
        if (totalCards % 2 != 0)
        {
            Debug.LogError("The number of cards must be even.");
            return;
        }

        int pairs = totalCards / 2;

        GameManager.Instance.totalPair = pairs;
        for (int i = 0; i < pairs; i++)
        {
            int value = i % cardSprites.Count; 
            cardValues.Add(value);
            cardValues.Add(value); 
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
