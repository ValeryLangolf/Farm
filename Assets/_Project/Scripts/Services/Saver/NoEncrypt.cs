public class NoEncrypt : IEncryptor
{
    public string Decrypt(string data) =>
        data;

    public string Encrypt(string data) =>
        data;
}