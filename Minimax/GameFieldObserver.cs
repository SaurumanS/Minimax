using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Windows;

namespace Minimax
{
    class GameFieldObserver : IObserver
    {
        private MainWindow ObservableWindow { get; }
        public GameFieldObserver(MainWindow window)
        {
            ObservableWindow = window;
        }
        public void Update()
        {
            ObservableWindow.Update();
        }
    }
}
