﻿using AcBlog.Data.Models;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using AcBlog.Data.Repositories.FileSystem.Readers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test.Data.Repositories
{
    [TestClass]
    public class FileSystemTest : RepositoriyTest
    {
        string RootPath { get; set; }

        [TestInitialize]
        public void Setup()
        {
            RootPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "test");
            if (Directory.Exists(RootPath))
                Directory.Delete(RootPath, true);
            Directory.CreateDirectory(RootPath);
        }

        [TestCleanup]
        public void Clean()
        {
            if (Directory.Exists(RootPath))
                Directory.Delete(RootPath, true);
        }

        [TestMethod]
        public Task Post()
        {
            return Task.CompletedTask;
            /*var root = Path.Join(RootPath, "posts");
            Directory.CreateDirectory(root);

            var seedPost = new Post[]
            {
                new Post{Title = "a", Id = Guid.NewGuid().ToString()},
            };

            await PostRepositoryBuilder.Build(seedPost.Select(x => new PostBuildData(x)).ToList(), new PostProtector(), new DirectoryInfo(root), 10);

            IPostRepository provider = new PostLocalReader(root);
            
            await PostRepository(provider);*/
        }
    }
}
