using UnityEngine;

public class Player
{
    public int PlayerID { get; private set; }
    public string PlayerName { get; set; }

    // Número de jugadores seleccionado en el menú (persistido con PlayerPrefs).
    public static int SelectedPlayersCount
    {
        get => PlayerPrefs.GetInt("SelectedPlayersCount", 1);
        set
        {
            var v = Mathf.Clamp(value, 1, 4);
            PlayerPrefs.SetInt("SelectedPlayersCount", v);
            PlayerPrefs.Save();
        }
    }

    public Player(int playerId, string playerName = "")
    {
        PlayerID = playerId;
        PlayerName = string.IsNullOrEmpty(playerName) ? $"Player {playerId}" : playerName;
    }
}