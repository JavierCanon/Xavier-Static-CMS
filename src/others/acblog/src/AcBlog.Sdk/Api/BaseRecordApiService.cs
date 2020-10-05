﻿using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AcBlog.Sdk.Api
{
    internal abstract class BaseRecordApiService<T, TQuery> where T : class, IHasId<string>
    {
        protected abstract string PrepUrl { get; }

        public BaseRecordApiService(IBlogService blog, HttpClient httpClient)
        {
            BlogService = blog;
            HttpClient = httpClient;
        }

        public IBlogService BlogService { get; private set; }

        public HttpClient HttpClient { get; }

        public RepositoryAccessContext Context { get; set; } = new RepositoryAccessContext();

        protected virtual void SetHeader()
        {
            if (Context != null && !string.IsNullOrWhiteSpace(Context.Token))
            {
                HttpClient.DefaultRequestHeaders.Remove("Authorization");
                HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Context.Token);
            }
        }

        public virtual async Task<IEnumerable<string>> All(CancellationToken cancellationToken = default)
        {
            SetHeader();
            using var responseMessage = await HttpClient.GetAsync(PrepUrl, cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<IEnumerable<string>>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default)
        {
            SetHeader();
            using var responseMessage = await HttpClient.GetAsync(PrepUrl, cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<RepositoryStatus>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<string?> Create(T value, CancellationToken cancellationToken = default)
        {
            SetHeader();

            using var responseMessage = await HttpClient.PostAsJsonAsync(PrepUrl, value, cancellationToken).ConfigureAwait(false);

            if (!responseMessage.IsSuccessStatusCode)
                return null;

            return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public virtual async Task<bool> Delete(string id, CancellationToken cancellationToken = default)
        {
            SetHeader();

            using var responseMessage = await HttpClient.DeleteAsync($"{PrepUrl}/{HttpUtility.UrlEncode(id)}", cancellationToken).ConfigureAwait(false);

            if (!responseMessage.IsSuccessStatusCode)
                return false;

            return await responseMessage.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            SetHeader();

            using var responseMessage = await HttpClient.GetAsync($"{PrepUrl}/{HttpUtility.UrlEncode(id)}", cancellationToken).ConfigureAwait(false);
            return responseMessage.IsSuccessStatusCode;
        }

        public virtual async Task<T?> Get(string id, CancellationToken cancellationToken = default)
        {
            SetHeader();

            using var responseMessage = await HttpClient.GetAsync($"{PrepUrl}/{HttpUtility.UrlEncode(id)}", cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();

            var result = await responseMessage.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken).ConfigureAwait(false);
            if (result != null)
                result.Id = id;

            return result;
        }

        public virtual async Task<QueryResponse<string>> Query(TQuery query, CancellationToken cancellationToken = default)
        {
            SetHeader();

            using var responseMessage = await HttpClient.PutAsJsonAsync($"{PrepUrl}/query", query, cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<QueryResponse<string>>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<bool> Update(T value, CancellationToken cancellationToken = default)
        {
            SetHeader();

            using var responseMessage = await HttpClient.PutAsJsonAsync($"{PrepUrl}/{HttpUtility.UrlEncode(value.Id)}", value, cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}
