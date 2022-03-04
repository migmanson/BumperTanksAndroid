using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game specific data
    // we keep these private and provide methods to modify them
    // instead, just to prevent any accidental corruption
    // or invalid data coming in
    private int score;
    private int highScore;
    private int level;
    private int health;
    private int lives;    
    private bool paused;
    private bool mainMenu;
    private bool nivelComenzado;
    private bool nivelTerminado;
    private bool partidaComenzada;
    private bool partidaTerminada;
    // this is the display name of the player
    public string playerName = "Anon";
    public virtual void GetDefaultData()
    {
        playerName = "Anon";
        level = 1;
        highScore = 0;
    }
    public string GetName()
    {
        return playerName;
    }
    public void SetName(string aName)
    {
        playerName = aName;
    }
    public int GetLevel()
    {
        return level;
    }
    public void SetLevel(int num)
    {
        level = num;
    }
    public void AddLevel(int num)
    {
        level++;
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


    public bool GetNivelComenzado()
    {
        return nivelComenzado;
    }

    public void SetNivelComenzado(bool aVal)
    {
        nivelComenzado = aVal;
    }
    public bool GetNivelTerminado()
    {
        return nivelTerminado;
    }

    public void SetNivelTerminado(bool aVal)
    {
        nivelTerminado = aVal;
    }

    public bool GetPartidaComenzada()
    {
        return partidaComenzada;
    }

    public void SetPartidaComenzada(bool aVal)
    {
        partidaComenzada = aVal;
    }
    public bool GetPartidaTerminada()
    {
        return partidaTerminada;
    }

    public void SetPartidaTerminada(bool aVal)
    {
        partidaTerminada = aVal;
    }

}
