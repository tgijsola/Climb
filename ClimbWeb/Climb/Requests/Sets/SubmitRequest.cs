namespace Climb.Requests.Sets
{
    public class SubmitRequest
    {
        public int SetID { get; set; }
        public MatchForm[] Matches { get; set; }
    }
}