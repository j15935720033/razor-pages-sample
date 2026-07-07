using System;

// 匯入 Entity Framework Core 相關功能。
// DbContext、DbSet、ModelBuilder 等類別都來自此命名空間。
using Microsoft.EntityFrameworkCore;

// 匯入此專案中的資料模型。
// Book 類別位於 RazorPagesSample.Web.Models 命名空間中。
using RazorPagesSample.Web.Models;

namespace RazorPagesSample.Web.Data
{
    /// <summary>
    /// 應用程式的資料庫內容類別。
    ///
    /// BookContext 繼承 DbContext，
    /// 是 Entity Framework Core 與資料庫之間的主要橋樑。
    ///
    /// 負責：
    /// 1. 連接資料庫
    /// 2. 管理資料表
    /// 3. 查詢資料
    /// 4. 新增、修改、刪除資料
    /// 5. 儲存資料庫變更
    /// 6. 設定資料表與初始資料
    /// </summary>
    public class BookContext : DbContext
    {
        /// <summary>
        /// BookContext 的建構子。
        ///
        /// DbContextOptions 包含資料庫連線及 EF Core 的設定，
        /// 例如：
        /// 1. 使用哪一種資料庫
        /// 2. 資料庫連線字串
        /// 3. EF Core 的其他設定
        ///
        /// ASP.NET Core 會透過依賴注入
        /// 自動將 DbContextOptions<BookContext> 傳入此建構子。
        /// </summary>
        /// <param name="options">
        /// BookContext 的資料庫連線與 EF Core 設定。
        /// </param>
        public BookContext(DbContextOptions<BookContext> options)
            : base(options)
        {
            // 將 options 傳給父類別 DbContext。
            //
            // 實際的資料庫連線設定會由 DbContext 處理，
            // 因此這裡不需要另外撰寫程式碼。
        }

        /// <summary>
        /// 對應資料庫中的 Books 資料表。
        ///
        /// DbSet<Book> 表示：
        /// 1. Book 對應資料表中的一筆資料
        /// 2. Books 對應整張 Books 資料表
        ///
        /// 可以透過此屬性進行：
        /// 1. 查詢書籍
        /// 2. 新增書籍
        /// 3. 修改書籍
        /// 4. 刪除書籍
        ///
        /// 例如：
        ///
        /// 查詢全部書籍：
        /// context.Books.ToList()
        ///
        /// 新增書籍：
        /// context.Books.Add(book)
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// 設定 Entity Framework Core 的資料模型。
        ///
        /// 此方法會在 EF Core 建立資料模型時執行，
        /// 可以用來設定：
        /// 1. 資料表名稱
        /// 2. 主鍵
        /// 3. 欄位限制
        /// 4. 資料表關聯
        /// 5. 初始資料
        /// </summary>
        /// <param name="modelBuilder">
        /// 用來設定 EF Core 資料模型的物件。
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 指定要設定 Book 實體。
            //
            // modelBuilder.Entity<Book>()
            // 代表接下來的設定是針對 Book 類別。
            //
            // HasData() 用來建立資料庫的初始資料。
            //
            // 執行 Migration 並更新資料庫時，
            // EF Core 會將以下資料加入 Books 資料表。
            modelBuilder.Entity<Book>().HasData(

                // 建立第 1 筆書籍初始資料。
                new Book
                {
                    // 書籍編號，通常也是資料表的主鍵。
                    Id = 1,

                    // 書籍名稱。
                    Title = "Lord of the Rings",

                    // 書籍說明。
                    Description = "Lorem ipsum",

                    // 書籍作者。
                    Author = "J.R.R. Tolkien",

                    // 書籍發行日期。
                    // DateTime 的參數順序為：年、月、日。
                    ReleaseDate = new DateTime(1950, 2, 4),

                    // 書籍圖片檔案名稱。
                    Image = "book_1.png",

                    // 書籍網址使用的簡短名稱。
                    // 例如可能產生網址：/book/lotr
                    Slug = "lotr"
                },

                // 建立第 2 筆書籍初始資料。
                new Book
                {
                    Id = 2,
                    Title = "Harry Potter",
                    Description = "Lorem ipsum",
                    Author = "J.K. Rowling",
                    ReleaseDate = new DateTime(2001, 10, 15),
                    Image = "book_2.png",

                    // 例如可能產生網址：/book/hp
                    Slug = "hp"
                },

                // 建立第 3 筆書籍初始資料。
                new Book
                {
                    Id = 3,
                    Title = "Get Programming with F#",
                    Description = "Lorem ipsum",
                    Author = "Isaac Abraham",
                    ReleaseDate = new DateTime(2016, 8, 23),
                    Image = "book_1.png",

                    // 例如可能產生網址：/book/fsharp
                    Slug = "fsharp"
                },

                // 建立第 4 筆書籍初始資料。
                new Book
                {
                    Id = 4,
                    Title = "Clean Code",
                    Description = "Lorem ipsum",
                    Author = "Robert C. Martin",
                    ReleaseDate = new DateTime(2006, 6, 3),
                    Image = "book_2.png",

                    // 例如可能產生網址：/book/clean-code
                    Slug = "clean-code"
                },

                // 建立第 5 筆書籍初始資料。
                new Book
                {
                    Id = 5,
                    Title = "Are your Lights on?",
                    Description = "Lorem ipsum",
                    Author = "Gerald M. Weinberg",
                    ReleaseDate = new DateTime(1995, 3, 30),
                    Image = "book_1.png",

                    // 例如可能產生網址：/book/lights-on
                    Slug = "lights-on"
                },

                // 建立第 6 筆書籍初始資料。
                new Book
                {
                    Id = 6,
                    Title = "The Only Story",
                    Description = "Lorem ipsum",
                    Author = "Julian Barnes",
                    ReleaseDate = new DateTime(2019, 2, 1),
                    Image = "book_2.png",

                    // 例如可能產生網址：/book/only-story
                    Slug = "only-story"
                }
            );
        }
    }
}