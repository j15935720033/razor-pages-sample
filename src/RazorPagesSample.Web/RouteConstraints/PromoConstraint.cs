using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace RazorPagesSample.Web.RouteConstraints
{
    /// <summary>
    /// 自訂的路由限制條件。
    ///
    /// 用來檢查網址中的優惠代碼（Promo Code）
    /// 是否為系統允許使用的優惠代碼。
    /// </summary>
    public class PromoConstraint : IRouteConstraint
    {
        /// <summary>
        /// 儲存系統允許使用的優惠代碼集合。
        /// </summary>
        private readonly IEnumerable<string> _promoCodes;

        /// <summary>
        /// 建構子。
        ///
        /// 建立 PromoConstraint 物件時，
        /// 初始化允許使用的優惠代碼。
        /// </summary>
        public PromoConstraint()
        {
            // 建立允許使用的優惠代碼清單。
            _promoCodes = new List<string>
            {
                "code1",
                "code3"
            };
        }

        /// <summary>
        /// 判斷目前的路由值是否符合限制條件。
        /// </summary>
        /// <param name="httpContext">
        /// 目前 HTTP 請求的相關資訊，
        /// 例如 Request、Response、User 等。
        /// </param>
        /// <param name="route">
        /// 目前正在進行比對的路由物件。
        /// </param>
        /// <param name="routeKey">
        /// 目前進行比對的路由參數名稱。
        /// </param>
        /// <param name="values">
        /// 網址中解析出來的路由參數集合。
        ///
        /// 例如網址為：
        /// /Promo/code1
        ///
        /// values 可能會包含：
        /// code = code1
        /// </param>
        /// <param name="routeDirection">
        /// 表示目前是在處理傳入網址，
        /// 還是在產生網址。
        /// </param>
        /// <returns>
        /// 如果優惠代碼存在於允許的清單中，回傳 true；
        /// 否則回傳 false。
        /// </returns>
        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            // 從路由參數 values 中取得名稱為 code 的值。
            //
            // 例如網址為：
            // /Promo/code1
            //
            // 則 values["code"] 的值可能是 code1。
            //
            // ?. 表示如果 values["code"] 是 null，
            // 就不執行 ToString()，並直接回傳 null。
            string promoCode = values["code"]?.ToString();

            // 使用 Contains() 檢查優惠代碼是否存在於
            // _promoCodes 集合中。
            //
            // promoCode 為 code1 或 code3 時，回傳 true。
            // 其他代碼則回傳 false。
            return _promoCodes.Contains(promoCode);
        }
    }
}