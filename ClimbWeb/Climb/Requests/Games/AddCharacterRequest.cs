namespace Climb.Requests.Games
{
    public class AddCharacterRequest
    {
        public int GameID { get; set; }
        public string Name { get; set; }

        public AddCharacterRequest()
        {
        }

        public AddCharacterRequest(int gameID, string name)
        {
            GameID = gameID;
            Name = name;
        }
    }
}