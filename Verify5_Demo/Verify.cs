using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Verify5_Demo
{
    /// <summary>
    /// 网站二次验证
    /// </summary>
    class Verify
    {
        /// <summary>
        /// 网站后台获取验证结果
        /// </summary>
        /// <param name="token">前端验证时使用的Token</param>
        /// <param name="verifyid">前端生成的uuid</param>
        /// <param name="userId">自定义参数，添加自定义参数后在风险告警和数据分析中可以关联到业务数据</param>
        /// <param name="userId">自定义参数，添加自定义参数后在风险告警和数据分析中可以关联到业务数据</param>	    
        public void ServerVerify(String token, String verifyid , String userId, String userId)
        {
            String timestamp = Program.UnixTimestamp();
            SortedDictionary<String, String> dictionary = new SortedDictionary<String, String>();
            dictionary.Add("token", token);
            dictionary.Add("verifyid", verifyid);
            dictionary.Add("timestamp", timestamp);

	    dictionary.Add("userId", userId);
	    dictionary.Add("orderId", orderId);
            
            String apiUrl = Program.appHost + "/openapi/verify";
            try
            {
                String json = Program.SendHttpRequest(apiUrl, dictionary);
                System.Console.WriteLine(json);
                IDictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                System.Console.WriteLine("success = " + dict["success"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
