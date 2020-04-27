using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Minimax.GameLogic;
using Xceed.Wpf.Toolkit;

namespace Minimax
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public bool? IsWinnerHere => TicTacToe.IsWinnerHere;
        private TicTacToeGame TicTacToe { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string[][] gameField;
        public string[][] GameField
        {
            get => gameField;
            set
            {
                gameField = value;
                OnPropertyChanged("GameField");
            }
        }


        private void DrawGameField(int size)
        {
            gridGameField.ColumnDefinitions.Clear();
            gridGameField.RowDefinitions.Clear();
            for(int i = 1; i <= size; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                RowDefinition rowDefinition = new RowDefinition();
                gridGameField.ColumnDefinitions.Add(columnDefinition);
                gridGameField.RowDefinitions.Add(rowDefinition);
            }

            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.TextAlignment = TextAlignment.Center;

                    textBlock.FontSize = 22;
                    textBlock.Tag = $"{row} {column}";
                    Binding binding = new Binding($"GameField[{row}][{column}]");
                    binding.Mode = BindingMode.TwoWay;
                    binding.Source = this;
                    textBlock.SetBinding(TextBlock.TextProperty, binding);
                    Grid.SetRow(textBlock, row);
                    Grid.SetColumn(textBlock, column);
                    gridGameField.Children.Add(textBlock);
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        private void gridGameField_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsWinnerHere != false)
            {
                System.Windows.MessageBox.Show(TicTacToe.InfoAboutWinner(), "Game over", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            TextBlock textBlock = (TextBlock)e.Source;
            int[] coordinates = textBlock.Tag.ToString().Split(' ').Select(curr => int.Parse(curr)).ToArray();
            TicTacToe.MakeMove(coordinates[0], coordinates[1]);

            string infoAboutWinner = TicTacToe.InfoAboutWinner();
            if(!String.IsNullOrEmpty(infoAboutWinner))
            {
                System.Windows.MessageBox.Show(infoAboutWinner, "Game over", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Update()
        {
            GameField = TicTacToe.GetGameField.ToCascade();
        }

        private void buttonStartGame_Click(object sender, RoutedEventArgs e)
        {
            GameField = null;
            bool isXChecked = (bool) radioButtonX.IsChecked;
            int size =(int) sizeUpDownControl.Value;
            int level = (int)levelUpDownControl.Value;
            int playUntil = (int)playUntilUpDownControl.Value;
            TicTacToe = TicTacToeGameBuilder.Create(size, isXChecked, playUntil, level, this);
            DrawGameField(size);
            DataContext = this;
            TicTacToe.StartGame();
        }

        private void UpDownControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            IntegerUpDown integerUpDown = (IntegerUpDown)sender;
            if (integerUpDown != null)
            {
                if (integerUpDown.Value > integerUpDown.Maximum)
                    integerUpDown.Value = integerUpDown.Maximum;
                else if (integerUpDown.Value < integerUpDown.Minimum)
                    integerUpDown.Value = integerUpDown.Minimum;
            }
        }
    }
}
