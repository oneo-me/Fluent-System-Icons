using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using FluentSystemIcons;
using FluentSystemIconsDemo.Binder;

namespace FluentSystemIconsDemo
{
    public class MainWindow_Model : ViewModel
    {
        public string Version { get; }
        public ICommand CopyIconCommand { get; }

        ObservableCollection<IconInfo> source;

        public ObservableCollection<IconInfo> Source
        {
            get => source;
            set => SetValue(ref source, value);
        }

        ObservableCollection<IconInfo> searchIcons;

        public ObservableCollection<IconInfo> SearchIcons
        {
            get => searchIcons;
            set => SetValue(ref searchIcons, value);
        }

        IconInfo currentIcon;

        public IconInfo CurrentIcon
        {
            get => currentIcon;
            set => SetValue(ref currentIcon, value);
        }

        string searchText = string.Empty;

        public string SearchText
        {
            get => searchText;
            set => SetValue(ref searchText, value, OnSearchTextChanged);
        }

        bool searchState;

        public bool SearchState
        {
            get => searchState;
            set => SetValue(ref searchState, value);
        }

        public MainWindow_Model()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var informationalVersionAttribute = Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyInformationalVersionAttribute)) as AssemblyInformationalVersionAttribute;
                Version = informationalVersionAttribute?.InformationalVersion;
            }

            CopyIconCommand = new Command(obj =>
            {
                if (!(obj is FluentIconKey icon))
                    return;
                var iconStr = $"{icon}";
                Clipboard.SetText(iconStr);
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow == null)
                    return;
                MessageBox.Show(mainWindow, $"Copy \"{iconStr}\" To Clipboard {(Clipboard.GetText() == iconStr ? "Success" : "Fail")}!", nameof(FluentSystemIconsDemo), MessageBoxButton.OK, MessageBoxImage.Information);
            });

            var result = Enum.GetValues(typeof(FluentIconKey)).Cast<FluentIconKey>().ToList();
            result.Sort((a, b) => string.CompareOrdinal($"{a}", $"{b}"));
            Source = new ObservableCollection<IconInfo>(result.Select(x => new IconInfo(x)));

            OnSearchTextChanged();
        }

        void OnSearchTextChanged()
        {
            if (string.IsNullOrWhiteSpace(SearchText) || SearchText.Length < 2)
            {
                SearchIcons = new ObservableCollection<IconInfo>();
                CurrentIcon = Source.FirstOrDefault();
                SearchState = SearchText?.Length > 0;
                return;
            }

            var keyWord = SearchText.ToLower();
            var result = source.Where(x => $"{x.Key}".ToLower().Contains(keyWord));
            SearchIcons = new ObservableCollection<IconInfo>(result);
            CurrentIcon = SearchIcons.FirstOrDefault();
            SearchState = true;
        }
    }
}