using System.ComponentModel.Composition.Hosting;
using System.Windows;

namespace DataGridColumnBinding
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected CompositionContainer Container { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()));
            Container = new CompositionContainer(catalog);
            this.Container.GetExportedValue<MainWindow>().Show();
        }
    }
}
