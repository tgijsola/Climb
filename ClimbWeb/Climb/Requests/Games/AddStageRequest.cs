namespace Climb.Requests.Games
{
    public class AddStageRequest
    {
        public int GameID { get; set; }
        public string Name { get; set; }

        public AddStageRequest()
        {
        }

        public AddStageRequest(int gameID, string name)
        {
            GameID = gameID;
            Name = name;
        }
    }
}