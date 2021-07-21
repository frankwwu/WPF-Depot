using System.ComponentModel.Composition;
using System.Xml.Linq;

namespace DataGridColumnBinding
{
    [Export(typeof(IService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Service : IService
    {
        public Service()
        {
        }

        public DataGridModel Refresh()
        {
            XDocument doc = XDocument.Load("DataSet.xml");
            return XmlConverters.ConvertXmlToDataTable(doc);
        }
    }
}
