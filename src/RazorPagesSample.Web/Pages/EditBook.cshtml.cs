using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesSample.Web.Models;
using RazorPagesSample.Web.Services;

namespace RazorPagesSample.Web.Pages
{
    /// <summary>
    /// 編輯書籍頁面的 PageModel。
    /// 負責處理 EditBook.cshtml 的資料讀取與表單送出。
    /// </summary>
    public class EditBookModel : PageModel
    {
        // 書籍服務，用來查詢及更新書籍資料。
        /// readonly 表示欄位只能在宣告時或建構子中指定。
        private readonly IBookService _bookService;

        /// <summary>
        /// 建構子。
        /// ASP.NET Core 會透過相依性注入（DI）
        /// 自動將 IBookService 傳入這個類別。
        /// </summary>
        /// <param name="bookService">書籍服務物件</param>
        public EditBookModel(IBookService bookService)
        {
            // 將傳入的書籍服務保存到欄位中，
            // 讓其他方法可以使用。
            _bookService = bookService;
        }

        /// <summary>
        /// 目前要編輯的書籍資料。
        /// </summary>
        // BindProperty 表示表單送出時，
        // ASP.NET Core 會自動把表單欄位資料綁定到 Book 屬性。
        [BindProperty]
        public Book Book { get; set; }

        /// <summary>
        /// 當使用者使用 GET 方式進入編輯頁面時執行。
        /// 例如：/EditBook?id=1
        /// </summary>
        /// <param name="id">要編輯的書籍編號</param>
        /// <returns>編輯頁面或 404 找不到頁面</returns>
        public IActionResult OnGet(int id)
        {
            // 根據書籍編號取得書籍資料。
            var book = _bookService.GetBook(id);

            // 如果找不到對應的書籍，
            // 回傳 HTTP 404 Not Found。
            if (book == null)
            {
                return NotFound();
            }

            // 將查詢到的書籍資料指定給 Book 屬性。
            // EditBook.cshtml 可以使用 Model.Book 顯示資料。
            Book = book;

            // 顯示目前的 EditBook.cshtml 頁面。
            return Page();
        }

        /// <summary>
        /// 當使用者送出編輯表單時執行。
        /// </summary>
        /// <returns>目前頁面或書籍列表頁面</returns>
        public IActionResult OnPost()
        {
            // 檢查表單資料是否通過模型驗證。
            // 例如：[Required]、[StringLength] 等驗證規則。
            if (!ModelState.IsValid)
            {
                // 驗證失敗時留在目前頁面，
                // 並顯示驗證錯誤訊息。
                return Page();
            }

            // 將使用者修改後的書籍資料更新到資料來源。
            _bookService.UpdateBook(Book);

            // 更新成功後，重新導向 Books Razor Page。
            // 通常會前往 Pages/Books.cshtml。
            return RedirectToPage("Books");
        }
    }
}