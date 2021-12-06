using System;

namespace event_client_app.Services
{
    public class HashAndVerify : IDisposable
    {
        public string HashCode(string code)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedCode = BCrypt.Net.BCrypt.HashPassword(code, salt);
            return hashedCode;
        }

        public bool VerifyHashedCode(string code, string hashedCode)
        {
            bool verified = BCrypt.Net.BCrypt.Verify(code, hashedCode);
            return verified;
        }

        public void Dispose()
        {
            // 需要写点啥？？？？？？？？？？？？？？
        }
    }
}