namespace PongClient.Model
{
    public class Player
    {
        public string? UserName { get; set; }
        public string? PlayerPosition { get; set; }
        public string? ConnectionId { get; set; }


        public Player()
        {

        }

        public Player(string connectionId, string userName, string possition)
        {
            UserName = userName;
            PlayerPosition = possition;
            ConnectionId = connectionId;
        }
    }
}

