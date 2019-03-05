using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IceCreamMonitorMVP.Model;
using System.Windows.Media;
using IceCreamMonitorMVP.DAL;

namespace IceCreamMonitorMVP
{
    public class MainPresenter
    {
        private IceCreamMonitor iceCreamMonitor;
        private IMainWiew view;

        public MainPresenter(IMainWiew view, IceCreamMonitor iceCreamMonitor)
        {
            this.view = view;
            this.iceCreamMonitor = iceCreamMonitor;
            this.iceCreamMonitor.newStationId += iceCreamMonitor_newStationId;
            InitializeView();
        }

        void iceCreamMonitor_newStationId(object sender, EventArgs e)
        {
            view.InitMeasurements(iceCreamMonitor.GetStationIds());
        }

        internal void InitializeView()
        {
            view.SetTarget(iceCreamMonitor.Target.ToString());
            view.InitMeasurements(iceCreamMonitor.GetStationIds());
        }

        // TODO
        // Implement the missing parts of the presenter
        public void ChangeStation(string stationId)
        {
            foreach (Measurement measurement in iceCreamMonitor.GetMeasurements())
            {
                if (measurement.StationId.Equals(stationId))
                {
                    view.SetStation(stationId);
                    view.SetDate(measurement.Date);
                    view.SetActual(measurement.Actual.ToString());
                    VarianceRange varianceRange = VarianceRange.normal;

                    view.SetVarianceColor(Color.FromRgb(200, 0, 0));
                    view.SetVariance(iceCreamMonitor.CalculateVariance(measurement.Actual, out varianceRange).ToString());
                }
            }
        }

        public void NewMeasurement(string stationId, DateTime selectedDateValue, string tbxActualValueText)
        {
            if(int.TryParse(tbxActualValueText, out var actual))
            {
                    iceCreamMonitor.AddMeasurement(stationId, selectedDateValue, actual);
                    view.InitMeasurements(iceCreamMonitor.GetStationIds());
            }
            else
            {
                MessageBoxResult messageBoxResult =
                    MessageBox.Show(
                        "You did not insert integer as actual value", "Failed",
                        MessageBoxButton.OK);
            }
            
        }

        public bool Closing()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                "Are you sure you want to close the application?", 
                "Closing Confirmation", 
                MessageBoxButton.YesNo);
            return messageBoxResult != MessageBoxResult.Yes;
        }

        public void ActualChanged(string tbxActualText)
        {
            if (int.TryParse(tbxActualText, out var actual))
            {
                VarianceRange varianceRange = VarianceRange.normal;

                view.SetVarianceColor(Color.FromRgb(200, 0, 0));
                view.SetVariance(iceCreamMonitor.CalculateVariance(actual, out varianceRange).ToString());
            }
            else
            {
                MessageBoxResult messageBoxResult =
                    MessageBox.Show(
                        "You did not insert integer as actual value", "Failed",
                        MessageBoxButton.OK);
            }
        }

        public void DateChanged(string text, DateTime? dpDateSelectedDate)
        {
            
        }
    }
}
