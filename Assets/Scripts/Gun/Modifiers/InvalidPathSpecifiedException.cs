using System;


public class InvalidPathSpecifiedException : Exception
    {
        public InvalidPathSpecifiedException(string attributeName) : base($"{attributeName} does not exist at the provided path!") { }
    }
