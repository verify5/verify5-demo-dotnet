using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Verify5_Demo
{
    class Program
    {
        //请在verify5控制台获取以下数据
        public static String appKey = "[请在后台获取APP KEY]";
        public static String appId = "[请在后台获取APP ID]";
        public static String appHost = "https://[请在后台获取APP HOST]";
        static void Main(string[] args)
        {
            System.Console.WriteLine("Refresh Token...");
            Token token = new Token();
            token.RefreshToken(appId, 24 * 60 * 60 * 1000);
            Verify verify = new Verify();
            System.Console.WriteLine("Server Verify...");
            verify.ServerVerify("599d20210a6445d0901c8b183b9660ff", "122222", "myUserId" , "myOrder0223232");
            Console.ReadKey();
        }


        public static String SendHttpRequest(String apiUrl, SortedDictionary<String, String> parameters)
        {
            String signature = CalcSignature(parameters);

            StringBuilder queryParams = new StringBuilder();
            //queryParams.Append("?");
            foreach (KeyValuePair<String, String> param in parameters)
            {
                queryParams.Append("&").Append(param.Key).Append("=").Append(param.Value);
            }
            queryParams.Append("&signature=").Append(signature);
            queryParams.Remove(0, 1);
            String url = apiUrl + "?" + queryParams.ToString();
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader streamReader = null;
            string respStr = null;
            try
            {
                request.Timeout = 30 * 1000;
                request.ReadWriteTimeout = 15 * 1000;
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                streamReader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
                respStr = streamReader.ReadToEnd();

                //获取返回结果并解析
                IDictionary<String,object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(respStr);
                System.Console.WriteLine(dict.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                try
                {
                    stream.Close();
                    streamReader.Close();
                    response.Close();
                }
                catch (NullReferenceException)
                {

                }
            }

            return respStr;
        }
        /// <summary>
        /// 加签
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static String CalcSignature(SortedDictionary<String, String> parameters)
        {
            StringBuilder content = new StringBuilder();
            foreach (KeyValuePair<String, String> item in parameters)
            {
                content.Append(item.Key).Append(item.Value);
            }
            content.Append(appKey);
            String siganutre = MD5(content.ToString());
            return siganutre;
        }

        public static String UnixTimestamp()
        {
            DateTime DateTime1970 = new DateTime(1970, 1, 1);
            TimeSpan t = DateTime.Now - DateTime1970;
            return ((long)t.TotalMilliseconds).ToString();
        }

        /// <summary>
        /// MD5签名方法
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public static string MD5(string inputText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(inputText);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }

            return byte2String;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }

}
