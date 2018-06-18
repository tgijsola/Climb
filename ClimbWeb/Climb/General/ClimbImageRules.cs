namespace Climb
{
    public static class ClimbImageRules
    {
        public static ImageRules ProfilePic { get; } = new ImageRules(100 * 1024, 150, 150, "profile-pics", string.Empty);
    }
}