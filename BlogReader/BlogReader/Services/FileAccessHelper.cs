using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace BlogReader.Services
{
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            return System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);
        }
    }
}
