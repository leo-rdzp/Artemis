using Renci.SshNet;
using System.Security.Authentication;

namespace Artemis.Backend.Core.Utilities
{
    public class FtpTools
    {
        private readonly ILogger<FtpTools> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public FtpTools(
            ILogger<FtpTools> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResultNotifier> UploadAsync(FtpConfig config, string fileName, byte[]? fileContent = null)
        {
            try
            {
                ValidateConfig(config);

                return config.Type?.ToUpperInvariant() switch
                {
                    "SFTP" => await UploadSftpAsync(config, fileName, fileContent),
                    "FTPS" or "FTP" => await UploadFtpAsync(config, fileName, fileContent),
                    _ => ResultNotifier.Failure("Invalid FTP type specified")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during file upload");
                return ResultNotifier.Failure(ex.Message, ex.ToString());
            }
        }

        public async Task<ResultNotifier> DownloadAsync(FtpConfig config, string remoteFileName, string localPath)
        {
            try
            {
                ValidateConfig(config);

                return config.Type?.ToUpperInvariant() switch
                {
                    "SFTP" => await DownloadSftpAsync(config, remoteFileName, localPath),
                    "FTPS" or "FTP" => await DownloadFtpAsync(config, remoteFileName, localPath),
                    _ => ResultNotifier.Failure("Invalid FTP type specified")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during file download");
                return ResultNotifier.Failure(ex.Message, ex.ToString());
            }
        }

        public async Task<ResultNotifier> ListDirectoryAsync(FtpConfig config, string path = "")
        {
            try
            {
                ValidateConfig(config);

                return config.Type?.ToUpperInvariant() switch
                {
                    "SFTP" => ListSftpDirectory(config, path),
                    "FTPS" or "FTP" => await ListFtpDirectoryAsync(config, path),
                    _ => ResultNotifier.Failure("Invalid FTP type specified")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing directory");
                return ResultNotifier.Failure(ex.Message, ex.ToString());
            }
        }

        #region FTP/FTPS Operations
        private async Task<ResultNotifier> UploadFtpAsync(FtpConfig config, string fileName, byte[]? fileContent)
        {
            try
            {
                var destinationUrl = BuildUrl(config, fileName);
                using var client = CreateFtpClient(config);
                using var content = CreateFileContent(fileName, fileContent);

                var request = new HttpRequestMessage(HttpMethod.Put, destinationUrl);
                request.Content = content;

                var response = await client.SendAsync(request);
                return await HandleHttpResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FTP upload failed");
                return ResultNotifier.Failure(ex.Message);
            }
        }

        private async Task<ResultNotifier> DownloadFtpAsync(FtpConfig config, string remoteFileName, string localPath)
        {
            try
            {
                var sourceUrl = BuildUrl(config, remoteFileName);
                using var client = CreateFtpClient(config);

                var response = await client.GetAsync(sourceUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return ResultNotifier.Failure($"Download failed: {response.StatusCode}");
                }

                await using var contentStream = await response.Content.ReadAsStreamAsync();
                await using var fileStream = File.Create(localPath);
                await contentStream.CopyToAsync(fileStream);

                return ResultNotifier.Success(localPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FTP download failed");
                return ResultNotifier.Failure(ex.Message);
            }
        }

        private async Task<ResultNotifier> ListFtpDirectoryAsync(FtpConfig config, string path)
        {
            try
            {
                var listUrl = BuildUrl(config, path);
                using var client = CreateFtpClient(config);

                var request = new HttpRequestMessage(HttpMethod.Get, listUrl);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return ResultNotifier.Failure($"Directory listing failed: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var entries = ParseFtpDirectoryListing(content);
                return ResultNotifier.Success(entries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FTP directory listing failed");
                return ResultNotifier.Failure(ex.Message);
            }
        }
        #endregion

        #region SFTP Operations
        private async Task<ResultNotifier> UploadSftpAsync(FtpConfig config, string fileName, byte[]? fileContent)
        {
            try
            {
                using var client = CreateSftpClient(config);
                client.Connect();

                var remotePath = BuildRemotePath(config.Path, fileName);
                EnsureRemoteDirectoryExists(client, Path.GetDirectoryName(remotePath));

                await using var contentStream = CreateContentStream(fileName, fileContent);
                client.UploadFile(contentStream, remotePath, true);

                return ResultNotifier.Success(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SFTP upload failed");
                return ResultNotifier.Failure(ex.Message);
            }
        }

        private async Task<ResultNotifier> DownloadSftpAsync(FtpConfig config, string remoteFileName, string localPath)
        {
            try
            {
                using var client = CreateSftpClient(config);
                client.Connect();

                var remotePath = BuildRemotePath(config.Path, remoteFileName);

                if (!client.Exists(remotePath))
                {
                    return ResultNotifier.Failure("Remote file not found");
                }

                await using var fileStream = File.Create(localPath);
                client.DownloadFile(remotePath, fileStream);

                return ResultNotifier.Success(localPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SFTP download failed");
                return ResultNotifier.Failure(ex.Message);
            }
        }

        private ResultNotifier ListSftpDirectory(FtpConfig config, string path)
        {
            try
            {
                using var client = CreateSftpClient(config);
                client.Connect();

                var remotePath = BuildRemotePath(config.Path, path);
                var entries = client.ListDirectory(remotePath)
                    .Select(entry => new FtpEntry
                    {
                        Name = entry.Name,
                        FullPath = entry.FullName,
                        IsDirectory = entry.IsDirectory,
                        Size = entry.Length,
                        LastModified = entry.LastWriteTime
                    })
                    .ToList();

                return ResultNotifier.Success(entries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SFTP directory listing failed");
                return ResultNotifier.Failure(ex.Message);
            }
        }
        #endregion

        #region Helper Methods
        private static void ValidateConfig(FtpConfig config)
        {
            if (string.IsNullOrEmpty(config.Server))
                throw new ArgumentException("Server is required", nameof(config));

            if (string.IsNullOrEmpty(config.Username))
                throw new ArgumentException("Username is required", nameof(config));

            if (string.IsNullOrEmpty(config.Password))
                throw new ArgumentException("Password is required", nameof(config));
        }

        private HttpClient CreateFtpClient(FtpConfig config)
        {
            var handler = new HttpClientHandler
            {
                Credentials = new System.Net.NetworkCredential(config.Username, config.Password)
            };

            if (config.Type?.ToUpperInvariant() == "FTPS")
            {
                handler.SslProtocols = SslProtocols.Tls12;
            }

            return _httpClientFactory.CreateClient();
        }

        private static SftpClient CreateSftpClient(FtpConfig config)
        {
            var port = !string.IsNullOrEmpty(config.Port) && int.TryParse(config.Port, out var parsedPort)
                ? parsedPort
                : 22;

            return new SftpClient(config.Server, port, config.Username, config.Password);
        }

        private static string BuildUrl(FtpConfig config, string path)
        {
            var scheme = config.Type?.ToUpperInvariant() == "FTPS" ? "ftps" : "ftp";
            var baseUrl = $"{scheme}://{config.Server}";

            if (!string.IsNullOrEmpty(config.Port))
                baseUrl += $":{config.Port}";

            if (!string.IsNullOrEmpty(config.Path))
                baseUrl += config.Path.StartsWith("/") ? config.Path : $"/{config.Path}";

            if (!string.IsNullOrEmpty(path))
                baseUrl += path.StartsWith("/") ? path : $"/{path}";

            return baseUrl;
        }

        private static string BuildRemotePath(string? basePath, string fileName)
        {
            if (string.IsNullOrEmpty(basePath))
                return fileName;

            return Path.Combine(basePath.Replace('\\', '/'), fileName).Replace('\\', '/');
        }

        private static void EnsureRemoteDirectoryExists(SftpClient client, string? path)
        {
            if (string.IsNullOrEmpty(path)) return;

            var normalizedPath = path.Replace('\\', '/');
            var parts = normalizedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var currentPath = string.Empty;

            foreach (var part in parts)
            {
                currentPath += $"/{part}";
                if (!client.Exists(currentPath))
                {
                    client.CreateDirectory(currentPath);
                }
            }
        }

        private static HttpContent CreateFileContent(string fileName, byte[]? fileContent)
        {
            if (fileContent != null)
                return new ByteArrayContent(fileContent);

            if (!File.Exists(fileName))
                throw new FileNotFoundException("File not found", fileName);

            return new StreamContent(File.OpenRead(fileName));
        }

        private static Stream CreateContentStream(string fileName, byte[]? fileContent)
        {
            if (fileContent != null)
                return new MemoryStream(fileContent);

            if (!File.Exists(fileName))
                throw new FileNotFoundException("File not found", fileName);

            return File.OpenRead(fileName);
        }

        private static async Task<ResultNotifier> HandleHttpResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return ResultNotifier.Success(null);

            var errorContent = await response.Content.ReadAsStringAsync();
            return ResultNotifier.Failure(
                $"Operation failed with status {response.StatusCode}",
                errorContent);
        }

        private static List<FtpEntry> ParseFtpDirectoryListing(string listing)
        {
            var entries = new List<FtpEntry>();
            var lines = listing.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 4) continue;

                entries.Add(new FtpEntry
                {
                    Name = parts[^1],
                    FullPath = parts[^1],
                    IsDirectory = parts[0].StartsWith("d"),
                    Size = long.TryParse(parts[^5], out var size) ? size : 0,
                    LastModified = DateTime.Now // Would need more complex parsing for actual date
                });
            }

            return entries;
        }
        #endregion
    }

    public class FtpConfig
    {
        public string Type { get; set; } = string.Empty;
        public string Server { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public static FtpConfig FromItemList(ItemList list)
        {
            return new FtpConfig
            {
                Type = list.GetString("ftpType", "FTP"),
                Server = list.GetString("ftpServer", ""),
                Port = list.GetString("ftpPort", ""),
                Path = list.GetString("ftpPath", ""),
                Username = list.GetString("ftpUser", ""),
                Password = list.GetString("ftpPassword", "")
            };
        }
    }

    public class FtpEntry
    {
        public string Name { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public bool IsDirectory { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
    }

    public static class FtpToolsExtensions
    {
        public static async Task<bool> TestConnectionAsync(
            this FtpTools tools,
            FtpConfig config,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Create a small test file
                var testContent = "Connection Test"u8.ToArray();
                var testFileName = $"test_{DateTime.Now:yyyyMMddHHmmss}.txt";

                var result = await tools.UploadAsync(config, testFileName, testContent);
                return result.ResultStatus.IsPassed;
            }
            catch
            {
                return false;
            }
        }
    }
}