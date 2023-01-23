using AssignmentLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentLibrary.Services
{
    public class MonitorService : IService<ObjectData>
    {
        private string fileToMonitor = string.Empty;
        private DateTime lastModified = DateTime.MinValue;
        private PeriodicTimer periodicTimer = null;
        private CancellationToken cancellationToken = default(CancellationToken);
        private FileHandlingService fileHandlingService = null;
        public List<ObjectData> objectData { get; set; }
        public MonitorService(FileHandlingService fileHandlingService, CancellationToken cancellationToken)
        {
            this.fileToMonitor = fileHandlingService.fileToRead;
            objectData = new List<ObjectData>();
            this.fileHandlingService = new FileHandlingService(fileToMonitor);
            this.cancellationToken = cancellationToken;
        }
        public async Task<bool> StartService(Func<List<ObjectData>> func)
        {
            bool status = false;
            try
            {
                periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));

                while (await periodicTimer.WaitForNextTickAsync(cancellationToken))
                {
                    var Modified = System.IO.File.GetLastWriteTime(fileToMonitor);
                    if (Modified > lastModified)
                    {
                        objectData = await fileHandlingService.FetchData();
                        lastModified = Modified;
                        status = true;
                        func();
                    }
                }
            }
            catch { }
            return status;
        }

        public async Task<bool> StopService(CancellationToken cancellationToken)
        {
            bool status = false;
            try
            {
                if (periodicTimer != null)
                {
                    this.cancellationToken = cancellationToken;
                    periodicTimer.Dispose();
                    status = true;
                }
            }
            catch { }
            return status;
        }
    }
}
