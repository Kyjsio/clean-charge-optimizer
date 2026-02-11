using System;

namespace CleanCharge_Optimizer.Exceptions
{
    public abstract class CleanChargeException : Exception
    {
        protected CleanChargeException(string message) : base(message) { }
        protected CleanChargeException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InvalidChargingParametersException : CleanChargeException
    {
        public InvalidChargingParametersException(string message) : base(message) { }
    }

    public class InsufficientDataException : CleanChargeException
    {
        public InsufficientDataException(string message) : base(message) { }
    }


}