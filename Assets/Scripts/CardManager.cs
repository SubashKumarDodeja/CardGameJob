using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip matchClip;
    [SerializeField]
    AudioClip notMatchClip;



    [SerializeField]
    internal CardSpawner cardSpawner;
    private void Awake()
    {
        Instance = this;
    }

    public List<Card> flippedCards = new List<Card>();  
    public bool isCheckingMatch = false;

    private void OnEnable()
    {
        Card.OnCardFlipped += HandleCardFlipped;
    }

    private void OnDisable()
    {
        Card.OnCardFlipped -= HandleCardFlipped;
    }

    private void HandleCardFlipped(Card card)
    {
        flippedCards.Add(card);

        if (flippedCards.Count == 2 && !isCheckingMatch)
        {
            StartCoroutine(CheckForMatch());
        }
    }



    private IEnumerator CheckForMatch()
    {
        isCheckingMatch = true;

        yield return new WaitForSeconds(0.8f);  

        if (flippedCards[0].GetCardValue() == flippedCards[1].GetCardValue())
        {
            
            Debug.Log("Match found!");
            flippedCards[0].DoneImgeOpen();  
            flippedCards[1].DoneImgeOpen();
            audioSource.clip = matchClip;
            audioSource.Play();
            bool save = GameManager.Instance.EarnPoints();
            if(save)
                SaveLoadData.Instance.SaveGame(cardSpawner.spawnedCards);
        }
        else
        {
            Debug.Log("No match.");
            flippedCards[0].FlipBack();
            flippedCards[1].FlipBack();
            audioSource.clip = notMatchClip;
            audioSource.Play();
            GameManager.Instance.DeductPoints();
        }

        flippedCards.Clear();
        isCheckingMatch = false;
    }
}
