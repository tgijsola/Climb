namespace Climb.Core.TieBreakers.New
{
    public interface IParticipant
    {
        int ID { get; }
        int Points { get; }
        int TieBreakerPoints { get; set; }
    }
}