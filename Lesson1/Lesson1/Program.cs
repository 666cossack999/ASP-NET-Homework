using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Lesson1
{
    class Program
    {
        /// <summary>
        /// Записывает пост в файл C:\tmp\result.txt
        /// </summary>
        /// <param name="response">Строка JSON</param>
        /// <param name="i">id Поста</param>
        static async Task WriteText(string response, int i)
        {
            string writePath = @"C:\tmp\result.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    var text = JsonConvert.DeserializeObject<JsonResponse>(response);

                    await sw.WriteLineAsync(text.userId);
                    await sw.WriteLineAsync(text.id);
                    await sw.WriteLineAsync(text.title);
                    await sw.WriteLineAsync(text.body);
                    await sw.WriteLineAsync();
                }


                Console.WriteLine($"{i-3}) Запись поста №{i} выполнена");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Делает запрос по ссылке
        /// </summary>
        /// <param name="url">ссылка</param>
        /// <returns>строку JSON</returns>
        static async Task<string> ReadFromUrl(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
               return streamReader.ReadToEnd();
            }

            
        }
       
        static async Task Main(string[] args)
        {
            //С какого id поста начать запись
            int fromPost = 4, 
                //на каком id поста закончить запись
                toPost = 13;

            for (int i = fromPost; i <= toPost; i++)
            {
                string url = $"https://jsonplaceholder.typicode.com/posts/{i}";

                var response = await ReadFromUrl(url);

                await WriteText(response, i);
            }
            
            Console.ReadLine();
        }
    }
}
