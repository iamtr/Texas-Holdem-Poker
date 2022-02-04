using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoldemHand;

public class PlayerScript : MonoBehaviour
{
	public DeckScript deckScript;
	public CardScript[] hand;
	public int handCardIndex = 0;
	public Sprite cardback;
	private Hand myHand;
	public GameManager gameManager;
	[SerializeField] public uint myHandValue;


	public void StartHand()
	{
		GetCard();
		GetCard();
	}
	private void GetCard()
	{
		deckScript.DealPlayerCard(hand, handCardIndex);
		handCardIndex++;
	}
	public string GenerateFormatting()
	{
		string returnString = hand[0].Formatting() + " " + hand[1].Formatting();
		return returnString;
	}
	public void ResetHand()
	{
		handCardIndex = 0;
		hand[0].ResetCards();
		hand[1].ResetCards();
	}




}
