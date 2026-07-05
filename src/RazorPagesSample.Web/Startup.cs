using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RazorPagesSample.Web.Data;
using RazorPagesSample.Web.RouteConstraints;
using RazorPagesSample.Web.Services;

namespace RazorPagesSample.Web
{
    public class Startup
    {
        /// <summary>
        /// Startup 類別的建構子。
        /// ASP.NET Core 啟動時，會自動把 IConfiguration 傳入。
        /// IConfiguration 可以讀取 appsettings.json、環境變數等設定資料。
        /// </summary>
        /// <param name="configuration">網站的設定資料</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 儲存網站的設定資料。
        /// 例如可以用來讀取 appsettings.json。
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 註冊網站需要使用的服務。
        ///
        /// 這個方法會在網站啟動時由 ASP.NET Core 自動呼叫。
        /// 主要用途是把服務加入 DI 相依性注入容器。
        /// </summary>
        /// <param name="services">ASP.NET Core 的服務集合</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 加入 MVC 功能。
            // ASP.NET Core 2.2 的 Razor Pages 也是透過 MVC 系統運作。
            services.AddMvc()

                // 設定 MVC 的相容版本為 ASP.NET Core 2.2。
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)

                // 設定 Razor Pages 的路由規則。
                .AddRazorPagesOptions(options =>
                {
                    // 把 Pages/Index.cshtml 額外對應到 /home。
                    //
                    // 使用者輸入：
                    // https://localhost:xxxx/home
                    //
                    // 實際執行：
                    // Pages/Index.cshtml
                    options.Conventions.AddPageRoute("/index", "home");

                    // 把 Pages/Index.cshtml 額外對應到 /start。
                    //
                    // 使用者輸入：
                    // https://localhost:xxxx/start
                    //
                    // 實際執行：
                    // Pages/Index.cshtml
                    options.Conventions.AddPageRoute("/index", "start");
                });

            // 設定 ASP.NET Core 的路由選項。
            services.Configure<RouteOptions>(options =>
            {
                // 註冊自訂的路由限制 promo。
                //
                // promo 是路由限制的名稱。
                // PromoConstraint 是實際執行限制判斷的類別。
                //
                // 之後可以在路由中寫成：
                // {參數名稱:promo}
                options.ConstraintMap.Add(
                    "promo",
                    typeof(PromoConstraint)
                );
            });

            // 把 IEmailService 介面與 EmailService 類別註冊到 DI 容器。
            //
            // 當其他類別要求 IEmailService 時，
            // ASP.NET Core 會自動建立 EmailService。
            //
            // AddScoped 表示：
            // 每一次 HTTP Request 使用同一個物件，
            // 不同 Request 會建立不同物件。
            services.AddScoped<IEmailService, EmailService>();

            // 把 IBookService 介面與 BookService 類別註冊到 DI 容器。
            //
            // 當其他類別要求 IBookService 時，
            // ASP.NET Core 會自動建立 BookService。
            //
            // AddScoped 表示：
            // 每一次 HTTP Request 使用同一個物件，
            // 不同 Request 會建立不同物件。
            services.AddScoped<IBookService, BookService>();

            // 加入 Logging 日誌功能。
            //
            // 註冊後可以在類別中注入：
            // ILogger<類別名稱>
            services.AddLogging();

            // 加入網站健康檢查功能。
            //
            // 後面會使用：
            // app.UseHealthChecks("/health");
            services.AddHealthChecks();

            // 把 BookContext 註冊到 DI 容器。
            //
            // BookContext 通常是 Entity Framework Core 的資料庫操作類別。
            services.AddDbContext<BookContext>(options =>
            {
                // 使用記憶體資料庫。
                //
                // 資料只會暫存在程式記憶體中，
                // 網站重新啟動後資料就會消失。
                //
                // bookTestDb 是記憶體資料庫的名稱。
                options.UseInMemoryDatabase("bookTestDb");
            });
        }

        /// <summary>
        /// 設定 HTTP Request 的處理流程。
        ///
        /// 這個方法會在網站啟動時由 ASP.NET Core 自動呼叫。
        /// 方法中的 Middleware 會依照由上到下的順序執行。
        /// </summary>
        /// <param name="app">用來設定 Middleware</param>
        /// <param name="env">目前網站執行環境</param>
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env
        )
        {
            /*
             * 自訂健康檢查 Middleware 範例。
             *
             * 目前整段被註解，所以不會執行。
             *
             * app.Use(async (context, next) =>
             * {
             *     // 判斷網址中是否包含 health。
             *     if (context.Request.Path.Value.Contains("health"))
             *     {
             *         // 如果包含 health，直接回傳文字。
             *         await context.Response.WriteAsync(
             *             "App is healthy"
             *         );
             *     }
             *     else
             *     {
             *         // 如果不是 health 網址，
             *         // 繼續執行下一個 Middleware。
             *         await next.Invoke();
             *     }
             * });
             */

            // 啟用健康檢查網址。
            //
            // 瀏覽：
            // https://localhost:xxxx/health
            //
            // ASP.NET Core 會執行健康檢查。
            app.UseHealthChecks("/health");

            // 判斷目前是不是開發環境。
            if (env.IsDevelopment())
            {
                // 開發環境發生錯誤時，
                // 顯示詳細的錯誤訊息、程式碼位置和堆疊資訊。
                //
                // 這個功能適合開發時除錯，
                // 不適合正式環境使用。
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // 正式環境發生錯誤時，
                // 導向 Razor Page 的 /Error 頁面。
                app.UseExceptionHandler("/Error");

                // 啟用 HSTS。
                //
                // HSTS 會要求瀏覽器之後只能使用 HTTPS 連線網站。
                // ASP.NET Core 預設的 HSTS 有效時間是 30 天。
                app.UseHsts();
            }

            // 把 HTTP 網址重新導向到 HTTPS。
            //
            // 例如：
            // http://localhost:5000
            //
            // 重新導向到：
            // https://localhost:5001
            app.UseHttpsRedirection();

            // 允許網站讀取靜態檔案。
            //
            // 靜態檔案通常放在 wwwroot 資料夾中，例如：
            // wwwroot/css
            // wwwroot/js
            // wwwroot/images
            app.UseStaticFiles();

            // 啟用 MVC 路由處理。
            //
            // 在 ASP.NET Core 2.2 中，
            // Razor Pages 也是透過 UseMvc() 處理路由。
            //
            // 它會根據網址找到對應的：
            // Razor Page
            // Controller
            // Action
            app.UseMvc();
        }
    }
}