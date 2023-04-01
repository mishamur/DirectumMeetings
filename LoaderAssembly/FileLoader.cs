using System.IO;

namespace LoaderAssembly
{
    public class FileLoader : ILoader
    {
        public readonly string filePath;

        public FileLoader(string filePath)
        {
            this.filePath = filePath;
           
        }

        public void Load(string content)
        {
            Directory.CreateDirectory(new FileInfo(filePath).DirectoryName);
            using (StreamWriter writer = File.CreateText(filePath))
            {
                writer.WriteLine(content);
            }
        }
    }
}