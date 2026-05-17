namespace authentication_engine.Features.Auth.Interfaces;

public interface IEncryptionService
{
    string Encrypt(object jsonObject);
    
    string Decrypt(string cipherText);
}