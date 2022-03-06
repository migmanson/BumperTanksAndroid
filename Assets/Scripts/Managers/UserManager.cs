using UnityEngine;

public class UserManager : MonoBehaviour
{
	// gameplay specific data
	// we keep these private and provide methods to modify them
	// instead, just to prevent any accidental corruption
	// or invalid data coming in
	private int score;
	private int highScore;
	private int health;
	private int lives;
	private bool isFinished;
	// this is the display name of the player
	public string playerName = "Anon";
	public virtual void GetDefaultPlayerData()
	{
		playerName = "Anon";
		score = 0;
		health = 3;
		lives = 4;
		highScore = 0;
		isFinished = false;
	}
	public string GetName()
	{
		return playerName;
	}
	public void SetName(string aName)
	{
		playerName = aName;
	}
	public int GetHighScore()
	{
		return highScore;
	}

	public int GetScore()
	{
		return score;
	}
	public virtual void AddScore(int anAmount)
	{
		score += anAmount;
	}

	public void LostScore(int num)
	{
		score -= num;
	}

	public void SetScore(int num)
	{
		score = num;
	}
	public int GetHealth()
	{
		return health;
	}
	public void AddHealth(int num)
	{
		health += num;
	}
	public void ReduceHealth(int num)
	{
		health -= num;
	}
	public void SetHealth(int num)
	{
		health = num;
	}

	public int GetLives()
	{
		return lives;
	}
	public void AddLives(int num)
	{
		lives += num;
	}
	public void ReduceLives(int num)
	{
		lives -= num;
	}
	public void SetLives(int num)
	{
		lives = num;
	}


	public bool GetIsFinished()
	{
		return isFinished;
	}

	public void SetIsFinished(bool aVal)
	{
		isFinished = aVal;
	}
}
