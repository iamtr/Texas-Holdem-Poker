using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using HoldemHand;

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		Preflop,
		Flop,
		Turn, 
		River
	}

	public DeckScript deckScript;
	private CardScript[] allCards = new CardScript[7];
	private CardScript[] boardCards = new CardScript[5];
	public string boardString = "";
	public Dictionary<PlayerScript, uint> keyValuePairs = new Dictionary<PlayerScript, uint>();
	[SerializeField] private bool playerActed;
	[SerializeField] private GameState gameState;

	[SerializeField] private List<Hand> handList = new List<Hand>();
	[SerializeField] private List<PlayerScript> playerScripts = new List<PlayerScript>();
	[SerializeField] private List<PlayerScript> participants = new List<PlayerScript>();
	[SerializeField] private List<GameObject> winner = new List<GameObject>();

	[SerializeField] private GameObject raiseUI;
	[SerializeField] private GameObject betUI;
	[SerializeField] private GameObject hideTurn;
	[SerializeField] private GameObject hideRiver;

	[SerializeField] private int minBetValue;
	[SerializeField] private int currentPlayerIndex;
	[SerializeField] private int callAmount;
	[SerializeField] private int potValue = 0;
	

	[SerializeField] private Text winText;
	[SerializeField] private Text gameStateText;
	[SerializeField] private Text potText;
	[SerializeField] private Text raiseErrorText;
	[SerializeField] private Text betErrorText;
	[SerializeField] private Text callText;

	[SerializeField] private Button callButton;
	[SerializeField] private Button raiseButton;
	[SerializeField] private Button foldButton;
	[SerializeField] private Button betButton;
	[SerializeField] private Button checkButton;
	[SerializeField] private Button allInButton;
	[SerializeField] private Button startButton;

	[SerializeField] private InputField raiseInputField;
	[SerializeField] private InputField betInputField;
	public void DealClicked()
	{
		ResetGame();
		winText.gameObject.SetActive(false);
		StartAllHands(); // player gets cards
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
	private void StartAllHands()
	{
		for (int i = 0; i < playerScripts.Count; i++)
			playerScripts[i].StartHand();
	}
	public void OldCheckClicked()
	{ 
		//StoreHandValues();
		CompareHandValues();
		GetWinner();
	}
	public void BetClicked()
	{
		betUI.SetActive(true);
	}
	public void BetEnterClicked()
	{
		if (int.Parse(betInputField.text) <= playerScripts[currentPlayerIndex].cash)
		{
			callAmount = int.Parse(betInputField.text);
			playerScripts[currentPlayerIndex].cash -= callAmount;
			potValue += callAmount;
			betUI.SetActive(false);
			playerScripts[currentPlayerIndex].UpdateUI();
			MoveToNextPlayer();
		}
		else if (int.Parse(raiseInputField.text) > playerScripts[currentPlayerIndex].cash)
			betErrorText.text = "Bet cannot be larger than own cash amount!";
	}
	public void CallClicked()
	{
		checkButton.gameObject.SetActive(false);
		playerScripts[currentPlayerIndex].cash -= callAmount;
		potValue += callAmount;
		playerScripts[currentPlayerIndex].UpdateUI();
		MoveToNextPlayer();
	}
	public void RaiseClicked()
	{
		raiseUI.SetActive(true);
	}	
	public void RaiseEnterClicked()
	{
		if (int.Parse(raiseInputField.text) >= callAmount * 2 && int.Parse(raiseInputField.text) <= playerScripts[currentPlayerIndex].cash)
		{
			callAmount = int.Parse(raiseInputField.text);
			playerScripts[currentPlayerIndex].cash -= callAmount;
			potValue += callAmount;
			raiseUI.SetActive(false);
			playerScripts[currentPlayerIndex].UpdateUI();
			MoveToNextPlayer();
		}
			
		else if (int.Parse(raiseInputField.text) <= callAmount * 2) raiseErrorText.text = "Raise amount must be at least 2x of call amount!";
		else if (int.Parse(raiseInputField.text) >= playerScripts[currentPlayerIndex].cash) raiseErrorText.text = "Not enough cash!";
			
	}
	public void FoldClicked()
	{
		playerScripts[currentPlayerIndex].isActivePlayer = false;
		Debug.Log(String.Format($"{playerScripts[currentPlayerIndex].gameObject.name} folds"));
		RemovePlayerFromList();
		if (playerScripts.Count == 1)
		{
			winner.Add(this.gameObject);
			GetWinner();
		}
		else MoveToNextPlayer();
			
	}
	public void CheckClicked()
	{
		if (callAmount != 0) Debug.Log("Check clicked when callamount not equal 0!");
		else MoveToNextPlayer();
	}
	public void AllInClicked()
	{
		if (callAmount <= playerScripts[currentPlayerIndex].cash)
			callAmount = playerScripts[currentPlayerIndex].cash;
		potValue += callAmount;
		playerScripts[currentPlayerIndex].cash = 0;
		playerScripts[currentPlayerIndex].UpdateUI();
		MoveToNextPlayer();
	}
	public void UpdateUI()
	{
		potText.text = $"Pot: {potValue}";
		callText.text = $"Call: {callAmount}";
		gameStateText.text = gameState.ToString();
		raiseUI.SetActive(false);
		betUI.SetActive(false);
		if (callAmount == 0)
		{
			betButton.gameObject.SetActive(true);
			callButton.gameObject.SetActive(false);
		}
		else
		{
			betButton.gameObject.SetActive(false);
			callButton.gameObject.SetActive(true);
		}
		playerScripts[currentPlayerIndex].UpdateUI();
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
	public void StartClicked()
	{
		callButton.gameObject.SetActive(true);
		raiseButton.gameObject.SetActive(true);
		foldButton.gameObject.SetActive(true);
		startButton.gameObject.SetActive(false);
		StartAllHands();
		CommencePreflop();
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
	private void StartTurn()
	{
		currentPlayerIndex = 0;
		playerScripts[currentPlayerIndex].isActivePlayer = true;
	}
	public void MoveToNextPlayer()
	{
		playerScripts[currentPlayerIndex].HideCards();
		currentPlayerIndex--;
		if (currentPlayerIndex >= 0) playerScripts[currentPlayerIndex].ShowCards();
		else if (currentPlayerIndex == -1)
			GoToNextSession();
		

		//Debug.Log("currentplayerindex: " + currentPlayerIndex);
	}
	private void GoToNextSession()
	{
		switch (gameState)
		{
			case (GameState.Preflop):
				CommenceFlop();
				break;
			case (GameState.Flop):
				CommenceTurn();
				break;
			case (GameState.Turn):
				CommenceRiver();
				break;
			case (GameState.River):
				CompareHandValues();
				GetWinner();
				break;
		}
	}
	private void CommencePreflop()
	{
		gameState = GameState.Preflop;
		gameStateText.text = "Preflop";
		currentPlayerIndex = playerScripts.Count - 1;
		playerScripts[currentPlayerIndex].ShowCards();
	}
	private void CommenceFlop()
	{
		currentPlayerIndex = playerScripts.Count - 1;
		playerScripts[currentPlayerIndex].ShowCards();
		callAmount = 0;
		UpdateUI();
		gameState = GameState.Flop;
		checkButton.gameObject.SetActive(true);
		allInButton.gameObject.SetActive(true);
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
			//Debug.Log("handvalue: " + handList[i].HandValue);
			keyValuePairs.Add(playerScripts[i], handList[i].HandValue);
		}
		hideTurn.gameObject.SetActive(true);
		hideRiver.gameObject.SetActive(true);

	}
	private void CommenceTurn()
	{
		currentPlayerIndex = playerScripts.Count - 1;
		playerScripts[currentPlayerIndex].ShowCards();
		callAmount = 0;
		gameState = GameState.Turn;
		gameStateText.text = "Turn";
		currentPlayerIndex = playerScripts.Count - 1;
		checkButton.gameObject.SetActive(true);
		allInButton.gameObject.SetActive(true);
		hideTurn.gameObject.SetActive(false);
	}
	private void CommenceRiver()
	{
		currentPlayerIndex = playerScripts.Count - 1;
		playerScripts[currentPlayerIndex].ShowCards();
		callAmount = 0;
		gameState = GameState.River;
		gameStateText.text = "River";
		currentPlayerIndex = playerScripts.Count - 1;
		checkButton.gameObject.SetActive(true);
		allInButton.gameObject.SetActive(true);
		hideRiver.gameObject.SetActive(false);
	}
	private void GetWinner()
	{
		RevealAllCards();
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

	private void RevealAllCards()
	{
		for (int i = participants.Count - 1; i > 0; i--)
			participants[i].ShowCards();
	}

	private void RemovePlayerFromList()
	{
		playerScripts.Remove(playerScripts[currentPlayerIndex]);
	}
}



