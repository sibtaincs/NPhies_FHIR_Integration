using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;


namespace Nphies.Core.Data.Common
{

    public static class ApiHelper
    {
        /// <summary>
        /// Calls an API endpoint asynchronously and returns the response.
        /// </summary>
        /// <typeparam name="TResponse">Type of the expected response.</typeparam>
        /// <param name="apiUrl">The URL of the API endpoint.</param>
        /// <param name="method">The HTTP method (e.g., HttpMethod.Get, HttpMethod.Post).</param>
        /// <param name="requestBody">The request body to send (null for GET).</param>
        /// <param name="authenticationRequired">Indicates whether authentication is required for the request.</param>
        /// <param name="token">Bearer token for authentication (null if not required).</param>
        /// <returns>The deserialized response of type TResponse.</returns>
        /// <exception cref="Exception">Thrown if the API call is not successful (status code other than 2xx).</exception>
        public static async Task<T> CallApiAsync<T>(string apiUrl,HttpMethod method,
                        object requestBody = null,bool authenticationRequired = false,string token = null,
                        bool isXmlResponse = false)
        {
            using (HttpClient client = new HttpClient())
            {
                if (authenticationRequired && !string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using (HttpRequestMessage request = new HttpRequestMessage(method, apiUrl))
                {
                    // Only set the content for methods that allow a body
                    if (requestBody != null && (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Delete))
                    {
                        var json = JsonConvert.SerializeObject(requestBody);
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    }

                    try
                    {
                        using (HttpResponseMessage response = await client.SendAsync(request))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = await response.Content.ReadAsStringAsync();
                                if (isXmlResponse)
                                {
                                    // Return the raw XML string if specified
                                    return (T)(object)responseContent;
                                }
                                else
                                {
                                    // Otherwise, deserialize as JSON
                                    return JsonConvert.DeserializeObject<T>(responseContent);
                                }
                            }
                            else
                            {
                                throw new Exception("API call was not successful. Status code: " + response.StatusCode);
                            }
                        }
                    }
                    catch (TaskCanceledException ex)
                    {
                        Console.WriteLine("Request timed out.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error during API call: " + ex.Message);
                    }
                }

                return default; // Return a default value of type T
            }
        }


    }
}
