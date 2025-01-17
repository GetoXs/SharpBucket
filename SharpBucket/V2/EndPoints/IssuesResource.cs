﻿using System;
using System.Collections.Generic;
using System.Threading;
using SharpBucket.V2.Pocos;

namespace SharpBucket.V2.EndPoints
{
    public class IssuesResource : EndPoint
    {
        public IssuesResource(RepositoryResource repositoryResource)
            : base(repositoryResource, "issues")
        {
        }

        /// <summary>
        /// List open issues on the repository.
        /// </summary>
        /// <returns></returns>
        public List<Issue> ListIssues()
            => ListIssues(new ListParameters());

        /// <summary>
        /// List issues on the repository.
        /// </summary>
        /// <param name="parameters">Parameters for the query.</param>
        public List<Issue> ListIssues(ListParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            return GetPaginatedValues<Issue>(BaseUrl, parameters.Max, parameters.ToDictionary());
        }

        /// <summary>
        /// Enumerate open issues on the repository.
        /// </summary>
        public IEnumerable<Issue> EnumerateIssues()
            => EnumerateIssues(new EnumerateParameters());

        /// <summary>
        /// Enumerate issues on the repository.
        /// </summary>
        /// <param name="parameters">Parameters for the queries.</param>
        public IEnumerable<Issue> EnumerateIssues(EnumerateParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            return SharpBucketV2.EnumeratePaginatedValues<Issue>(BaseUrl, parameters.ToDictionary(), parameters.PageLen);
        }

#if CS_8
        /// <summary>
        /// Enumerate issues on the repository asynchronously, doing requests page by page.
        /// </summary>
        /// <param name="token">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public IAsyncEnumerable<Issue> EnumerateIssuesAsync(CancellationToken token = default)
            => EnumerateIssuesAsync(new EnumerateParameters(), token);

        /// <summary>
        /// Enumerate issues on the repository asynchronously, doing requests page by page.
        /// </summary>
        /// <param name="parameters">Parameters for the queries.</param>
        /// <param name="token">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public IAsyncEnumerable<Issue> EnumerateIssuesAsync(EnumerateParameters parameters, CancellationToken token = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            return SharpBucketV2.EnumeratePaginatedValuesAsync<Issue>(BaseUrl, parameters.ToDictionary(), parameters.PageLen, token);
        }
#endif

        #region Issue Resource

        /// <summary>
        /// Get the Issue Resource.
        /// </summary>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns></returns>
        public IssueResource IssueResource(int issueId)
        {
            return new IssueResource(this, issueId);
        }

        #endregion
    }
}
