using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Jammit.Model;
using Newtonsoft.Json.Linq;
using Plugin.DeviceInfo;

using Xamarin.Forms;

namespace Jammit.Mobile.Client
{
  class RestClient : BindableObject, IClient
  {
    public static readonly BindableProperty AuthStatusProperty =
      BindableProperty.Create("AuthStatus", typeof(AuthorizationStatus), typeof(AuthorizationStatus), AuthorizationStatus.Unknown, BindingMode.OneWay);

    #region IClient methods

    public async Task<List<SongInfo>> LoadCatalog()
    {
      var result = new List<SongInfo>();

      using (var cliente = new HttpClient())
      {
        cliente.BaseAddress = new Uri($"{Settings.ServiceUri}/track");
        cliente.DefaultRequestHeaders.Clear();
        cliente.DefaultRequestHeaders.Add("Accept", "application/json");

        var response = await cliente.GetAsync(cliente.BaseAddress.AbsoluteUri);
        if (response.IsSuccessStatusCode)
        {
          var jsonString = await response.Content.ReadAsStringAsync();
          var jsonObject = JObject.Parse(jsonString);

          var tracks = jsonObject["_embedded"]["track"] as JArray;
          foreach (var track in tracks)
          {
            result.Add(new SongInfo(
              Guid.Parse(track["id"].ToString()),
              track["artist"].ToString(),
              track["album"].ToString(),
              track["title"].ToString(),
              track["instrument"].ToString(),
              track["genre"].ToString()
            ));
          }
        } // if Succeeded response
      }

      return result;
    }

    public async Task<Stream> DownloadSong(Guid id)
    {
      using (var cliente = new HttpClient())
      {
        cliente.BaseAddress = new Uri($"{Settings.ServiceUri}/download?id={id.ToString().ToUpper()}");
        cliente.DefaultRequestHeaders.Clear();

        return await cliente.GetStreamAsync(cliente.BaseAddress.AbsoluteUri);
      }
    }

    public async Task RequestAuthorization()
    {
      using (var cliente = new HttpClient())
      {
        cliente.BaseAddress = new Uri($"{Settings.ServiceUri}/register-device");
        cliente.DefaultRequestHeaders.Clear();
        cliente.DefaultRequestHeaders.Add("Accept", "application/json");

        var json = new JObject();
        json.Add("id", CrossDeviceInfo.Current.Id);
        json.Add("platform", CrossDeviceInfo.Current.Platform.ToString());
        var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        var responseTask = cliente.PostAsync(cliente.BaseAddress.AbsoluteUri, content);
        var response = responseTask.Result;
        var responseContent = response.Content;
        var responseContentStringTask = responseContent.ReadAsStringAsync();
        var responseContentString = responseContentStringTask.Result;
        var responseJson = JObject.Parse(responseContentString);
        var authorization = responseJson["authorization"].ToString();

        switch (authorization)
        {
          case "0":
            AuthStatus = AuthorizationStatus.Unknown; break;
          case "1":
            AuthStatus = AuthorizationStatus.Requested; break;
          case "2":
            AuthStatus = AuthorizationStatus.Rejected; break;
          case "3":
            AuthStatus = AuthorizationStatus.Approved; break;
          default:
            throw new Exception($"Unknown autorization status code [{authorization}].");
        }
      } // Using HttpClient
    }

    #endregion

    #region IClient properties

    public AuthorizationStatus AuthStatus
    {
      get
      {
        return (AuthorizationStatus)GetValue(AuthStatusProperty);
      }

      private set
      {
        SetValue(AuthStatusProperty, value);
      }
    }

    #endregion
  }
}
