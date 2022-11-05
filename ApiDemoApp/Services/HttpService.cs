using ApiDemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Net.WebRequestMethods;

namespace ApiDemoApp.Services
{
    public class HttpService
    {
		public static string Base_URL { get; set; }
        public static async Task<dynamic> Execute_Get(string cmdname)
        {
			try
			{
				using(var client = new HttpClient())
				{
                    string call_url = Base_URL + "/reeman/" + cmdname;
					switch (cmdname)
					{
						case "pose":
							return await client.GetFromJsonAsync<Coordinace>(call_url);
						case "speed":
                            return await client.GetFromJsonAsync<AGVSpeed>(call_url);
                        case "base_encode":
                            return await client.GetFromJsonAsync<Battery>(call_url);
                        case "movebase_status":
                            return await client.GetFromJsonAsync<MoveStatus>(call_url);
                        case "hostname":
                            return await client.GetFromJsonAsync<HostName>(call_url);
                        case "current_version":
                            return await client.GetFromJsonAsync<VersionName>(call_url);
                        case "get_mode":
                            return await client.GetFromJsonAsync<ModeName>(call_url);
                        case "android_target":
							{
                                List<TargetPointsModel> lst = new List<TargetPointsModel>();
                                Dictionary<string, List<string>> temp_lst = await client.GetFromJsonAsync<Dictionary<string, List<string>>>(call_url);
                                foreach (var key in temp_lst.Keys)
                                {
                                    TargetPointsModel targetPointsModel = new TargetPointsModel()
                                    {
                                        name = key,
                                        coordinace = new Coordinace()
                                        {
                                            x = double.Parse(temp_lst[key][0]),
                                            y = double.Parse(temp_lst[key][1]),
                                            theta = double.Parse(temp_lst[key][2])
                                        }
                                    };
                                    lst.Add(targetPointsModel);
                                }
                                return lst;
                            }
                        case "map":
                            {
                                return await client.GetFromJsonAsync<MapLayer>(call_url);
                            }
                        case "current_map":
                            {
                                var q = await client.GetFromJsonAsync<JsonObject>(call_url);
                                return q["name"].ToString();

                            }
                        case "history_map":
                            {
                                var lst = await client.GetFromJsonAsync<JsonObject>(call_url);
                                var result = lst["maps"];
                                return JsonSerializer.Deserialize<List<MapModel>>(result);
                            }
                         
                        default:
                            return "";
                    }
                }
			}
			catch (Exception ex)
			{

				LogService.LogMessage("Execute_Get:" + cmdname + ex.Message);
				return ex.Message;
			}
        }

		public static async Task Execute_Post(string cmdname,string payload)
		{
            try
            {
                using (var client = new HttpClient())
                {
                    string call_url = Base_URL + "/cmd/" + cmdname;
                   var httpContent = payload == null ? null : new StringContent(payload, Encoding.UTF8, "application/json");
                    var httpResponse = await client.PostAsync(call_url, httpContent);
                }

            }
            catch (Exception ex)
            {
                LogService.LogMessage("Execute_Post:"+ cmdname + ex.Message);
            }
		}


        public static async Task<string> UploadMap(MultipartFormDataContent content)
        {
            try
            {
                string call_url = Base_URL + "/upload/import_map";
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                    var postResult = await client.PostAsync(call_url, content);
                    var postContent = await postResult.Content.ReadAsStringAsync();

                    if (!postResult.IsSuccessStatusCode)
                    {
                        LogService.LogMessage("uploadcontent" + postResult);
                        return "failed";
                    }
                    else
                    {
                        return "success";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
                LogService.LogMessage("upload:" + ex.Message);
            }
          
        }

        //public static async Task<string> UploadMap(ImageFile content)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            string call_url = Base_URL + "/upload/import_map";
        //            string payload = JsonSerializer.Serialize(content);
        //            //var ms = new MemoryStream();
        //            //await content.CopyToAsync(ms).ConfigureAwait(false);
        //            //HttpContent copyContent = new StreamContent(ms);
        //            //if (content.Headers != null)
        //            //{
        //            //    foreach (var h in content.Headers)
        //            //    {
        //            //        copyContent.Headers.Add(h.Key, h.Value);
        //            //    }
        //            //}

        //            var httpResponse = await client.PatchAsync(call_url, JsonSerializer.Serialize(content), CancellationToken.None);
        //            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
        //            {
        //                return "";
        //            }
        //            else
        //            {
        //                string errorMessage = await httpResponse.Content.ReadAsStringAsync();
        //                LogService.LogMessage(errorMessage);
        //                return errorMessage;
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        LogService.LogMessage(ex.Message);
        //        return ex.Message;
        //    }
        //}

        public static async Task<HttpResponseMessage> MapDownLoad(string mapName)
        {
          
            try
            {
            
                using (var client = new HttpClient())
                {
                     string call_url = Base_URL + "/download/export_map";
                    //  var stringPayload = "{ \"name\":\"" + mapName + "\"}";
                    var stringPayload = JsonSerializer.Serialize(new MapName() { name = mapName });
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    var httpResponse = await client.PostAsync(call_url, httpContent);
                    httpResponse.EnsureSuccessStatusCode();
                    return httpResponse;
                }
             }
            catch (Exception ex)
            {
                LogService.LogMessage("dataservice mapdownload " + ex.Message );
                return null;
            }
        }

    
    }
}
