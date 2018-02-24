using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Jammit.Net
{
  public class ClassicApiConnector
  {
    private readonly string server;

    private WebClient client;

    public ClassicApiConnector(string server)
    {
      this.server = server;
      client = new WebClient();
    }

    public void Cancel()
    {
      client.CancelAsync();
    }

    public async Task<int> IdCheck(string username, string evje)
    {
      var ret = await client.DownloadStringTaskAsync($"https://store.{server}/scripts/ddidcheck.php?uname={username}&evje={evje}");
      var status = 1;
      int.TryParse(ret, out status);
      return status;
    }

    public async Task<long> GetJammitID(string username, string password)
    {
      var ret = await client.DownloadStringTaskAsync($"https://store.{server}/jammit_app_data?username={username}&password={password}");
      if (string.IsNullOrEmpty(ret))
        return 0;
      string[] data = ret.Split(new[] { "~~" }, StringSplitOptions.None);
      if (data.Length <= 30)
        return -4L;
      return Convert.ToInt64(data[30]) - 32L;
    }

    public async Task MailRegister(long jammitId)
    {
      await client.DownloadStringTaskAsync($"https://store.jammit.com/ddmailregister.php?jammitid={jammitId}");
    }

    public async Task<IReadOnlyList<Guid>> GetGuids(long jammitId, string evje, string build, string osver)
    {
      var ids = await client.DownloadStringTaskAsync($"https://{server}/scripts/ddtv1.php?vje={jammitId}&evje={evje}&bld={build}&typ={osver}");
      if (string.IsNullOrEmpty(ids) || ids.Contains("noid") || ids.Contains("*3max") || !ids.Contains("^^"))
      {
        return new List<Guid>();
      }
      return ids
        .Substring(0, ids.IndexOf("%"))
        .Split(new[] { "^^" }, StringSplitOptions.None)
        .Select(Guid.Parse).ToList();
    }

    public async Task DownloadContent(Guid id, string destPath, DownloadProgressChangedEventHandler handler)
    {
      client.DownloadProgressChanged += handler;
      await client.DownloadFileTaskAsync($"https://store.{server}/sites/default/files/{id.ToString("D").ToUpperInvariant()}.zip", destPath);
      client.DownloadProgressChanged -= handler;
    }
  }
}
