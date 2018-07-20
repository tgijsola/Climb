namespace Climb.Core.TieBreakers.Internal
{
    internal class TieBreakerFactory : ITieBreakerFactory
    {
        public ITieBreaker Create()
        {
            return new TieBreaker();
        }
    }
}