// See https://aka.ms/new-console-template for more information
using Amazon.S3;
using S3StreamUnzip;
using System.Runtime;
using S3StreamUnzip.Helper;



if (args.Length < 4)
{
    Console.WriteLine($"Usage S3StreamUnzip <<inputbucket>>  <<input_zip_object_key>> <<outputbucket>> <<out_put_dir_prefix>> [<<s3_service_url>>]");
    return;
}
Utils.WriteMemoryAndGCDetails();

string acceskey = "minioadmin";
string secretKey = "minioadmin";

string inputBucketName = args[0];
string inputZipObjectKey = args[1];
string outputBucketName = args[2];
string outputPrefix = args[3];
string s3Url = string.Empty;
if (args.Length > 4)
{
    s3Url = args[4];
}


var logger = Utils.GetLogger();

IAmazonS3 s3Client;
if (string.IsNullOrEmpty(s3Url))
{
    // create from .aws profile
    s3Client = Utils.GetS3ClientUsingAwsProfile();
}
else
{
    // using minio
    s3Client = Utils.GetS3ClientForMinio(s3Url, acceskey, secretKey);
}


S3UnzipManager unzipManager = new S3UnzipManager(s3Client, logger);
try
{
    var list = unzipManager.UnzipUsingCSharpziplib(inputBucketName, inputZipObjectKey, outputBucketName, string.Empty).GetAwaiter().GetResult();
    foreach (var item in list)
    {
        Console.WriteLine(item);
    }
}
catch (Exception exp)
{
    Console.WriteLine(exp.ToString());

}

Console.WriteLine("Done");

