using System;
using System.Collections.Generic;
using System.Text;

namespace GGJ_2021
{
	public enum SoundType
	{
		Background
	}

	public enum GameState
	{
		MainMenu,
		Setup,
		StartLevel,
		CityLevel,
		Battle,
		EndScreen,
		Settings,
		//Settings assumes to draw start level
		//But we might want to be in the city and do settings
		CityLevelSettings
	}

	public enum BattleState
	{
		Draw,
		Select, //select out of the cards
		Compare, //Show AI selection and the +- points
		Result //If battle ends, show who won
	}

	public enum FaceDirection
	{
		Up,
		Down,
		Left,
		Right
	}

	public enum CardType
	{
		Rock,
		Paper,
		Scissor
	}
}
