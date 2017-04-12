using System.IO;
using System.Linq;

namespace vega.Core.Models
{
    public class PhotoSettings
    {
        public int MaxBytes { get; set; }
        public string[] AcceptedFileTypes { get; set; }

        public bool IsSupported(string fileName) 
        {
          return AcceptedFileTypes.Any(s => s == Path.GetExtension(fileName).ToLower());          
        }        
    }
}