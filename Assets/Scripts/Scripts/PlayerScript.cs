using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
	public bool isActivePlayer = true;
	public bool stillInGame = true;
	public int cash = 1000;
	[SerializeField] private Text playerText;
	[SerializeField] private GameObject hideCard1;
	[SerializeField] private GameObject hideCard2;

	private void Start()
	{
		playerText.text = this.gameObject.name + ": " + cash;
	}
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
	public void UpdateUI()
	{
		playerText.text = this.gameObject.name + ": " + cash;
	}
	public void ShowCards()
	{
		hideCard1.gameObject.SetActive(false);
		hideCard2.gameObject.SetActive(false);
	}
	public void HideCards()
	{
		hideCard1.gameObject.SetActive(true);
		hideCard2.gameObject.SetActive(true);
	}



}
