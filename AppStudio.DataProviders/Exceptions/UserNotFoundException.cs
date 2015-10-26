using System;

namespace AppStudio.DataProviders.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string screenName)
            : base("User " + screenName + " not found.")
        {
        }
    }
}
