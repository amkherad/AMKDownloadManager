using System.Collections.Generic;
using System.IO;
using System.Net;
using AMKDownloadManager.Core.Api.Barriers;

namespace AMKDownloadManager.Core.Api
{
    public static class HttpHelpers
    {
        public static HttpWebRequest CreateHttpWebRequestFromRequest(IRequest request)
        {
            var req = WebRequest.CreateHttp(request.Uri);

            req.Method = request.Method;

            foreach (KeyValuePair<string, string> header in request.Headers)
            {
                req.Headers[header.Key] = header.Value;
            }
            
            var accept = request.Headers.Accept;
            if (accept != null) req.Accept = accept;

            var contentType = request.Headers.ContentType;
            if (contentType != null) req.ContentType = contentType;

            //foreach (var cookie in request.Cookies)
            //{
            //    req.CookieContainer.Add(cookie.Uri ?? request.Uri, new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
            //}
            
            return req;
        }

        public static void FillResponseFromHttpResponse(HttpResponse res, HttpWebResponse response, Stream stream = null)
        {
            res.StatusCode = response.StatusCode;
            
            if (stream != null)
            {
                res.ResponseStream = stream;
            }
            
            foreach (string header in response.Headers)
            {
                res.Headers.Add(header, response.Headers[header]);                
            }
        }
    }
}