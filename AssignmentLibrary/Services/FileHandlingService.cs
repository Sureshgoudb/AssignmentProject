using AssignmentLibrary.BusinessLogic;
using AssignmentLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentLibrary.Services
{
    public class FileHandlingService : IFileHandlingService<ObjectData>
    {
        public string fileToRead = string.Empty;
        private JSONReader jsonReader = null;
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        public FileHandlingService(string fileToRead)
        {
            this.fileToRead = fileToRead;
            jsonReader = new JSONReader(this.fileToRead);
        }
        public async Task<bool> ClearData()
        {
            bool jsonResult = false;
            try
            {
                _readWriteLock.EnterWriteLock();
                jsonResult = await jsonReader.ClearInformation();
            }
            catch
            {
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
            return jsonResult;
        }

        public async Task<List<ObjectData>> FetchData()
        {
            List<ObjectData> jsonResult = null; ;
            try
            {
                _readWriteLock.EnterWriteLock();
                jsonResult = await jsonReader.ReadInformation();
            }
            catch
            {

            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
            return jsonResult;
        }
    }
}
