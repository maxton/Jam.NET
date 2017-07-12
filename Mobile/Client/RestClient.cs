using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Jammit.Model;
using Jammit.Util;
using Newtonsoft.Json.Linq;

namespace Jammit.Mobile.Client
{
  class RestClient : IClient
  {
    public async Task<List<SongInfo>> LoadCatalog()
    {
      var result = new List<SongInfo>();

      using (var client = new HttpClient())
      {
        client.BaseAddress = new Uri($"{Settings.ServiceUri}/track");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        var response = await client.GetAsync(client.BaseAddress.AbsoluteUri);
        if (response.IsSuccessStatusCode)
        {
          var jsonString = await response.Content.ReadAsStringAsync();
          var jsonObject = JObject.Parse(jsonString);

          var tracks = jsonObject["_embedded"]["track"] as JArray;
          foreach(var track in tracks)
          {
            result.Add(new SongInfo(
              Guid.Parse(track["id"].ToString()),
              track["artist"].ToString(),
              track["album"]?.ToString(), //TODO: Make mandatory
              track["title"].ToString(),
              track["instrument"].ToString(),
              track["genre"]?.ToString()  //TODO: Make mandatory
            ));
          }
        } // if Succeeded response
      }

      return result;
    }

    public int Foo()
    {
      return 999;
    }

    public async Task<Stream> DownloadSong(Guid id)
    {
      using (var client = new HttpClient())
      {
        client.BaseAddress = new Uri($"{Settings.ServiceUri}/download?id={id}");
        client.DefaultRequestHeaders.Accept.Clear();
        //client.DefaultRequestHeaders.Add("Accept", "application/json");
        //client.DefaultRequestHeaders.Add("Content-Type", "application/octet-stream");

        return await client.GetStreamAsync(client.BaseAddress.AbsoluteUri);
      }
    }
  }
}
