namespace Jeomseon.Samples.Singleton
{
    public sealed class SampleGameService : System.IDisposable
    {
        public bool IsReady { get; private set; }

        public void Initialize()
        {
            IsReady = true;
        }

        public void Dispose()
        {
            IsReady = false;
        }
    }
}
