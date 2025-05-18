using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;
using Windows.Graphics;

namespace CheatBuster
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetWindowSize(1000, 500);
            SetAcrylicFooter();
            DisableResize();
        }

        private void SetWindowSize(int width, int height)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow?.Resize(new SizeInt32(width, height));    
        }

        private void SetAcrylicFooter()
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            var titleBar = appWindow.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        private void DisableResize()
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            if (appWindow != null && appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = false;
            }   
        }

        private async void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            string inputText = IdTextBox.Text;

            if (string.IsNullOrWhiteSpace(inputText))
            {
                AppendToConsole("������: ���� �� ����� ���� ������!"); 
                ContentDialog noInputDialog = new()
                {
                    Title = "������ �����",
                    Content = "����������, ������� ���� �����.",
                    CloseButtonText = "��",
                    XamlRoot = this.Content.XamlRoot
                };
                await noInputDialog.ShowAsync();
                return;
            }

            if (!inputText.All(char.IsDigit))
            {
                AppendToConsole("������: ���� ������ �������� ������ �� ����!"); 
                ContentDialog invalidInputDialog = new()
                {
                    Title = "������ �����",
                    Content = "���� ����� ������ �������� ������ �� ����.",
                    CloseButtonText = "��",
                    XamlRoot = this.Content.XamlRoot
                };
                await invalidInputDialog.ShowAsync();
                return;
            }

            AppendToConsole($"������ ���������� ID: {inputText}"); 
            string windowsUserName = System.Environment.UserName;
            AppendToConsole($"��� ������������: {windowsUserName}"); 


            LoadingBar.IsIndeterminate = true;
            LoadingBar.ShowPaused = false;
            LoadingBar.ShowError = false;

            AppendToConsole("�������� ��������..."); 
        }

        public void AppendToConsole(string message)
        {
            if (Terminal != null && Terminal.Document != null)
            {
                Terminal.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out string currentText);

                string textToAppend = string.IsNullOrEmpty(currentText) ? message : "\n" + message;

                Terminal.Document.Selection.SetRange(int.MaxValue, int.MaxValue); 
                Terminal.Document.Selection.TypeText(textToAppend);

                var grid = VisualTreeHelper.GetChild(Terminal, 0) as Grid;
                if (grid != null)
                {
                    for (var i = 0; i < VisualTreeHelper.GetChildrenCount(grid); i++)
                    {
                        var child = VisualTreeHelper.GetChild(grid, i);
                        if (child is ScrollViewer scrollViewer)
                        {
                            scrollViewer.ChangeView(null, scrollViewer.ScrollableHeight, null, false); 
                            break;
                        }
                    }
                }
            }
        }
    }
}
