using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verify5_Demo
{
    class Token
    {
        /// <summary>
        /// 网站后台刷新token
        /// </summary>
        /// <param name="appid">verify5提供的appId, 必填项</param>
        /// <param name="expiredIn">新生成的token有效时间,单位毫秒 选填，默认30天</param>
        public void RefreshToken(String appid, Nullable<Int64> expiredIn)
        {
            String timestamp = Program.UnixTimestamp();
            SortedDictionary<String, String> dictionary = new SortedDictionary<String, String>();
            dictionary.Add("appid", appid);
            if (expiredIn != null)
            {
                dictionary.Add("expiredIn", expiredIn + "");
            }
            dictionary.Add("timestamp", timestamp + "");

            String apiUrl = Program.appHost + "/openapi/getToken";
            try
            {
                String json = Program.SendHttpRequest(apiUrl, dictionary);
                System.Console.WriteLine(json);
                IDictionary<String,object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                System.Console.WriteLine("success = " + dict["success"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
