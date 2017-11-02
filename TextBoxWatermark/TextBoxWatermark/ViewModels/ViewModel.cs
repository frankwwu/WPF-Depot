using System.ComponentModel.Composition;

namespace TextBoxWatermark.ViewModels
{
    [Export(typeof(ViewModel))]
    public class ViewModel
    {
        [ImportingConstructor]
        public ViewModel()
        {                    
        }
    }
}
