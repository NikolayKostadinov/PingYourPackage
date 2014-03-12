namespace PingYourPackage.Domain.Services
{
    using System;
    using System.Linq;

    public class OperationResult
    {
        public OperationResult(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; private set; }
    }
}
