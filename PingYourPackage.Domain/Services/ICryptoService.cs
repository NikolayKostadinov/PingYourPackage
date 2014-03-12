namespace PingYourPackage.Domain.Services
{
    using System;
    using System.Linq;

    public interface ICryptoService
    {
        string GenerateSalt();
        string EncryptPassword(string password, string salt);
    }
}
