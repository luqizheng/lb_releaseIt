namespace ReleaseIt.Log
{
    public class EmptyLogger : ILog
    {
        public void Info(string str)
        {
        }

        public void Error(string str)
        {
        }
    }
}