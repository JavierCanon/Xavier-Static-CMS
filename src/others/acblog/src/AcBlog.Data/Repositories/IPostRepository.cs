﻿using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories
{
    public interface IPostRepository : IRecordRepository<Post, string, PostQueryRequest>
    {
        Task<CategoryTree> GetCategories(CancellationToken cancellationToken = default);

        Task<KeywordCollection> GetKeywords(CancellationToken cancellationToken = default);
    }
}
