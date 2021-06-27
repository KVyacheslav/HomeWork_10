using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HomeWork_10
{
    /// <summary>
    /// Класс, описывающий погоду.
    /// </summary>
    public static class Weather
    {
        private static string url = "http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&appid={1}";
        private static HttpWebRequest httpWebRequest;
        private static HttpWebResponse httpWebResponse;

        /// <summary>
        /// Перевести город с русского на английский.
        /// </summary>
        /// <param name="city">Город на русском языке.</param>
        /// <returns>Город на английском языке.</returns>
        private static string GetEngCity(string city)
        {
            string api = "dict.1.1.20210627T052011Z.17793f7943de03a8.9813059669482579d87c1b1853c0bc6b698228dd";
            string urlCity = string.Format("https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key={0}&lang=ru-en&text={1}",
                api, city);
            httpWebRequest = (HttpWebRequest)WebRequest.Create(urlCity);
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            try
            {
                return JObject.Parse(response)["def"][0]["tr"][0]["text"].ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Перевести город с английского на русский.
        /// </summary>
        /// <param name="city">Город на английском языке.</param>
        /// <returns>Город на русском языке.</returns>
        private static string GetRusCity(string city)
        {
            string api = "dict.1.1.20210627T052011Z.17793f7943de03a8.9813059669482579d87c1b1853c0bc6b698228dd";
            string urlCity = string.Format("https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key={0}&lang=en-ru&text={1}",
                api, city);
            httpWebRequest = (HttpWebRequest)WebRequest.Create(urlCity);
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            return JObject.Parse(response)["def"][0]["tr"][0]["text"].ToString();
        }

        /// <summary>
        /// Получить погоду.
        /// </summary>
        /// <param name="city">Город.</param>
        /// <returns>Погода.</returns>
        public static string GetWeather(string city)
        {
            string engCity = GetEngCity(city);

            try
            {
                string api = "6fa9b3a5be90fe58476558f094b6a379";
                httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format(url, engCity, api));
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string response;

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                JObject weather = JObject.Parse(response);
                double temp = Convert.ToDouble(weather["main"]["temp"].ToString());
                city = GetRusCity(engCity);

                return $"Погода в городе {city}: {temp:##.#} °C";
            }
            catch
            {
                return null;
            }
        }
    }
}
