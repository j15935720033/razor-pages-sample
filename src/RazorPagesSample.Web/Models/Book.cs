using System;

// 匯入資料驗證功能。
// 可使用 [Required]、[MinLength]、[MaxLength] 等驗證屬性。
using System.ComponentModel.DataAnnotations;

namespace RazorPagesSample.Web.Models
{
    /// <summary>
    /// 書籍資料模型。
    /// 用來保存一本書的基本資料。
    /// </summary>
    public class Book
    {
        /// <summary>
        /// 書籍編號。
        /// 通常會作為資料庫資料表的主鍵。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 書籍名稱。
        /// </summary>

        // Required 表示此欄位為必填。
        // 如果使用者沒有輸入，資料驗證就不會通過。
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 書籍網址代稱。
        /// 例如：網址是
        /// /book/lotr
        /// Slug就等於lotr
        /// </summary>

        // 此欄位為必填。
        [Required]

        // 最少必須輸入 3 個字元。
        [MinLength(3)]

        // 最多只能輸入 10 個字元。
        [MaxLength(10)]
        public string Slug { get; set; }

        /// <summary>
        /// 書籍作者。
        /// </summary>

        // 此欄位為必填。
        [Required]
        public string Author { get; set; }

        /// <summary>
        /// 書籍內容說明。
        /// 沒有加上 [Required]，因此不是必填欄位。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 書籍出版日期。
        /// DateTime 用來儲存日期與時間。
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// 書籍圖片路徑或圖片檔案名稱。
        /// 例如：
        /// images/book1.jpg
        /// </summary>
        public string Image { get; set; }
    }
}