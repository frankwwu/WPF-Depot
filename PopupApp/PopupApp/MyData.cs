namespace PopupApp
{
    public class MyData : ViewModelBase
    {
        private string name = string.Empty;
        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
    }
}
