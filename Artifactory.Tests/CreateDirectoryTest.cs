using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Inedo.BuildMasterExtensions.Artifactory;

using MbUnit.Framework;

namespace Artifactory.Tests
{
    [TestFixture]
    public class CreateDirectoryTest
    {
        CreateDirectoryAction action = null; 
        [SetUp]
        public void Setup()
        {
            action = new CreateDirectoryAction();
            action.TestConfigurer = new ArtifactoryConfigurer();
            var cred = File.ReadAllText(@"c:\temp\art.txt").Split('|');
            action.TestConfigurer.Username = cred[0];
            action.TestConfigurer.Password = cred[1];
            action.TestConfigurer.Server = @"http://localhost:8081/artifactory";
        }

        [Test]
        public void TestCreate()
        {
            action.RepositoryKey = "ext-snapshot-local";
            action.DirectoryName = "joe";// Guid.NewGuid().ToString();
            Assert.IsNotEmpty(action.Test());
        }

    }
}
