using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShiftScheduleDataAccess.Dao.FileDao
{
    internal class FileClient
    {
        public string FolderPath { get; }

        public FileClient(string folderPath)
        {
            FolderPath = folderPath;
        }

        protected IEnumerable<string> GetFiles()
        {
            try
            {
                return Directory.EnumerateFiles(FolderPath);
            }
            catch
            {
                Console.Error.WriteLine("Unable to list files of folder: " + FolderPath);
                throw;
            }
        }

        protected TextReader GetTextReader(string file)
        {
            try
            {
                return new StreamReader(file, Encoding.UTF8);
            }
            catch
            {
                Console.Error.WriteLine("Unable to return stream reader for the file: " + file);
                throw;
            }
        }

        protected TextWriter GetTextWriter(string file)
        {
            try
            {
                return new StreamWriter(file);
            }
            catch
            {
                Console.Error.WriteLine("Unable to return stream writer for the file: " + file);
                throw;
            }
        }
    }
}