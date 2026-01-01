public class DataEncryptor : IEncryptor
{
    private const byte Key = 0x5A;

    public string Encrypt(string data)
    {
        if (string.IsNullOrEmpty(data))
            return data;

        char[] array = data.ToCharArray();

        for (int i = 0; i < array.Length; i++)
            array[i] = (char)(array[i] ^ Key);

        return new string(array);
    }

    public string Decrypt(string data) =>
        Encrypt(data);
}