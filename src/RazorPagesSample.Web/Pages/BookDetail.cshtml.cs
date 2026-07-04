using System.Linq; // 提供 SingleOrDefault() 等 LINQ 查詢方法
using Microsoft.AspNetCore.Mvc.RazorPages; // 提供 Razor Pages 的 PageModel
using RazorPagesSample.Web.Models; // 引用 Book 資料模型
using RazorPagesSample.Web.Services; // 引用 IBookService 書籍服務介面

namespace RazorPagesSample.Web.Pages
{
    /// <summary>
    /// BookDetail.cshtml 頁面所使用的 PageModel。
    /// 負責取得指定書籍的詳細資料。
    /// </summary>
    public class BookDetailModel : PageModel
    {
        /// <summary>
        /// 書籍服務。
        /// 用來取得書籍資料。
        /// readonly 表示欄位只能在宣告時或建構子中指定。
        /// </summary>
        private readonly IBookService _bookService;

        /// <summary>
        /// 要顯示在 BookDetail.cshtml 頁面上的書籍資料。
        ///
        /// get：BookDetail.cshtml 可以讀取這個屬性。
        /// private set：只有 BookDetailModel 類別內部可以修改。
        /// </summary>
        public Book Book { get; private set; }

        /// <summary>
        /// 建構子。
        ///
        /// ASP.NET Core 會透過依賴注入（Dependency Injection）
        /// 自動將 IBookService 的實作物件傳入。
        /// </summary>
        /// <param name="bookService">提供書籍資料的服務物件</param>
        public BookDetailModel(IBookService bookService)
        {
            // 將注入進來的書籍服務儲存在欄位中，
            // 讓其他方法可以使用它取得書籍資料。
            _bookService = bookService;
        }

        /// <summary>
        /// 當使用者使用 GET 方法進入 BookDetail 頁面時執行。
        ///
        /// 例如網址：
        /// /BookDetail?slug=clean-code
        ///
        /// 此時 slug 的值就是 "clean-code"。
        /// </summary>
        /// <param name="slug">
        /// 從網址或路由取得的書籍識別文字
        /// </param>
        public void OnGet(string slug)
        {
            // _bookService.GetBooks()
            // 取得所有書籍資料。
            //
            // x => x.Slug == slug
            // 檢查每一本書的 Slug 是否等於網址傳入的 slug。
            //
            // SingleOrDefault()
            // 如果剛好找到一本符合條件的書，就回傳該書。
            // 如果完全找不到，則回傳 null。
            // 如果找到超過一本，則會發生例外錯誤。
            Book = _bookService
                .GetBooks()
                .SingleOrDefault(x => x.Slug == slug);
        }
    }
}