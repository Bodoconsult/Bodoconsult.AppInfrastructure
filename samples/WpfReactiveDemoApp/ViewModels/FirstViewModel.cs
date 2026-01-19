using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace WpfReactiveDemoApp.ViewModels
{
    public partial class FirstViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "first";

        //private string _name;
        //public string Test
        //{
        //    get => _name;
        //    set => this.RaiseAndSetIfChanged(ref _name, value);
        //}

        /// <summary>
        /// 
        /// </summary>
        [Reactive] public string Test { get; set; }

        public IScreen HostScreen { get; }

        public FirstViewModel(IScreen screen)
        {
            HostScreen = screen;
            Test = "Blubb";
        }
    }
}
