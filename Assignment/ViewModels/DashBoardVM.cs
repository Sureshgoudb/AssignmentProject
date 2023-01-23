using Assignment.Commands;
using AssignmentLibrary.BusinessLogic;
using AssignmentLibrary.Model;
using AssignmentLibrary.Services;
using Microsoft.Win32;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment.ViewModels
{
    public class DashBoardVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Button Click Command
        /// </summary>
        public BaseCommand ClearCommand
        {
            get;
            set;

        }

        public BaseCommand CloseCommand
        {
            get;
            set;

        }


        //lst to bind 
        private ObservableCollection<ObjectData> _objectData;
        public ObservableCollection<ObjectData> objectData
        {
            get
            {
                return _objectData;
            }
            set
            {
                _objectData = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("objectData"));
                }
            }
        }
        private string fileLocation = string.Empty;


        private FileHandlingService filehandlingService = null;

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private MonitorService monitorService = null;
        public DashBoardVM()
        {
            fileLocation = Config();
            _objectData = new ObservableCollection<ObjectData>();
            filehandlingService = new FileHandlingService(fileLocation);

            ClearCommand = new BaseCommand(o => ClearButtonClick("ClearCommand"), o => true);
            CloseCommand = new BaseCommand(o => WindowClose("N/A"),o=> true);
            updateList();
        }

        private async Task ClearButtonClick(object sender)
        {
            try
            {
                objectData.Clear();
                await filehandlingService.ClearData();
            }
            catch
            {

            }
        }

        private async Task WindowClose(object sender)
        {
            try
            {
              if(  monitorService != null)
                {
                   await monitorService.StopService(cancellationTokenSource.Token);
                }
            }
            catch
            {

            }
        }

        private string Config()
        {
            string location = string.Empty;
            try
            {
                location = Convert.ToString(ConfigurationManager.AppSettings["FileInformation"]);
            }
            catch (Exception ex)
            {
                location = string.Empty;
            }
            return location;
        }
        private async void updateList()
        {
            try
            {
                monitorService = new MonitorService(filehandlingService, cancellationTokenSource.Token);
                await Task.Factory.StartNew(() => monitorService.StartService(() =>
                {
                    try
                    {
                        objectData = new ObservableCollection<ObjectData>(monitorService.objectData);
                    }
                    catch
                    {

                    }
                    return null;
                }));
                
            }
            catch
            {
                _objectData = new ObservableCollection<ObjectData>();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
