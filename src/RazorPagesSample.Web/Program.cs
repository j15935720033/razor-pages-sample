using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RazorPagesSample.Web
{
    /// <summary>
    /// ASP.NET Core 應用程式的進入點。
    /// 
    /// 當專案啟動時，會先執行此類別中的 Main 方法，
    /// 接著建立並啟動 Web 主機。
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        /// <param name="args">
        /// 啟動程式時傳入的命令列參數。
        /// </param>
        public static void Main(string[] args)
        {
            // 建立 Web 主機設定。
            IWebHostBuilder webHostBuilder = CreateWebHostBuilder(args);

            // 根據設定建立 IWebHost 物件。
            IWebHost webHost = webHostBuilder.Build();

            // 啟動 Web 主機。
            //
            // Run() 會讓網站持續執行並監聽瀏覽器送進來的 HTTP 請求，
            // 直到應用程式被關閉為止。
            webHost.Run();
        }

        /// <summary>
        /// 建立 ASP.NET Core Web 主機。
        /// </summary>
        /// <param name="args">
        /// 啟動程式時傳入的命令列參數。
        /// </param>
        /// <returns>
        /// 回傳設定完成的 IWebHostBuilder。
        /// </returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            // 建立 ASP.NET Core 預設的 Web 主機設定。
            //
            // CreateDefaultBuilder() 會自動完成許多常用設定，例如：
            // 1. 使用 Kestrel Web 伺服器。
            // 2. 讀取 appsettings.json。
            // 3. 讀取 appsettings.{Environment}.json。
            // 4. 讀取環境變數。
            // 5. 設定主控台 Console 日誌。
            // 6. 設定目前專案的內容根目錄。
            return WebHost.CreateDefaultBuilder(args)

                // 指定使用 Startup 類別。
                //
                // ASP.NET Core 啟動時，會到 Startup.cs 中執行：
                // 1. ConfigureServices()：註冊服務與依賴注入。
                // 2. Configure()：設定 HTTP 請求處理流程。
                .UseStartup<Startup>();
        }
    }
}