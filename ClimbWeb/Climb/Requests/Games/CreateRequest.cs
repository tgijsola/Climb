namespace Climb.Requests.Games
{
    public class CreateRequest
    {
        public string Name { get; set; }
        public int CharactersPerMatch { get; set; }
        public int MaxMatchPoints { get; set; }

        public CreateRequest()
        {
        }

        public CreateRequest(string name, int characterCount, int maxPoints)
        {
            Name = name;
            CharactersPerMatch = characterCount;
            MaxMatchPoints = maxPoints;
        }
    }
}