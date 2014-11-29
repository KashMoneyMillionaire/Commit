using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace Infrastructure.Services
{
    public class BackgroundService : IService
    {
        public string UnzipFile(string zipToUnpack)
        {
            var unpackDirectory = Path.Combine(Path.GetDirectoryName(zipToUnpack), Path.GetFileNameWithoutExtension(zipToUnpack));
            using (var zip = ZipFile.Read(zipToUnpack))
            {
                // here, we extract every entry, but we could extract conditionally
                // based on entry name, size, date, checkbox status, etc.  \
                zip.ExtractAll(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
            }
            
            return unpackDirectory;
        }
    }
}
