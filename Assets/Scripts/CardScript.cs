using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
	[SerializeField] private int value = 0;
	[SerializeField] private int suit = 0;
	[SerializeField] private Sprite sprite;
	[SerializeField] private Sprite cardback;
	public int GetValue()
	{
		return value;
	}
	public void SetValue(int newValue)
	{
		value = newValue;
	}
	public int GetSuit()
	{
		return suit;
	}
	public void SetSuit(int newSuit)
	{
		suit = newSuit;
	}
	public Sprite GetSprite()
	{
		return sprite;
	}
	public void SetSprite(Sprite newSprite)
	{
		sprite = newSprite;
		GetComponent<SpriteRenderer>().sprite = newSprite;
	}
	public string Formatting()
	{
		string valueString, cardString;
		string suitString = "";
		switch (value)
		{
			case 1:
				valueString = "a";
				break;
			case 11:
				valueString = "j";
				break;
			case 12:
				valueString = "q";
				break;
			case 13:
				valueString = "k";
				break;
			default:
				valueString = value.ToString();
				break;
		}
		switch (suit)
		{
			case 0:
				suitString = "c";
				break;
			case 1:
				suitString = "d";
				break;
			case 2:
				suitString = "h";
				break;
			case 3:
				suitString = "s";
				break;
		}
		cardString = valueString + suitString;
		//Debug.Log(cardString);
		return cardString;
	}
	public void ResetCards()
	{
		SetValue(0);
		SetSuit(-1);
		SetSprite(cardback);
	}
}
