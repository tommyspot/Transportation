using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Amazon;
using Amazon.S3.Model;
using Amazon.S3;
using System.IO;

namespace Epinion.Clarity.IntegrationTests
{
    public class S3FileServiceFixture: IntegrationFixture
    {                
        private string bucketName = "clarity.fileserver";

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            var tenant = Conjurer.Presto.Persist<Tenant>(x=> x.Name = "IntegrationTest");
            ClarityDB.CurrentTenant = tenant;
        }

        [Test]
        public void GetBucketName_ReturnsBucketName(){
            var service = new S3FileService();
            Assert.AreEqual("clarity.fileserver", service.GetBucketName());
        }

        [Test]
        public void GetKeyName_FileNameIsEmpty_ReturnsKeyName()
        {
            var service = new S3FileService();
            Assert.AreEqual("IntegrationTest/MyFile.jpg", service.GetKeyName("MyFile.jpg"));
        }

        [Test]
        public void Exists_FileExists_ReturnsTrue() {
            var fileName = "IntegrationTest/MyFile.txt";

            if (!Exists(fileName))
            {
                UploadFile(fileName);
            }

            var service = new S3FileService();
            Assert.IsTrue(service.Exists("MyFile.txt"));    
        }

        [Test]
        public void Exists_FileDoesNotExist_ReturnsFalse()
        {
            var fileName = "IntegrationTest/NoSuchFile.txt";
            var service = new S3FileService();
            Assert.IsFalse(service.Exists(fileName));    
        }

        [Test]
        public void Upload_UploadsFileToS3() {
            var fileName = "MyFile.txt";

            if (Exists("IntegrationTest/MyFile.txt"))
            {
                Delete("IntegrationTest/MyFile.txt");
            }

            using (var stream = GenerateStreamFromString("My file"))
            {
                var service = new S3FileService();
                service.Upload(stream, fileName);
            }

            Assert.IsTrue(Exists("IntegrationTest/MyFile.txt"));
        }

        [Test]
        public void Delete_FileExistsDeletesFileFromS3()
        {
            var keyName = "IntegrationTest/MyFile.txt";

            if (!Exists(keyName))
            {
                UploadFile(keyName);
            }

            var service = new S3FileService();
            service.Delete("MyFile.txt");

            Assert.IsFalse(Exists(keyName));
        }

        [Test]
        public void Delete_FileDoesNotExist_DoesNotThrowException()
        {
            var keyName = "IntegrationTest/NoSuchFile.txt";

            if (Exists(keyName))
            {
                Delete(keyName);
            }

            var service = new S3FileService();
            Assert.DoesNotThrow(() => service.Delete("NoSuchFile.txt"));
        }

        private Stream GenerateStreamFromString(string s) {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private void UploadFile(string keyName) {
            using (var client = AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.EUWest1))
            {
                PutObjectRequest request = new PutObjectRequest
                {
                    ContentBody = "This is a test file",
                    BucketName = bucketName,
                    Key = keyName,
                    CannedACL = S3CannedACL.PublicRead
                };

                PutObjectResponse response = client.PutObject(request);
            }
        }
        
        private bool Exists(string keyName)
        {
            using (var client = AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.EUWest1))
            {
                try
                {
                    GetObjectMetadataRequest request = new GetObjectMetadataRequest { BucketName = bucketName, Key = keyName };
                    client.GetObjectMetadata(request);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private void Delete(string keyName)
        {
            using (var client = AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.EUWest1))
            {
                DeleteObjectRequest request = new DeleteObjectRequest { BucketName = bucketName, Key = keyName };
                client.DeleteObject(request);
            }
        }
    }
}
