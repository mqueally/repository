using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace AmqCompanySecretarialManager;

public sealed class BackupService
{
    public void CreateEncryptedBackup(string databasePath, string backupPath, string password)
    {
        if (!File.Exists(databasePath))
        {
            throw new FileNotFoundException("Database not found.", databasePath);
        }

        using var tempStream = new MemoryStream();
        using (var archive = new ZipArchive(tempStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            archive.CreateEntryFromFile(databasePath, Path.GetFileName(databasePath), CompressionLevel.Optimal);
        }

        tempStream.Position = 0;
        var encrypted = Encrypt(tempStream.ToArray(), password);
        File.WriteAllBytes(backupPath, encrypted);
    }

    public void RestoreEncryptedBackup(string backupPath, string outputDatabasePath, string password)
    {
        if (!File.Exists(backupPath))
        {
            throw new FileNotFoundException("Backup not found.", backupPath);
        }

        var encrypted = File.ReadAllBytes(backupPath);
        var zipBytes = Decrypt(encrypted, password);

        using var zipStream = new MemoryStream(zipBytes);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var entry = archive.Entries.Count > 0 ? archive.Entries[0] : null;

        if (entry is null)
        {
            throw new InvalidOperationException("Backup archive is empty.");
        }

        entry.ExtractToFile(outputDatabasePath, overwrite: true);
    }

    private static byte[] Encrypt(byte[] data, string password)
    {
        using var aes = Aes.Create();
        aes.Key = DeriveKey(password, aes.KeySize / 8);
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        using var output = new MemoryStream();
        output.Write(aes.IV, 0, aes.IV.Length);

        using (var cryptoStream = new CryptoStream(output, encryptor, CryptoStreamMode.Write))
        {
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
        }

        return output.ToArray();
    }

    private static byte[] Decrypt(byte[] data, string password)
    {
        using var aes = Aes.Create();
        var ivLength = aes.BlockSize / 8;
        var iv = new byte[ivLength];
        Array.Copy(data, 0, iv, 0, ivLength);

        aes.Key = DeriveKey(password, aes.KeySize / 8);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        using var input = new MemoryStream(data, ivLength, data.Length - ivLength);
        using var cryptoStream = new CryptoStream(input, decryptor, CryptoStreamMode.Read);
        using var output = new MemoryStream();
        cryptoStream.CopyTo(output);
        return output.ToArray();
    }

    private static byte[] DeriveKey(string password, int keyBytes)
    {
        using var deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("AMQ-Secretarial-Backup"), 10000, HashAlgorithmName.SHA256);
        return deriveBytes.GetBytes(keyBytes);
    }
}
