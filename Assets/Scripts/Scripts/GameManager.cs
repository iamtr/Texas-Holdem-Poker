using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HoldemHand;

public class GameManager : MonoBehaviour
{
	public DeckScript deckScript;
	private CardScript[] allCards = new CardScript[7];
	private CardScript[] boardCards = new CardScript[5];
	//private List<Hand> equalHands = new List<Hand>();
	public string boardString = "";
	private List<Hand> handList = new List<Hand>();
	[SerializeField]
	private List<PlayerScript> playerScripts = new List<PlayerScript>();
	private List<GameObject> winner = new List<GameObject>();
	private Dictionary<PlayerScript, uint> keyValuePairs = new Dictionary<PlayerScript, uint>();
	

	public void DealClicked()
	{
		ResetGame();
		for (int i = 0; i < playerScripts.Count ; i++)
			playerScripts[i].StartHand();// player gets cards
		deckScript.DealCommunityCards();
		keyValuePairs.Clear();
		for (int i = 0; i < deckScript.communityCards.Length; i++)
		{
			boardCards[i] = deckScript.communityCards[i];
			boardString += boardCards[i].Formatting();
			if (!(i == allCards.Length)) boardString += " ";
		}
		for (int i = 0; i < playerScripts.Count; i++)
		{
			handList.Add(new Hand(playerScripts[i].GenerateFormatting(), boardString));
			playerScripts[i].myHandValue = handList[i].HandValue;
			keyValuePairs.Add(playerScripts[i], handList[i].HandValue);
		}
	}
	public void CheckClicked()
	{

		//StoreHandValues();
		CompareHandValues();
		GetWinner();
	}

/*	private void StoreHandValues()
	{
		keyValuePairs.Clear();
		for (int i = 0; i < playerScripts.Count; i++)
		{
			handList.Add(new Hand(playerScripts[i].GenerateFormatting(), boardString));
			keyValuePairs.Add(playerScripts[i], handList[i].HandValue);
		}
	}*/

	private void CompareHandValues()
	{
		/*for (int i = 0; i < handList.Count; i++)
			Debug.Log("handMask: " + handList[i].HandValue);*/
		uint maxValue = keyValuePairs.Values.Max();
		var maxResult = keyValuePairs.Where(x => x.Value == maxValue).Select(x => new { x.Key, x.Value }).ToList();
		
		if (maxResult.Count > 1) 
			Debug.Log("more than 1");
		else Debug.Log("only 1");
		foreach (var item in maxResult)
		{
			winner.Add(item.Key.gameObject);
			Debug.Log("key: " + item.Key + "value: " + item.Value);
		}
	}
	private void ResetGame()
	{
		deckScript.Shuffle();
		foreach (PlayerScript playerScript in playerScripts)
			playerScript.ResetHand();
		deckScript.ResetDeck();
	}
	private void GetWinner()
	{
		for (int i = 0; i < winner.Count; i++)
			Debug.Log("win: " + winner[i].name);
	}
}



