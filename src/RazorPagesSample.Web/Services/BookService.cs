using System;

// 提供泛型集合相關功能。
// 此程式中的 IEnumerable<Book> 來自這個命名空間。
using System.Collections.Generic;

// 提供 LINQ 查詢功能。
// SingleOrDefault()、OrderByDescending() 都來自這個命名空間。
using System.Linq;

// 匯入 BookContext 資料庫內容類別。
using RazorPagesSample.Web.Data;

// 匯入 Book 資料模型。
using RazorPagesSample.Web.Models;

namespace RazorPagesSample.Web.Services
{
    /// <summary>
    /// 書籍服務類別。
    ///
    /// 負責處理與書籍資料相關的操作，例如：
    /// 1. 查詢單一本書
    /// 2. 查詢全部書籍
    /// 3. 查詢最近出版的書籍
    /// 4. 修改書籍資料
    /// 5. 刪除書籍資料
    ///
    /// 此類別實作 IBookService 介面。
    /// </summary>
    public class BookService : IBookService
    {
        /// <summary>
        /// BookContext 資料庫內容物件。
        ///
        /// readonly 表示：
        /// 此欄位只能在宣告時或建構子中指定，
        /// 建構完成後不能再改成其他物件。
        /// </summary>
        private readonly BookContext _bookContext;

        /// <summary>
        /// BookService 建構子。
        ///
        /// ASP.NET Core 會透過依賴注入，
        /// 自動將 BookContext 物件傳入。
        /// </summary>
        /// <param name="bookContext">
        /// 用來存取 Books 資料表的資料庫內容物件。
        /// </param>
        public BookService(BookContext bookContext)
        {
            // 將注入進來的 BookContext 儲存在欄位中，
            // 讓此類別的其他方法可以使用它存取資料庫。
            _bookContext = bookContext;

            // 確認資料庫是否已經存在。
            //
            // 如果資料庫不存在，EnsureCreated() 會自動建立：
            // 1. 資料庫
            // 2. 資料表
            // 3. OnModelCreating() 中設定的初始資料
            //
            // 如果資料庫已存在，則不會重複建立。
            //
            // 注意：
            // EnsureCreated() 通常適合教學、測試或小型專案。
            // 正式專案通常會使用 Migration 管理資料庫結構。
            _bookContext.Database.EnsureCreated();
        }

        /// <summary>
        /// 根據書籍編號查詢單一本書。
        /// </summary>
        /// <param name="id">
        /// 要查詢的書籍編號。
        /// </param>
        /// <returns>
        /// 如果找到符合編號的書籍，回傳 Book 物件；
        /// 如果找不到，回傳 null。
        /// </returns>
        public Book GetBook(int id)
        {
            // _bookContext.Books：
            // 代表資料庫中的 Books 資料表。
            //
            // SingleOrDefault()：
            // 尋找唯一一筆符合條件的資料。
            //
            // x => x.Id == id：
            // 表示尋找 Book.Id 等於傳入 id 的書籍。
            //
            // 查詢結果：
            // 1. 找到一筆資料：回傳該 Book 物件
            // 2. 找不到資料：回傳 null
            // 3. 找到超過一筆：拋出例外
            return _bookContext.Books.SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// 取得全部書籍資料。
        /// </summary>
        /// <returns>
        /// 回傳所有書籍資料。
        /// </returns>
        public IEnumerable<Book> GetBooks()
        {
            // 直接回傳 Books 資料表。
            //
            // 因為 DbSet<Book> 可以當成 IEnumerable<Book> 使用，
            // 所以可以直接回傳。
            //
            // 真正執行查詢的時間，
            // 通常是在列舉資料或呼叫 ToList() 時。
            return _bookContext.Books;
        }

        /// <summary>
        /// 取得所有書籍，並依發行日期由新到舊排序。
        /// </summary>
        /// <returns>
        /// 回傳依 ReleaseDate 由新到舊排列的書籍資料。
        /// </returns>
        public IEnumerable<Book> GetRecentBooks()
        {
            // OrderByDescending()：
            // 使用指定欄位進行遞減排序。
            //
            // x => x.ReleaseDate：
            // 指定使用書籍的發行日期排序。
            //
            // 日期較新的書籍會排在前面。
            return _bookContext.Books
                .OrderByDescending(x => x.ReleaseDate);
        }

        /// <summary>
        /// 更新指定書籍資料。
        /// </summary>
        /// <param name="book">
        /// 包含修改後資料的 Book 物件。
        /// </param>
        public void UpdateBook(Book book)
        {
            // 告訴 EF Core：
            // 此 Book 物件的資料已經被修改。
            //
            // EF Core 會將此物件標記為 Modified 狀態。
            _bookContext.Update(book);

            // 將目前追蹤到的變更儲存到資料庫。
            //
            // 此處會執行對應的 SQL UPDATE 指令。
            _bookContext.SaveChanges();
        }

        /// <summary>
        /// 刪除指定書籍資料。
        /// </summary>
        /// <param name="book">
        /// 要刪除的 Book 物件。
        /// </param>
        public void DeleteBook(Book book)
        {
            // 告訴 EF Core：
            // 此 Book 物件要從資料庫中刪除。
            //
            // EF Core 會將此物件標記為 Deleted 狀態。
            _bookContext.Remove(book);

            // 將刪除操作儲存到資料庫。
            //
            // 此處會執行對應的 SQL DELETE 指令。
            _bookContext.SaveChanges();
        }
    }
}