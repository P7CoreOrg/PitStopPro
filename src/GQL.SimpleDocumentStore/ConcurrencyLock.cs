namespace SimpleDocumentStore
{
    internal static class ConcurrencyLock
    {
        private static object _theLock;
        public static object TheLock
        {
            get { return _theLock ?? (_theLock = new object()); }
        }
    }
}
