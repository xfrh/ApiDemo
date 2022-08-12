using ApiDemoApp.Models;
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

        public async Task<AGVStatus> StartNav(Coordinace coordinace)
        {
            //  var client = _client.CreateClient("cmd");
            var client = _client.CreateClient();
            string? statusMessage;
            try
            {
                string call_url = Base_URL + "/cmd/nav";
                coordinace.theta = coordinace.theta * Math.PI / 180;
                var stringPayload = JsonSerializer.Serialize(coordinace);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(call_url, httpContent);
                statusMessage = await httpResponse.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AGVStatus>(statusMessage);
                //if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                //{
                //    statusMessage = "";
                //}
                //else
                //{
                //    statusMessage = await httpResponse.Content.ReadAsStringAsync();
                //    LogService.LogMessage(statusMessage);
                //}
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return null;
            }
            
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
                    type = 0,
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
