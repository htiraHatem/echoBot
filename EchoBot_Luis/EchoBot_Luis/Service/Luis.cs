using EchoBot_Luis.Serialization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace EchoBot_Luis.Services
{
    public class Luis
    {
        public static async Task<Utterance> GetResponse(string message)
        {
            using (var client = new HttpClient())
            {
                const string authKey = "e2886707708144bba941f137b2120fc3";

                var url = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/9350fdf8-e08f-41a6-b6a4-46aec43dd35f?verbose=true&timezoneOffset=0&subscription-key={authKey}&q={message}";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode) return null;
                var result = await response.Content.ReadAsStringAsync();

                var js = new DataContractJsonSerializer(typeof(Utterance));
                var ms = new MemoryStream(Encoding.ASCII.GetBytes(result));
                var list = (Utterance)js.ReadObject(ms);

                return list;
            }
        }
    }
}