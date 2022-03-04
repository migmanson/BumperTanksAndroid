using UnityEngine;

static class Grid
{
    public static UserManager playerStats;
    public static SoundController sfx;
    public static GameManager game;

    static Grid()
    {
        GameObject g = GameObject.Find("_app");

        playerStats = g.GetComponent<UserManager>();
        sfx = g.GetComponent<SoundController>();
        game = g.GetComponent<GameManager>();
    }
}
