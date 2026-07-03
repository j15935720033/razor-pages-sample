// 引用 ASP.NET Core MVC 功能。
// IActionResult、BindProperty 等功能來自這個命名空間。
using Microsoft.AspNetCore.Mvc;

// 引用 Razor Pages 功能。
// PageModel、Page()、RedirectToPageResult 等功能來自這個命名空間。
using Microsoft.AspNetCore.Mvc.RazorPages;

// 引用專案中的資料模型。
// Contact 類別定義在 Models 資料夾中。
using RazorPagesSample.Web.Models;

// 引用專案中的服務。
// IEmailService 定義在 Services 資料夾中。
using RazorPagesSample.Web.Services;

namespace RazorPagesSample.Web.Pages
{
    /// <summary>
    /// Contact.cshtml 對應的 PageModel。
    /// 負責處理聯絡表單顯示、送出聯絡資料以及訂閱電子郵件。
    /// </summary>
    public class ContactModel : PageModel
    {
        // 儲存寄送電子郵件的服務物件。
        // readonly 表示這個欄位只能在建構子中設定。
        private readonly IEmailService _emailService;

        /// <summary>
        /// 接收 Contact.cshtml 表單送出的聯絡資料。
        /// </summary>
        // [BindProperty] 表示 ASP.NET Core 會自動將表單欄位資料，
        // 綁定到這個 Contact 屬性中。
        [BindProperty]
        public Contact Contact { get; set; }

        /// <summary>
        /// 顯示給使用者看的訊息。
        /// </summary>
        // private set 表示外部只能讀取，
        // 只有 ContactModel 類別內部可以修改。
        public string Message { get; private set; }

        /// <summary>
        /// ContactModel 的建構子。
        /// ASP.NET Core 會透過相依性注入 DI，
        /// 自動傳入實作 IEmailService 的服務物件。
        /// </summary>
        /// <param name="emailService">電子郵件服務</param>
        public ContactModel(IEmailService emailService)
        {
            // 將注入進來的電子郵件服務，
            // 儲存在 _emailService 欄位中。
            _emailService = emailService;
        }

        /// <summary>
        /// 使用 GET 方法進入 Contact 頁面時執行。
        /// 例如瀏覽器開啟 /Contact。
        /// </summary>
        public void OnGet()
        {
            // 目前沒有需要初始化的資料，
            // 所以這個方法暫時沒有程式碼。
        }

        /// <summary>
        /// 使用 POST 方法送出主要聯絡表單時執行。
        /// </summary>
        /// <returns>回傳要顯示或導向的頁面</returns>
        public IActionResult OnPost()
        {
            // 檢查表單資料是否通過模型驗證。
            // 例如 Required、EmailAddress 等驗證屬性。
            if (ModelState.IsValid)
            {
                // 表單資料正確時，
                // 呼叫電子郵件服務寄送聯絡表單內容。
                _emailService.SendMail(Contact);

                // 寄送完成後，重新導向到 Confirmation Razor Page。
                //
                // 第一個參數 "Confirmation"：
                // 表示導向 Pages/Confirmation.cshtml。
                //
                // 第二個參數 "Contact"：
                // 表示指定 Page Handler 為 Contact。
                //
                // 最後會執行 ConfirmationModel 中的：
                // OnGetContact()
                return new RedirectToPageResult("Confirmation", "Contact");
            }

            // 如果表單驗證失敗，
            // 留在目前的 Contact.cshtml 頁面，
            // 並顯示驗證錯誤訊息。
            return Page();
        }

        /// <summary>
        /// 處理訂閱電子郵件的表單。
        /// </summary>
        /// <param name="address">使用者輸入的電子郵件地址</param>
        /// <returns>導向訂閱完成頁面</returns>
        //
        // 方法名稱是 OnPostSubscribe，
        // 所以 Contact.cshtml 表單通常會使用：
        //
        // asp-page-handler="Subscribe"
        //
        // 送出後就會執行這個方法。
        public IActionResult OnPostSubscribe(string address)
        {
            // 呼叫電子郵件服務，
            // 處理使用者輸入的訂閱信箱地址。
            _emailService.SendMail(address);

            // 處理完成後，導向 Confirmation Razor Page。
            //
            // 第二個參數 "Subscribe" 表示執行：
            // ConfirmationModel 中的 OnGetSubscribe()
            return new RedirectToPageResult("Confirmation", "Subscribe");
        }
    }
}