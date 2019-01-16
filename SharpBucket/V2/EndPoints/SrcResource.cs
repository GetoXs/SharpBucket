﻿using System;
using System.Collections.Generic;
using System.Text;
using SharpBucket.Utility;
using SharpBucket.V2.Pocos;

namespace SharpBucket.V2.EndPoints
{
    /// <summary>
    /// https://developer.atlassian.com/bitbucket/api/2/reference/resource/repositories/%7Busername%7D/%7Brepo_slug%7D/src#get
    /// </summary>
    public class SrcResource
    {
        private RepositoriesEndPoint RepositoriesEndPoint { get; }
        private string SrcPath { get; }

        public SrcResource(RepositoriesEndPoint repositoriesEndPoint, string accountName, string repoSlugOrName, string revision = null, string path = null)
        {
            RepositoriesEndPoint = repositoriesEndPoint ?? throw new ArgumentNullException(nameof(repositoriesEndPoint));

            var rootSrcPath = $"{accountName.GuidOrValue()}/{repoSlugOrName.ToSlug()}/src/";
            if (string.IsNullOrEmpty(revision))
            {
                // This may potentially be optimized since this call will first hit a redirect toward an url that contains the revision
                // but the actual architecture of the code doesn't allow us to fetch just the redirect location of a GET request.
                // so we found back the data we need in the response of the call to the url where we are redirected.
                var rootEntry = repositoriesEndPoint.GetTreeEntry(rootSrcPath);
                revision = rootEntry.Commit.hash;
            }

            SrcPath = UrlHelper.ConcatPathSegments(rootSrcPath, revision, path).EnsureEndsWith('/');
        }

        private SrcResource(RepositoriesEndPoint repositoriesEndPoint, string srcPath, string subDirPath)
        {
            RepositoriesEndPoint = repositoriesEndPoint;
            SrcPath = UrlHelper.ConcatPathSegments(srcPath, subDirPath).EnsureEndsWith('/');
        }

        /// <summary>
        /// List the files and directories that are present at the root of this resource, or in the specified sub directory.
        /// </summary>
        /// <param name="subDirPath">The path to a sub directory, or null to list the root directory of this resource.</param>
        /// <param name="max">The maximum number of entries to return, or 0 for unlimited size.</param>
        public List<TreeEntry> ListTreeEntries(string subDirPath = null, int max = 0)
        {
            return RepositoriesEndPoint.ListTreeEntries(SrcPath, subDirPath, max);
        }

        /// <summary>
        /// Gets the metadata of a specified file or directory in this resource.
        /// </summary>
        /// <param name="subPath">The path to the file or directory, or null to retrieve the metadata of the root of this resource.</param>
        public TreeEntry GetTreeEntry(string subPath = null)
        {
            return RepositoriesEndPoint.GetTreeEntry(SrcPath, subPath);
        }

        /// <summary>
        /// Gets a new <see cref="SrcResource"/> which is rooted on a sub directory of the current <see cref="SrcResource"/>.
        /// </summary>
        /// <param name="subDirPath">The path to a sub directory.</param>
        public SrcResource SubSrcResource(string subDirPath)
        {
            if (string.IsNullOrWhiteSpace(subDirPath)) throw new ArgumentNullException(nameof(subDirPath));
            return new SrcResource(RepositoriesEndPoint, SrcPath, subDirPath);
        }

        /// <summary>
        /// Gets the raw content of the specified file.
        /// </summary>
        /// <param name="filePath">The path to a file relative to the root of this resource.</param>
        internal string GetFileContent(string filePath)
        {
            return RepositoriesEndPoint.GetFileContent(SrcPath, filePath);
        }
    }
}
