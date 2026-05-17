using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using authentication_engine.Features.Auth.Interfaces;

namespace authentication_engine.Features.Auth.Services;

public class EncryptionService(IConfiguration configuration): IEncryptionService
{
    private readonly IConfiguration _configuration = configuration;
    
    private const int KeySizeInBytes = 32;  // 256 bits = 32 bytes
    private const int IvSizeInBytes = 16;  // 128 bits = 16 bytes

    public string Encrypt(object jsonObject)
    {
        // Serialize JSON object to string
        string plainText = JsonSerializer.Serialize(jsonObject);
        
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentNullException("Plain text cannot be null or empty");

        // Use shared key
        byte[] keyBytes = GetSharedKey();
        
        // Generate random IV
        var (iv, ivBytes) = GenerateRandomIvWithBytes();

        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        using (Aes aes = Aes.Create())
        {
            //AES Configuration
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyBytes;
            aes.IV = ivBytes;

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();
                
                byte[] cipherBytes = memoryStream.ToArray();
                string encryptedMessage = Convert.ToBase64String(cipherBytes);
                
                // Separate IV and message with colon
                return $"{iv}:{encryptedMessage}";
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
            throw new ArgumentNullException("Cipher text cannot be null or empty");

        // Split IV and message
        string[] parts = cipherText.Split(':');
        if (parts.Length != 2)
            throw new ArgumentNullException("Invalid encrypted data format. Expected format: 'IV:EncryptedMessage'");
        
        string iv = parts[0];
        string encryptedMessage = parts[1];
        
        // Use shared key
        byte[] keyBytes = GetSharedKey();
        byte[] ivBytes = Convert.FromBase64String(iv);
        byte[] cipherBytes = Convert.FromBase64String(encryptedMessage);

        using (Aes aes = Aes.Create())
        {
            //AES Configuration
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyBytes;
            aes.IV = ivBytes;

            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (StreamReader streamReader = new StreamReader(cryptoStream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
    
    //INFO:: Shared Key should be base64 string with 32bytes
    //INFO:: Use https://generate-random.org/base64-string
    private byte[] GetSharedKey()
    {
        var sharedKeyBase64 = _configuration["Encryption:SharedKey"];
        
        if (string.IsNullOrEmpty(sharedKeyBase64))
            throw new ArgumentNullException("Encryption key not configured");
    
        byte[] keyBytes = Convert.FromBase64String(sharedKeyBase64);
        
        if (keyBytes.Length != KeySizeInBytes)
            throw new BadHttpRequestException($"Invalid encryption key size.");
        
        return keyBytes;
    }
    
    private (string Iv, byte[] IvBytes) GenerateRandomIvWithBytes()
    {
        byte[] ivBytes = new byte[IvSizeInBytes];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(ivBytes);
        }
        return (Convert.ToBase64String(ivBytes), ivBytes);
    }
    
}