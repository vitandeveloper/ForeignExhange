
namespace ForeignExhange.Services
{
    using ForeignExhange.Helpers;
    using ForeignExhange.Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    public class ApiService
    {
        //validar conexion a internet
        public async Task<Response> CheckConnection()
        {
            //Validar que si esta el internet habilitado
            if(!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSucces = false,
                    Message = Lenguages.Error_settings,
                };
            }
            //Valida que si tenga datos el dispositivo probando conectarse a una pagina web
           /* var response = await CrossConnectivity.Current.IsReachable("google.com");
            if (!response)
            {
                return new Response
                {
                    IsSucces = false,
                    Message = Lenguages.Error_conecction,
                };
            }*/

            return new Response
            {
                IsSucces = true,
            };
        }

        //hacer peticion a un ws
        public async Task<Response> GetList<T>(string UrlBase,string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(UrlBase); 
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSucces = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);

                return new Response
                {
                    IsSucces = true,
                    Result = list,
                };
            }
            catch(Exception ex)
            {
                return new Response
                {
                    IsSucces = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
