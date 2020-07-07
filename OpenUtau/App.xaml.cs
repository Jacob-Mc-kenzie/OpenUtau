using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using Serilog;

namespace OpenUtau {
    public enum Skin { Light, Dark }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public static Skin Skin { get; set; } = Skin.Dark;
        private App() {
            InitializeComponent();
            ChangeSkin(Skin.Dark);
        }

        public static void SelectCulture(string culture) {
            if (string.IsNullOrEmpty(culture)) {
                return;
            }
            var dictionaryList = Current.Resources.MergedDictionaries.ToList();
            var resDictName = string.Format(@"UI\Strings.{0}.xaml", culture);
            var resDict = dictionaryList.
                FirstOrDefault(d => d.Source.OriginalString == resDictName);
            if (resDict != null) {
                Current.Resources.MergedDictionaries.Remove(resDict);
                Current.Resources.MergedDictionaries.Add(resDict);
            }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }

        [STAThread]
        private static void Main() {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            NBug.Settings.ReleaseMode = true;
            NBug.Settings.StoragePath = NBug.Enums.StoragePath.CurrentDirectory;
            NBug.Settings.UIMode = NBug.Enums.UIMode.Full;

            Core.DocManager.Inst.SearchAllSingers();
            var pm = new Core.PartManager();

            var app = new App();
            if (!Debugger.IsAttached) {
                AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
                app.DispatcherUnhandledException += NBug.Handler.DispatcherUnhandledException;
            }
            var window = new UI.MainWindow();
            app.Run(window);
        }

        public void ChangeSkin(Skin newSkin) 
        {
            Skin = newSkin;
            Resources.Clear();
            Resources.MergedDictionaries.Clear();
            if (Skin == Skin.Dark) {
                ApplyResources("UI\\Colors\\DarkTheme.xaml");
            }
            else if (Skin == Skin.Light) {
                ApplyResources("UI\\Colors\\LightTheme.xaml");
            }
            ApplyResources("UI\\Styles\\Button.xaml");
            ApplyResources("UI\\Styles\\ScrollBar.xaml");
            ApplyResources("UI\\Styles\\Menu.xaml");
            ApplyResources("UI\\Styles\\BorderlessWindow.xaml");
            ApplyResources("UI\\Styles\\ComboBox.xaml");
            ApplyResources("UI\\Styles\\TrackHeaderStyles.xaml");
            ApplyResources("UI\\Styles\\ProgressBar.xaml");
            ApplyResources("UI\\Strings.zh-CN.xaml");
            ApplyResources("UI\\Strings.ja-JP.xaml");
            ApplyResources("UI\\Strings.xaml");
            SelectCulture(CultureInfo.InstalledUICulture.Name);
            UI.ThemeManager.LoadTheme();
        }
        private void ApplyResources(string src) {
            var dict = new ResourceDictionary() { Source = new Uri(src, UriKind.Relative) };
            foreach (var mergeDict in dict.MergedDictionaries) {
                Resources.MergedDictionaries.Add(mergeDict);
            }

            foreach (var key in dict.Keys) {
                Resources[key] = dict[key];
            }
        }
    }
}
