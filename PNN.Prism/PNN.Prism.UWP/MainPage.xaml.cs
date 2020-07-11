using Prism;
using Prism.Ioc;

namespace PNN.Prism.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            LoadApplication(new PNN.Prism.App(new UwpInitializer()));
        }
    }

    public class UwpInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}
