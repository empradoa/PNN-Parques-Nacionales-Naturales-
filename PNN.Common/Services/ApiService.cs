using Newtonsoft.Json;
using Plugin.Connectivity;
using PNN.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PNN.Common.Services
{
    public class ApiService : IApiService
    {
        
        public async Task<Response<UserResponse>> GetOwnerByEmailAsync(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, string email)
         {
            try
            {
                var request = new EmailRequest { Email = email };
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<UserResponse>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var user = JsonConvert.DeserializeObject<UserResponse>(result);
                return new Response<UserResponse>
                {
                    IsSuccess = true,
                    Result = user
                };
            }
            catch (Exception ex)
            {
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<TokenResponse>> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request)
         {
                    try
                    {
                        var requestString = JsonConvert.SerializeObject(request);
                        var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                        var client = new HttpClient
                        {
                            BaseAddress = new Uri(urlBase)
                        };

                        var url = $"{servicePrefix}{controller}";
                        var response = await client.PostAsync(url,content);
                        var result = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                        {
                            return new Response<TokenResponse>
                            {
                                IsSuccess = false,
                                Message = result,
                            };
                        }

                        var token = JsonConvert.DeserializeObject<TokenResponse>(result);
                        return new Response<TokenResponse>
                        {
                            IsSuccess = true,
                            Result = token
                        };
                    }
                    catch (Exception ex)
                    {
                        return new Response<TokenResponse>
                        {
                            IsSuccess = false,
                            Message = ex.Message
                        };
                    }

         }

        public async Task<bool> CheckConnectionAsync(string url)
        {
            if (!CrossConnectivity.Current.IsConnected) 
            {
                return false;
            }

            return await CrossConnectivity.Current.IsRemoteReachable(url);
        }

        public async Task<Response<PublicationsResponse>> GetContentsAsync(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken)
        {
            try
            {

                var content = new StringContent(String.Empty);
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<PublicationsResponse>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var publications = JsonConvert.DeserializeObject<PublicationsResponse>(result);
                return new Response<PublicationsResponse>
                {
                    IsSuccess = true,
                    Result = publications
                };
            }
            catch (Exception ex)
            {
                return new Response<PublicationsResponse>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

    }
        
}
