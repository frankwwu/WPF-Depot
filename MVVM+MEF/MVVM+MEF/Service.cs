using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_MEF
{   
    [Export(typeof(IService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Service : MVVM_MEF.IService
    {
        [ImportingConstructor]
        public Service()
        {
        }

        public string Data { get { return "Data from the service..."; } }
    }
}
