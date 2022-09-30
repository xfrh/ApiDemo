using ApiDemoApp.Models;
using ApiDemoApp.Pages;
using MudBlazor.Charts.SVG.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;

namespace ApiDemoApp.Services
{
    public class DataService
    {
        private readonly IHttpClientFactory _client;
    
        public DataService(IHttpClientFactory httpClientFactory) =>
         _client = httpClientFactory;
       
        public string Base_URL { get; set; }
        public async Task<Coordinace> Cur_Position()
        {
            // var client = _client.CreateClient("nav");
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/reeman/pose";
                var q = await client.GetFromJsonAsync<Coordinace>(call_url);
                return q;

            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return await Task.FromResult<Coordinace>(null);
            }
        }

        public async Task<String> IMU()
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/reeman/imu";
                var q = await client.GetFromJsonAsync<string>(call_url);
                return q;

            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return await Task.FromResult<String>(null);
            }
        }

        public async Task<AGVSpeed> Speed()
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/reeman/speed";
                var q = await client.GetFromJsonAsync<AGVSpeed>(call_url);
                return q;

            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return await Task.FromResult<AGVSpeed>(null);
            }
        }

        public async Task<List<Coordinace>> Local_Plan()
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/reeman/local_plan";
                return await client.GetFromJsonAsync<List<Coordinace>>(call_url);
               

            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return await Task.FromResult<List<Coordinace>>(null);
            }
        }

        public async Task<List<Coordinace>> Global_Plan()
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/reeman/global_plan";
                return await client.GetFromJsonAsync<List<Coordinace>>(call_url);


            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return await Task.FromResult<List<Coordinace>>(null);
            }
        }

        public async Task<MoveStatus> Cur_Status()
        {
            //  var client = _client.CreateClient("nav");
            var client = _client.CreateClient();

            try
            {
                string call_url = Base_URL + "/reeman/movebase_status";
                return await client.GetFromJsonAsync<MoveStatus>(call_url);

            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
                return await Task.FromResult<MoveStatus>(null);
            }
        }

        public async Task<List<TargetPointsModel>> Target_Points()
        {
            // var client = _client.CreateClient("nav");
            var client = _client.CreateClient();
            List<TargetPointsModel> lst = new List<TargetPointsModel>();
            try
            {
                string call_url = Base_URL + "/reeman/android_target";
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
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return await Task.FromResult<List<TargetPointsModel>>(null);
            }
        }

        public async Task<Dictionary<string, List<List<double>>>> Target_Routes()
        {
            try
            {
                // var client = _client.CreateClient("nav");
                var client = _client.CreateClient();
                string call_url = Base_URL + "/reeman/navi_routes";
                return await client.GetFromJsonAsync<Dictionary<string, List<List<double>>>>(call_url);
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return await Task.FromResult<Dictionary<string, List<List<double>>>>(null);
            }
        }

        public async Task<string> StartNav(Coordinace coordinace)
        {
            //  var client = _client.CreateClient("cmd");
            var client = _client.CreateClient();
            string statusMessage ="";
            try
            {
                string call_url = Base_URL + "/cmd/nav";
                coordinace.theta = coordinace.theta * Math.PI / 180;
                var stringPayload = JsonSerializer.Serialize(coordinace);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(call_url, httpContent);
                statusMessage = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    statusMessage = "";
                }
                else
                {
                    statusMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(statusMessage);
                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                
            }
            return statusMessage;
        }


        public async Task<string> StartNavByTargetName(TargetName pointname)
        {
            //  var client = _client.CreateClient("cmd");
            var client = _client.CreateClient();
            string statusMessage = "";
            try
            {
                string call_url = Base_URL + "/cmd/nav_name";
                var stringPayload = JsonSerializer.Serialize(pointname);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(call_url, httpContent);
                statusMessage = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    statusMessage = "";
                }
                else
                {
                    statusMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(statusMessage);
                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);

            }
            return statusMessage;
        }


        public async Task<string> ManuMoveCar(AGVSpeed speed)
        {
            //  var client = _client.CreateClient("cmd");
            var client = _client.CreateClient();
            string statusMessage = "";
            try
            {
                string call_url = Base_URL + "/cmd/speed";
                var stringPayload = JsonSerializer.Serialize(speed);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(call_url, httpContent);
                statusMessage = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    statusMessage = "";
                }
                else
                {
                    statusMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(statusMessage);
                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);

            }
            return statusMessage;
        }


        public async Task<string> CancelNav()
        {
            //   var client = _client.CreateClient("cmd");
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/cmd/cancel_goal";
                var httpResponse = await client.PostAsync(call_url, null);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "";
                }
                else
                {
                    string statusMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(statusMessage);
                    return statusMessage;
                }

            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
                return ex.Message;
            }
        }

        public async Task<string> NavCharge()
        {
            //  var client = _client.CreateClient("cmd");
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/cmd/charge";
                Charge charge = new Charge()
                {
                    type = 1,
                    point = "充电桩"
                };
                var stringPayload = JsonSerializer.Serialize(charge);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(call_url, httpContent);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "";
                }
                else
                {
                    string errorMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(errorMessage);
                    return errorMessage;
                }

            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
                return await Task.FromResult<string>(ex.Message);
            }
        }


        

        public async Task<string> BatteryEmpower()
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/cmd/lock";
                var httpResponse = await client.PostAsync(call_url, null);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "";
                }
                else
                {
                   string errorMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(errorMessage);
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return ex.Message;
            }

        }

        public async Task<string> BatteryUnEmpower()
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/cmd/unlock";
                var httpResponse = await client.PostAsync(call_url, null);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "";
                }
                else
                {
                    string errorMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(errorMessage);
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return ex.Message;
            }

        }

        #region mapmode

        public async Task<string> Switch_Map()
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/cmd/set_mode";
             
                var stringPayload = "{ \"mode\":3}";
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(call_url, httpContent);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "";
                }
                else
                {
                    string errorMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(errorMessage);
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return ex.Message;
            }
        }


        public async Task<string> Savemap()
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/cmd/save_map";
                var httpResponse = await client.PostAsync(call_url, null);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "";
                }
                else
                {
                    string errorMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(errorMessage);
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
                return ex.Message;
            }

        }

        public async Task<AGVMapModel> Cur_Map_Name()
        {
            //  var client = _client.CreateClient("nav");
            var client = _client.CreateClient();
            string call_url = Base_URL + "/reeman/cur_map";
            try
            {
                return await client.GetFromJsonAsync<AGVMapModel>(call_url);

            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return await Task.FromResult<AGVMapModel>(null);
            }
        }

        public async Task<List<AGVMapModel>> MapList()
        {
            //  var client = _client.CreateClient("nav");
            var client = _client.CreateClient();
            string call_url = Base_URL + "/reeman/history_map";
            try
            {
                return await client.GetFromJsonAsync<List<AGVMapModel>>(call_url);

            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return await Task.FromResult<List<AGVMapModel>>(null);
            }
        }

        public async Task<string> ApplyMap(AGVMapModel aGVMap)
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/cmd/save_map";
                var stringPayload = JsonSerializer.Serialize(aGVMap);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(call_url, httpContent);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "";
                }
                else
                {
                    string errorMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(errorMessage);
                    return errorMessage;
                }
               
            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
                return ex.Message;
            }

        }

        public async Task<string> MapDownLoad(AGVMapModel aGVMap)
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/cmd/export_map";
                var stringPayload = JsonSerializer.Serialize(aGVMap);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(call_url, httpContent);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "";
                }
                else
                {
                    string errorMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(errorMessage);
                    return errorMessage;
                }

            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
                return ex.Message;
            }
        }

        public async Task<string> RenameMap(AGVMapModel aGVMapOld, AGVMapModel aGVMapNew)
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/cmd/rename_map";
                var stringPayload = "{ \"old_name\":\"" + aGVMapOld.name + "\",\"new_name\":\""+ aGVMapNew.name + "\"}";
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(call_url, httpContent);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "";
                }
                else
                {
                    string errorMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(errorMessage);
                    return errorMessage;
                }

            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
                return ex.Message;
            }

        }


        public async Task<string> UploadMap(string filePath)
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/cmd/import_map";
                string _filename = Path.GetFileName(filePath);
                using (var multipartFormContent = new MultipartFormDataContent())
                {
                    //Load the file and set the file's Content-Type header
                    var fileStreamContent = new StreamContent(File.OpenRead(filePath));
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                    //Add the file
                    multipartFormContent.Add(fileStreamContent, name: "file", fileName: _filename);

                    //Send it
                    var response = await client.PostAsync(call_url, multipartFormContent);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return ex.Message;
            }
        }

        public async Task<string> Delete(AGVMapModel aGVMap)
        {
            var client = _client.CreateClient();
            try
            {
                string call_url = Base_URL + "/cmd/delete_map";
                var stringPayload = JsonSerializer.Serialize(aGVMap);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(call_url, httpContent);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "";
                }
                else
                {
                    string errorMessage = await httpResponse.Content.ReadAsStringAsync();
                    LogService.LogMessage(errorMessage);
                    return errorMessage;
                }

            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
                return ex.Message;
            }

        }

        public async Task<string> SpecialPloy()
        {
            //  var client = _client.CreateClient("nav");
            var client = _client.CreateClient();
            string call_url = Base_URL + "/reeman/special_polygon";
            try
            {
                return await client.GetFromJsonAsync<string>(call_url);

            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return await Task.FromResult<string>(null);
            }
        }

        #endregion

        public async Task<MapLayer> Cur_Map()
        {
            //  var client = _client.CreateClient("nav");
            var client = _client.CreateClient();
            string call_url = Base_URL + "/reeman/map";
            try
            {
                return await client.GetFromJsonAsync<MapLayer>(call_url);

            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return await Task.FromResult<MapLayer>(null);
            }
        }

        public async Task<Battery> Battery_Status()
        {
            // var client = _client.CreateClient("nav");
            var client = _client.CreateClient();
            string call_url = Base_URL + "/reeman/base_encode";
            try
            {
                return await client.GetFromJsonAsync<Battery>(call_url);

            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
                return await Task.FromResult<Battery>(null);
            }
        }

        public async Task<MoveStatus> Navi_Status()
        {
            //   var client = _client.CreateClient("nav");
            var client = _client.CreateClient();
            string call_url = Base_URL + "/reeman/movebase_status";
            try
            {
                return await client.GetFromJsonAsync<MoveStatus>(call_url);

            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
                return await Task.FromResult<MoveStatus>(null);
            }
        }

    }
}
