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
        }

        public string Title { get { return "MVVM + MEF"; } }

        public string Data { get { return _service.Data; } }
    }
}
