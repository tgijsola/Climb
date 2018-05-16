namespace Climb.Requests.Sets
{
    public class MatchForm
    {
        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
        public int[] Player1Characters { get; set; }
        public int[] Player2Characters { get; set; }
        public int? StageID { get; set; }
    }
}