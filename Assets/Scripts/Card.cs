using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public static readonly string Show = "Show";
    public static readonly string Hide = "Hide";

    public Animator animator;
    public Image spriteRenderer;  

    [SerializeField]
    bool isFlipped = false;
    [SerializeField]
    int cardValue;
    [SerializeField]
    AudioSource cardFlip;

    public delegate void CardFlipped(Card card);  
    public static event CardFlipped OnCardFlipped;

    public bool IsFlipped()
    {
        return isFlipped;
    }

    public void CardButton()
    {
        if (CardManager.Instance.flippedCards.Count >= 2 || CardManager.Instance.isCheckingMatch) 
        {
            return;
        }
        
        if (!isFlipped)
        {
            ShowCard();
            isFlipped = true;

            
            OnCardFlipped?.Invoke(this);  
        }
    }

    public void ShowCard()
    {
        animator.SetBool(Show, true);
        animator.SetBool(Hide, false);
    }

    public void HideCard()
    {
        animator.SetBool(Show, false);
        animator.SetBool(Hide, true);
        isFlipped = false;  
    }

    public void FlipBack()
    {
        StartCoroutine(FlipBackCoroutine());
    }

    private IEnumerator FlipBackCoroutine()
    {
        yield return new WaitForSeconds(0.6f); 
        HideCard();
    }

    public void DoneImgeOpen()
    {
        gameObject.SetActive(false);
    }

    public void SetCardValue(int value)
    {
        cardValue = value;
       
    }

    public int GetCardValue()
    {
        return cardValue;
    }

    public void SetCardSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite; 
    }

   

    public void CardFlipSound()
    {
        cardFlip.Play();
    }
}
