using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Jammit.Model;
using Newtonsoft.Json.Linq;

namespace Jammit.Mobile.Client
{
  class RestClient : IClient
  {
    const string ENDPOINT = "http://localhost:8000/jammit";

    public async Task<List<SongMeta>> LoadCatalog()
    {
      var result = new List<SongMeta>();

      using (var client = new HttpClient())
      {
        client.BaseAddress = new Uri($"{ENDPOINT}/track");
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
            var x = track["id"].ToString();
            result.Add(new SongMeta
            {
              ContentGuid = Guid.Parse(track["id"].ToString()),
              Artist = track["artist"].ToString(),
              Album = track["album"]?.ToString(),
              Name = track["title"].ToString(),
              Instrument = track["instrument"].ToString(),
            });
          }
        }
      }

      return result;
    }

    public int Foo()
    {
      return 999;
    }
  }
}
