using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace MonitorChart
{
    public class Service
    {
        private DispatcherTimer timer;

        public Service()
        {
            data = new ObservableCollection<Point>();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);
        }

        private ObservableCollection<Point> data;
        public ObservableCollection<Point> Data
        {
            get
            {               
                return data;
            }
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
   
        private void timer_Tick(object sender, EventArgs e)
        {
            // Append new data here...

            Random r = new Random();           
            double y = r.Next(-9, 9);
            DateTime dt = DateTime.Now;
            data.Add(new Point(dt.ToOADate(), y));
        }
    }
}
