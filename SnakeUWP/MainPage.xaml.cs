using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SnakeUWP
{
    public sealed partial class MainPage : Page
    {
        private int[,] board;
        private List<SnakePoint> player;
        private int dir;
        private SnakePoint head;
        private DispatcherTimer disp;
        private int state;
        private Random rnd;
        private SnakePoint food;

        private void InitGame()
        {
            player = new List<SnakePoint>();
            player.Add(new SnakePoint(5, 2));
            player.Add(new SnakePoint(4, 2));
            player.Add(new SnakePoint(3, 2));
            player.Add(new SnakePoint(2, 2));

            head = player[0];

            dir = 2;

            disp = new DispatcherTimer();
            disp.Tick += Disp_Tick;
            disp.Interval = new TimeSpan(0, 0, 0, 1);
            disp.Start();

            rnd = new Random();

            state = 0;
        }

        private SnakePoint Direction(int dir, List<SnakePoint> points)
        {
            if (dir == 0)
            {
                if (food.X == points[0].X - 1 && food.Y == points[0].Y) Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = "!!!";
                return new SnakePoint(points[0].X - 1, points[0].Y);
            }
            if (dir == 1)
            {
                if (food.X == points[0].X && food.Y == points[0].Y + 1) Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = "!!!";
                return new SnakePoint(points[0].X, points[0].Y + 1);
            }
            if (dir == 2)
            {
                if (food.X == points[0].X + 1 && food.Y == points[0].Y) Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = "!!!";
                return new SnakePoint(points[0].X + 1, points[0].Y);
            }
            if (dir == 3)
            {
                if (food.X == points[0].X && food.Y == points[0].Y - 1) Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = "!!!";
                return new SnakePoint(points[0].X, points[0].Y - 1);
            }
            return new SnakePoint(points[0].X, points[0].Y);
        }

        private void drawBoard()
        {
            gridBoard.Children.Clear();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Rectangle rect = new Rectangle();
                    if (board[i, j] == 0) rect.Fill = new SolidColorBrush(Color.FromArgb(100, 255, 255, 0));
                    if (board[i, j] == 1) rect.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
                    if (board[i, j] == 2) rect.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                    rect.Width = 20;
                    rect.Height = 20;
                    rect.HorizontalAlignment = HorizontalAlignment.Left;
                    rect.VerticalAlignment = VerticalAlignment.Top;
                    rect.Margin = new Thickness(20 * j, 20 * i, 0, 0);
                    gridBoard.Children.Add(rect);
                }
            }

        }

        private struct SnakePoint
        {
            public int X, Y;

            public SnakePoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            InitGame();

            Window.Current.CoreWindow.KeyDown += (s, e) =>
            {
                if (e.VirtualKey == Windows.System.VirtualKey.Left) dir = 0;
                if (e.VirtualKey == Windows.System.VirtualKey.Down) dir = 1;
                if (e.VirtualKey == Windows.System.VirtualKey.Right) dir = 2;
                if (e.VirtualKey == Windows.System.VirtualKey.Up) dir = 3;
            };
        }

        private void Disp_Tick(object sender, object e)
        {

            List<SnakePoint> tmpPoints = player;
            player = new List<SnakePoint>();

            if (food.X == tmpPoints[0].X && food.Y == tmpPoints[0].Y) Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = "!!!";

            player.Add(Direction(dir, tmpPoints));

            if (food.X == player[0].Y && food.Y == player[0].X)
            {
                state = 0;
            }

            for (int i = 0; i < tmpPoints.Count - state; i++)
            {
                player.Add(tmpPoints[i]);
            }

            //if (food.X == player[0].Y && food.Y == player[0].X) Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = "!!!";
            //Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = "food: " + food.X + " " + food.Y + "  plr: " + player[0].X + " " + player[0].Y;

            board = new int[10, 20];
            Array.Clear(board, 0, board.Length);
            foreach (SnakePoint i in player)
            {
                if (i.Y > board.GetLength(0) - 1 || i.Y < 0 || i.X > board.GetLength(1) - 1 || i.X < 0 || board[i.Y, i.X] == 1)
                {
                    disp.Stop();
                    return;
                }
                board[i.Y, i.X] = 1;
            }

            if (state == 0)
            {
                food = new SnakePoint { X = rnd.Next(board.GetLength(0) - 1), Y = rnd.Next(board.GetLength(1) - 1) };
                state = 1;
            }
            board[food.X, food.Y] = 2;
            //textBlock.Text ="\n\n\n\n\n";
            //for (int i = 0; i < board.GetLength(0); i++)
            //{
            //    for (int j = 0; j < board.GetLength(1); j++)
            //    {
            //        textBlock.Text += board[i, j] + " ";
            //    }
            //    textBlock.Text += "\n";
            //}
            drawBoard();
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = player.Count.ToString();
        }
    }
}
