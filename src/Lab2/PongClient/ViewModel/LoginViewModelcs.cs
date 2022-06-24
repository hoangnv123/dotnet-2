using PongClient.Service;
using PongClient.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace PongClient.ViewModel
{
    public class LoginViewModelcs: ViewModelBase
    {
        private IPongClientService _pongService = new PongClientService();

        private bool _isConnected;

        public bool IsConnected
        {
            get { return _isConnected; }
            set 
            { 
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        private string? _position;

        public string? Position
        {
            get { return _position; }
            set 
            { 
                _position = value;
                OnPropertyChanged();
            }
        }

        private string? _playerName;

        public string? PlayerName
        {
            get { return _playerName; }
            set 
            { 
                _playerName = value;
                OnPropertyChanged();
            }
        }

        #region Connect Command
        private ICommand? _connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new CommandAsync(Connect));
            }
        }

        private async Task<bool> Connect()
        {
            try
            {
                await _pongService.Connect();
                IsConnected = true;
                return true;
            }
            catch (Exception) { return false; }
        }
        #endregion

        private ICommand? _loginCommand;

        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand =
                    new CommandAsync(Login));
            }
        }
        private async Task<bool> Login()
        {
            try
            {
                if (IsConnected == false)
                {
                    MessageBox.Show("You Need Connect To Server To Start Game!");
                    return false;
                }
                var mainViewModel = new MainViewModel(_pongService);
                if (await _pongService.Login(_playerName!, _position!))
                {
                    MainWindow mainWindow = new MainWindow { DataContext = mainViewModel };
                    if (_position == "Left") mainWindow.LeftPaddle.Name = _position;
                    if (_position == "Right") mainWindow.RightPaddle.Name = _position;
                    mainWindow.Show();
                    Application.Current.MainWindow.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
    }
}
