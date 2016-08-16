using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace MVVM_MEF
{
    [Export(typeof(ViewModel))]
    public class ViewModel
    {
        private readonly IService _service;

        [ImportingConstructor]
        public ViewModel(IService service)
        {
            _service = service;
            DataItems = service.DataItems;
        }

        public string Title { get { return "MVVM + MEF"; } }

        public List<Model> DataItems { get; private set; }
    }
}
