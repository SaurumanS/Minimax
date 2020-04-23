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

namespace Minimax
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public bool IsWinnerHere { get; private set; }
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

        public void WinnerIsHere(bool state)
        {
            IsWinnerHere = state;
        }

        private void gridGameField_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock) e.Source;
            int[] coordinates = textBlock.Tag.ToString().Split(' ').Select(curr => int.Parse(curr)).ToArray();
            TicTacToe.MakeMove(coordinates[0], coordinates[1]);
        }

        public void Update()
        {
            GameField = TicTacToe.GetGameField.ToCascade();
        }

        private void buttonStartGame_Click(object sender, RoutedEventArgs e)
        {
            bool isXChecked = (bool) radioButtonX.IsChecked;
            int size =(int) sizeUpDownControl.Value;
            int level = (int)levelUpDownControl.Value;
            TicTacToe = TicTacToeGameBuilder.Create(size, isXChecked, level, this);
            DrawGameField(size);
            DataContext = this;
            TicTacToe.StartGame();
        }
    }
}
