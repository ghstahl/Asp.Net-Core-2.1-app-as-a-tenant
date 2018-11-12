using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Tenant.Core.Shims
{
    public class TenantHttpRequestMessageFeature 
    {
        private HttpRequestMessage _httpRequestMessage;
        private HttpRequest _httpRequest;

        public TenantHttpRequestMessageFeature(HttpRequest httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));
            this._httpRequest = httpRequest;
        }

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                if (this._httpRequestMessage == null)
                    this._httpRequestMessage = TenantHttpRequestMessageFeature.CreateHttpRequestMessage(
                        this._httpRequest);
                return this._httpRequestMessage;
            }
            set
            {
                this._httpRequestMessage = value;
            }
        }

        private static HttpRequestMessage CreateHttpRequestMessage(HttpRequest request)
        {
            string requestUri = request.Scheme + "://" + (object)request.Host + request.PathBase + request.Path + (object)request.QueryString;
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod(request.Method), requestUri);
            httpRequestMessage.Content = (HttpContent)new StreamContent(request.Body);
            foreach (KeyValuePair<string, StringValues> header in (IEnumerable<KeyValuePair<string, StringValues>>)request.Headers)
            {
                if (!httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, (IEnumerable<string>)header.Value))
                    httpRequestMessage.Content.Headers.TryAddWithoutValidation(header.Key, (IEnumerable<string>)header.Value);
            }
            return httpRequestMessage;
        }
    }
}
