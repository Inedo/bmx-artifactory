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
    public class DeployArtifactTest
    {
        DeployArtifactAction action = null;
        DeleteItemAction del = null;
        RetrieveArtifactAction ret = null;
        SetItemPropertiesAction set = null;

        [SetUp]
        public void Setup()
        {
            action = new DeployArtifactAction();
            action.TestConfigurer = new ArtifactoryConfigurer();
            var cred = File.ReadAllText(@"c:\temp\art.txt").Split('|');
            action.TestConfigurer.Username = cred[0];
            action.TestConfigurer.Password = cred[1];
            action.TestConfigurer.Server = @"http://artifactory.local:8081/artifactory";
            del = new DeleteItemAction();
            del.TestConfigurer = new ArtifactoryConfigurer();
            del.TestConfigurer.Username = cred[0];
            del.TestConfigurer.Password = cred[1];
            del.TestConfigurer.Server = @"http://artifactory.local:8081/artifactory";
            ret = new RetrieveArtifactAction();
            ret.TestConfigurer = new ArtifactoryConfigurer();
            ret.TestConfigurer.Username = cred[0];
            ret.TestConfigurer.Password = cred[1];
            ret.TestConfigurer.Server = @"http://artifactory.local:8081/artifactory";
            set = new SetItemPropertiesAction();
            set.TestConfigurer = new ArtifactoryConfigurer();
            set.TestConfigurer.Username = cred[0];
            set.TestConfigurer.Password = cred[1];
            set.TestConfigurer.Server = @"http://artifactory.local:8081/artifactory";
        }

        [Test]
        public void TestDeploy()
        {
            action.RepositoryKey = "ext-snapshot-local";
            action.DirectoryName = Guid.NewGuid().ToString();
            action.Properties = "foo=bar";
            action.FileName = @"C:\src\javatest\InedoExample\target\InedoExample-1.0-SNAPSHOT.jar";
            Assert.IsNotEmpty(action.Test());
            del.RepositoryKey = action.RepositoryKey;
            del.ItemName = action.DirectoryName + "/";
            Assert.IsNotEmpty(del.Test());
        }

        [Test]
        public void TestMove()
        {
            action.RepositoryKey = "ext-snapshot-local";
            action.DirectoryName = Guid.NewGuid().ToString();
            action.Properties = "foo=bar";
            action.FileName = @"C:\src\javatest\InedoExample\target\InedoExample-1.0-SNAPSHOT.jar";
            Assert.IsNotEmpty(action.Test());
            MoveItemAction move = new MoveItemAction();
            move.TestConfigurer = action.TestConfigurer;
            move.RepositoryKey = action.RepositoryKey;
            move.SourceItemName = action.DirectoryName + "/" + Path.GetFileName(action.FileName);
            move.DestinationRepository = action.RepositoryKey;
            move.DestinationItemName = "joe/";
            Assert.IsNotEmpty(move.Test());
            del.RepositoryKey = action.RepositoryKey;
            del.ItemName = action.DirectoryName + "/";
            Assert.IsNotEmpty(del.Test());
        }

        [Test]
        public void TestRetrieve()
        {
            action.RepositoryKey = "ext-snapshot-local";
            action.DirectoryName = Guid.NewGuid().ToString();
            action.Properties = "foo=bar";
            action.FileName = @"C:\src\javatest\InedoExample\target\InedoExample-1.0-SNAPSHOT.jar";
            Assert.IsNotEmpty(action.Test());
            string fname = Path.GetTempFileName();
            ret.RepositoryKey = action.RepositoryKey;
            ret.ItemName = action.DirectoryName + "/" + Path.GetFileName(action.FileName);
            ret.FileName = fname;
            Assert.IsNotEmpty(ret.Test());
            
            del.RepositoryKey = action.RepositoryKey;
            del.ItemName = action.DirectoryName + "/";
            Assert.IsNotEmpty(del.Test());
        }

        [Test]
        public void TestSetItemProps()
        {
            action.RepositoryKey = "ext-snapshot-local";
            action.DirectoryName = Guid.NewGuid().ToString();
            action.FileName = @"C:\src\javatest\InedoExample\target\InedoExample-1.0-SNAPSHOT.jar";
            Assert.IsNotEmpty(action.Test());
            set.RepositoryKey = action.RepositoryKey;
            set.ItemName = action.DirectoryName + "/" + Path.GetFileName(action.FileName);
            set.Properties = "foo=bar";
            Assert.IsNotEmpty(set.Test());
            del.RepositoryKey = action.RepositoryKey;
            del.ItemName = action.DirectoryName + "/";
            Assert.IsNotEmpty(del.Test());
        }


    }
}
