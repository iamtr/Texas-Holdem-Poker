using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using HoldemHand;

public class GameManager : MonoBehaviour
{
	public enum CurrentGameState
	{
		preflop,
		flop,
		turn, 
		river
	}

	public DeckScript deckScript;
	private CardScript[] allCards = new CardScript[7];
	private CardScript[] boardCards = new CardScript[5];
	public string boardString = "";
	[SerializeField] private List<Hand> handList = new List<Hand>();
	[SerializeField] private List<PlayerScript> playerScripts = new List<PlayerScript>();
	[SerializeField] private List<GameObject> winner = new List<GameObject>();
	public Dictionary<PlayerScript, uint> keyValuePairs = new Dictionary<PlayerScript, uint>();
	[SerializeField] private int minBetValue;
	[SerializeField] private int currentPlayerIndex = 0;
	[SerializeField] private int callAmount;
	[SerializeField] private int potValue = 0;
	[SerializeField] private bool playerActed;

	[SerializeField] private Text winText;
	[SerializeField] private Text minBetText;
	[SerializeField] private Text potText;

	[SerializeField] private Button callButton;
	[SerializeField] private Button raiseButton;
	[SerializeField] private Button foldButton;
	[SerializeField] private Button checkButton;
	[SerializeField] private Button allInButton;
	private void Start()
	{
		minBetText.text = String.Format($"Min Bet: {minBetValue}");
	}
	public void DealClicked()
	{
		Debug.Log("deal clicked");
		ResetGame();
		winText.gameObject.SetActive(false);
		for (int i = 0; i < playerScripts.Count ; i++)
			playerScripts[i].StartHand();// player gets cards
		deckScript.DealCommunityCards();
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
			Debug.Log("handvalue: " + handList[i].HandValue);
			keyValuePairs.Add(playerScripts[i], handList[i].HandValue);
		}
	}
	public void CheckClicked()
	{ 
		//StoreHandValues();
		CompareHandValues();
		GetWinner();
	}
	public void RaiseClicked()
	{

	}	
	public void FoldClicked()
	{

	}
	public void AllInClicked()
	{

	}
	public void CallClicked()
	{
		playerScripts[currentPlayerIndex].cash -= callAmount;
		potValue += callAmount;
		UpdateUI();
		playerScripts[currentPlayerIndex].UpdateUI();
	}
	private void UpdateUI()
	{
		potText.text = $"Pot: {potValue}";
	}
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
		Array.Clear(allCards, 0, allCards.Length);
		Array.Clear(boardCards, 0, boardCards.Length);
		handList.Clear();
		winner.Clear();
		keyValuePairs.Clear();
		boardString = "";
	}
	private void GetWinner()
	{
		for (int i = 0; i < winner.Count; i++)
			Debug.Log("win: " + winner[i].name);
		winText.gameObject.SetActive(true);
		if (winner.Count == 1) winText.text = winner[0].name + " wins!";
		else
		{
			winText.text = "Tie: ";
			for (int i = 0; i < winner.Count; i++)
			{
				winText.text += winner[i].name;
				if (i != winner.Count - 1) winText.text += ",";
				winText.text += " ";
			}
		}

	}
	private void StartTurn()
	{
		currentPlayerIndex = 0;
		playerScripts[currentPlayerIndex].isActivePlayer = true;
	}

	public void MoveToNextPlayer()
	{
		playerScripts[currentPlayerIndex].isActivePlayer = false;
		currentPlayerIndex++;
		if (currentPlayerIndex / playerScripts.Count == 1) currentPlayerIndex = 0;
		playerScripts[currentPlayerIndex].isActivePlayer = true;
	}
}



