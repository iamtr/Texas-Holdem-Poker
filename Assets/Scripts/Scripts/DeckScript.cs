using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
	public Sprite[] deckSprite;
	public int[] deckValue = new int[52];
	public int[] deckSuit = new int[52];
	public CardScript[] communityCards;
	public Sprite cardback;
	private int deckHandIndex = 0; // shows which card out of 5 community cards dealt
	private int deckCardIndex; // shows position of dealt card on deck
	List<PlayerScript> playerScripts = new List<PlayerScript>();

	private void Start()
	{
		GetCardValues();
		Shuffle();
	}
	private void GetCardValues()
	{
		for (int i = 0; i < deckSprite.Length; i++)
		{
			int j = (i % 13) + 1;
			int k = i / 13;
			k = Mathf.FloorToInt(k);
			//Debug.Log(k);
			deckValue[i] = j;
			deckSuit[i] = k;
			/*
			switch (k) 
			{
				case 0:
					deckSuit[i] = "Clubs";
					break;
				case 1:
					deckSuit[i] = "Diamonds";
					break;
				case 2:
					deckSuit[i] = "Hearts";
					break;
				case 3:
					deckSuit[i] = "Spades";
					break;
				default:
					Debug.Log("Switch case error");
					break;
			}
			*/
		}
	}
	public void Shuffle()
	{
		for (int i = 0; i < deckSprite.Length; i++)
		{
			int j = Random.Range(0, 51);
			Sprite tempSprite = deckSprite[i];
			int tempSuit = deckSuit[i];
			int tempValue = deckValue[i];
			deckSprite[i] = deckSprite[j];
			deckValue[i] = deckValue[j];
			deckSuit[i] = deckSuit[j];
			deckSprite[j] = tempSprite;
			deckValue[j] = tempValue;
			deckSuit[j] = tempSuit;
		}	
	}
	public void DealCommunityCard()
	{
		communityCards[deckHandIndex].SetValue(deckValue[deckCardIndex]);
		communityCards[deckHandIndex].SetSuit(deckSuit[deckCardIndex]);
		communityCards[deckHandIndex].SetSprite(deckSprite[deckCardIndex]);
		deckCardIndex++;
		deckHandIndex++;
	}

	public void DealCommunityCards()
	{
		DealCommunityCard();
		DealCommunityCard();
		DealCommunityCard();
		DealCommunityCard();
		DealCommunityCard();
	}

	public void DealPlayerCard(CardScript[] hand, int handCardIndex)
	{
		hand[handCardIndex].SetValue(deckValue[deckCardIndex]);
		hand[handCardIndex].SetSuit(deckSuit[deckCardIndex]);
		hand[handCardIndex].SetSprite(deckSprite[deckCardIndex]);
		deckCardIndex++;
	}
	public void ResetDeck()
	{
		deckCardIndex = 0;
		deckHandIndex = 0;
		for (int i = 0; i < communityCards.Length; i++)
		{
			communityCards[i].ResetCards();
		}
	}
}
