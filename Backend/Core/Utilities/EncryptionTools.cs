using System.Security.Cryptography;
using System.Text;

namespace Artemis.Backend.Core.Utilities
{
    public class EncryptionTools : IDisposable
    {
        private readonly ILogger<EncryptionTools> _logger;
        private readonly Aes _aes;
        private bool _disposed;

        private const int KeySize = 256;
        private const int BlockSize = 128;
        private const int Iterations = 10000;
        private static readonly byte[] DefaultSalt = "Artemis.Backend.Core"u8.ToArray();

        public EncryptionTools(ILogger<EncryptionTools> logger)
        {
            _logger = logger;
            _aes = InitializeAes();
        }

        #region AES Encryption
        public string EncryptString(string plainText)
        {
            try
            {
                if (string.IsNullOrEmpty(plainText))
                    return string.Empty;

                var ivBytes = GenerateRandomBytes(BlockSize / 8);
                var key = GetEncryptionKey();

                using var encryptor = _aes.CreateEncryptor(key, ivBytes);
                var plainBytes = Encoding.UTF8.GetBytes(plainText);

                using var msEncrypt = new MemoryStream();
                msEncrypt.Write(ivBytes, 0, ivBytes.Length);

                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                var encryptedBytes = msEncrypt.ToArray();
                return Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encryption failed");
                throw new EncryptionException("Failed to encrypt data", ex);
            }
        }

        public string DecryptString(string encryptedText)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedText))
                    return string.Empty;

                if (!IsBase64String(encryptedText))
                    return encryptedText;

                var fullCipher = Convert.FromBase64String(encryptedText);
                var iv = new byte[BlockSize / 8];
                var cipher = new byte[fullCipher.Length - iv.Length];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                var key = GetEncryptionKey();

                using var decryptor = _aes.CreateDecryptor(key, iv);
                using var msDecrypt = new MemoryStream(cipher);
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);

                return srDecrypt.ReadToEnd();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Decryption failed");
                throw new EncryptionException("Failed to decrypt data", ex);
            }
        }
        #endregion

        #region Hash Functions
        public string ComputeHash(string input, HashType hashType = HashType.SHA256)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using var hashAlgorithm = GetHashAlgorithm(hashType);
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = hashAlgorithm.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }

        public bool VerifyHash(string input, string hash, HashType hashType = HashType.SHA256)
        {
            var computedHash = ComputeHash(input, hashType);
            return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        #region Password Hashing
        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            var salt = GenerateRandomBytes(16);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password: Encoding.UTF8.GetBytes(password),
                salt: salt,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32);

            var outputBytes = new byte[48];
            Buffer.BlockCopy(salt, 0, outputBytes, 0, 16);
            Buffer.BlockCopy(hash, 0, outputBytes, 16, 32);

            return Convert.ToBase64String(outputBytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                var inputBytes = Convert.FromBase64String(hashedPassword);
                var salt = new byte[16];
                var hash = new byte[32];

                Buffer.BlockCopy(inputBytes, 0, salt, 0, 16);
                Buffer.BlockCopy(inputBytes, 16, hash, 0, 32);

                var verificationHash = Rfc2898DeriveBytes.Pbkdf2(
                    password: Encoding.UTF8.GetBytes(password),
                    salt: salt,
                    iterations: Iterations,
                    hashAlgorithm: HashAlgorithmName.SHA256,
                    outputLength: 32);

                return CryptographicOperations.FixedTimeEquals(hash, verificationHash);
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Helper Methods
        private static Aes InitializeAes()
        {
            var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }

        private static byte[] GetEncryptionKey()
        {
            // In a real application, get this from secure configuration
            var passphrase = "YourSecurePassphrase";
            return Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(passphrase),
                DefaultSalt,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize / 8);
        }

        private static byte[] GenerateRandomBytes(int length)
        {
            var randomBytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return randomBytes;
        }

        private static bool IsBase64String(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            try
            {
                Span<byte> buffer = new byte[input.Length];
                return Convert.TryFromBase64String(input, buffer, out _);
            }
            catch
            {
                return false;
            }
        }

        private static HashAlgorithm GetHashAlgorithm(HashType hashType) => hashType switch
        {
            HashType.MD5 => MD5.Create(),
            HashType.SHA1 => SHA1.Create(),
            HashType.SHA256 => SHA256.Create(),
            HashType.SHA384 => SHA384.Create(),
            HashType.SHA512 => SHA512.Create(),
            _ => throw new ArgumentException($"Unsupported hash type: {hashType}")
        };

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _aes.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public enum HashType
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    public class EncryptionException : Exception
    {
        public EncryptionException(string message) : base(message) { }
        public EncryptionException(string message, Exception inner) : base(message, inner) { }
    }

    public static class EncryptionExtensions
    {
        public static string ToMD5Hash(this string input)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }

        public static string ToSHA256Hash(this string input)
        {
            using var sha256 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}
