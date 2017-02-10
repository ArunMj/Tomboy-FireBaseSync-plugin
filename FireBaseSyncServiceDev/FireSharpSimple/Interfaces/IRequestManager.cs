using System;
using System.Net.Http;
using System.Threading.Tasks;
using FireSharpSimple.Response;

namespace FireSharpSimple.Interfaces
{
    internal interface IRequestManager : IDisposable
    {
        Task<HttpResponseMessage> ListenAsync(string path);
        Task<HttpResponseMessage> ListenAsync(string path, QueryBuilder queryBuilder);
        Task<HttpResponseMessage> RequestAsync(HttpMethod method, string path, object payload = null);
        Task<HttpResponseMessage> RequestAsync(HttpMethod method, string path, QueryBuilder queryBuilder, object payload = null);
        Task<HttpResponseMessage> RequestApiAsync(HttpMethod method, string path, QueryBuilder queryBuilder, object payload = null);
    }
}