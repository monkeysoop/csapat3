namespace Mekkdonalds.Exception
{
    [Serializable]
    internal class PackagesDataException : System.Exception
    {
        public PackagesDataException()
        {
        }

        public PackagesDataException(string? message) : base(message)
        {
        }

        public PackagesDataException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }
    }
}