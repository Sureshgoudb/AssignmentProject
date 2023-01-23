using AssignmentLibrary.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AssignmentLibrary.BusinessLogic
{
    public class JSONReader : IRead<ObjectData>
    {
        public string fileName { get; set; }
        public JSONReader(String fileName) {
        this.fileName = fileName;
        }
        public async Task<bool> ClearInformation()
        {
            bool status = false;
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.Flush();
                    writer.Close();
                    status = true;
                }
            }
            catch
            {
                status = false;
            }

            return status;
        }

        public async Task<List<ObjectData>> ReadInformation()
        {
            List<ObjectData>? source = null;
            try
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    string json = r.ReadToEnd();
                    source = JsonSerializer.Deserialize<List<ObjectData>>(json);
                }
            }
            catch
            {
                source = null;
            }

            return source;
        }
    }
}
