using System;


namespace SalesWebMvc.Services.Exceptions
{
    public class IntegrityEcpetion : ApplicationException
    {
        public IntegrityEcpetion(string message) : base(message)
        {

        }
    }
}
