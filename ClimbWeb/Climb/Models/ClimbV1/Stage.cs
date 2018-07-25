namespace ClimbV1.Models
{
    public class Stage
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int GameID { get; set; }
        public Game Game { get; set; }
    }
}
