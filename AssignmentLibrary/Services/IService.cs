using AssignmentLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentLibrary.Services
{
    public interface IService<T>
    {
        Task<bool> StartService(Func<List<ObjectData>> func);
        Task<bool> StopService(CancellationToken cancellationToken);
    }

    public interface IFileHandlingService<T>
    {
        Task<List<T>> FetchData();

        Task<bool> ClearData();
    }
}
