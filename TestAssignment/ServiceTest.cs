using AssignmentLibrary.Services;
using System.Configuration;
using System.Threading;

namespace TestAssignment
{
    [TestFixture]
    public class ServiceTest
    {
        MonitorService monitorService = null;
        string location = string.Empty;
        public ServiceTest()
        {
      
        }
       [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task StartServiceTest()
        {
            location = @"C:\Users\sures\source\repos\Assignment\Assignment\bin\Debug\net6.0-windows\file.json";
            FileHandlingService filehandlingService = new FileHandlingService(location);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            monitorService = new MonitorService(filehandlingService, cancellationTokenSource.Token);
            var result = await monitorService.StartService(null);
            if (result)
                Assert.Pass();
            else
                Assert.Fail();
        }


        [Test]
        public async Task StopServiceTest()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var result = await monitorService.StopService(cancellationTokenSource.Token);
            if (result)
                Assert.Pass();
            else
                Assert.Fail();
        }
    }
}