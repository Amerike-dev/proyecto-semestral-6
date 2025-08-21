
// [Serializable]
public class Player
{
    public int PlayerID { get; private set; }
    public string PlayerName { get; set; }

    public Player(int playerId, string playerName = "")
    {
        //Esto Identifica al jugador
        PlayerID = playerId;
        PlayerName = string.IsNullOrEmpty(playerName) ? $"Player {playerId}" : playerName;
    }
}