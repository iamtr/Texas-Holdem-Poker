using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoldemHand;

public class GameManager : MonoBehaviour
{
	public DeckScript deckScript;
	public PlayerScript player1Script;
	public PlayerScript player2Script;
	private CardScript[] allCards = new CardScript[7];
	private CardScript[] boardCards = new CardScript[5];
	private CardScript[] playerCards = new CardScript[2];
	
	[SerializeField]
	private List<PlayerScript> playerScripts = new List<PlayerScript>();
	public void DealClicked()
	{
		ResetGame();
		foreach (PlayerScript playerScript in playerScripts)
			playerScript.StartHand();// player gets cards
		deckScript.DealCommunityCards();
	}
	public void CheckNew()
	{
		for (int i = 0; i < 5; i++)
		{
			boardCards[i] = deckScript.communityCards[i];
		}
		/*for (int i = 0; i < 2; i++)
		{
			foreach (PlayerScript playerScript in playerScripts) 
				playerScript.hand[i];
		}*/

		string boardString = "";

		for(int i = 0; i < boardCards.Length; i++)
		{
			boardString += boardCards[i].Formatting();
			if (!(i == allCards.Length)) boardString += " ";
		}

		Hand h1 = new Hand(player1Script.GenerateFormatting(), boardString);
		Hand h2 = new Hand(player2Script.GenerateFormatting(), boardString);

		//Debug.Log("h1: " + h1);
		Debug.Log("h1: " + h1.HandTypeValue);
		Debug.Log("h2: " + h2.HandTypeValue);
		if (h1 > h2) Debug.Log("h1 > h2");
		else Debug.Log("h2 > h1");
	}
	private void ResetGame()
	{
		deckScript.Shuffle();
		foreach (PlayerScript playerScript in playerScripts)
		{
			playerScript.ResetHand();
		}
		deckScript.ResetDeck();
	}
}



