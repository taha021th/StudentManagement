//using Microsoft.AspNetCore.Localization;
//using System.Globalization;

//namespace StudentManagement.Middleware
//{
//    public class LocalizationMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public LocalizationMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

       
//        public async Task InvokeAsync(HttpContext context)
//        {
//            // دریافت Accept-Language از Headers
//            if (context.Request.Headers.TryGetValue("Accept-Language", out var userLanguage))
//            {
//                var language = userLanguage.ToString();

//                if (!string.IsNullOrEmpty(language))
//                {
//                    var culture = new CultureInfo(language);
//                    CultureInfo.CurrentCulture = culture;
//                    CultureInfo.CurrentUICulture = culture;
//                }
//            }

//            // ادامه به Middleware بعدی
//            await _next(context);
//        }
//    }
//}
