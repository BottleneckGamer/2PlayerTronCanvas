using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;
using System.Media;

namespace _2PlayerTronCanvasViaSnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Point> trailPoints1 = new List<Point>();
        List<Point> trailPoints2 = new List<Point>();

        private enum MOVINGDIRECTION
        {
            UPWARDS = 8,
            DOWNWARDS = 2,
            TOLEFT = 4,
            TORIGHT = 6,
            Up = 1,
            Down = 3,
            Left = 5,
            Right = 7
        };


        private bool active = false;
        private Random rnd = new Random();
        private Point P1headPosition = new Point(100, 100);
        private Point P2headPosition = new Point(100, 100);
        private int P1direction = 0;
        private int P2direction = 0;
        private int P1previousDirection = 0;
        private int P2previousDirection = 0;
        private int P1score = 0;
        private int P2score = 0;

        public MainWindow()
        {
            InitializeComponent();
            P1headPosition = new Point(rnd.Next(5, 745), rnd.Next(5, 745));
            trailPoints1.Add(P1headPosition);
            P2headPosition = new Point(rnd.Next(5, 745), rnd.Next(5, 745));
            trailPoints2.Add(P2headPosition);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(p1p2timer_Tick);           
            timer.Interval = TimeSpan.FromMilliseconds(10);
            SoundPlayer player = new SoundPlayer(Properties.Resources.Daft_Punk__Derezzed);            
            player.Play();
            timer.Start();

            P1painthead();
            P2painthead();
            KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        private void p1p2timer_Tick(object sender, EventArgs e)
        {
            p1score.Text = "P1 Score: " + P1score;
            p2score.Text = "P2 Score: " + P2score;
            collisionCheckforP1P2();

            //P1Bounds
            if (P1headPosition.X >= 5 && P1headPosition.X <= 745 && P1headPosition.Y >= 5 && P1headPosition.Y <= 745 && active == true)
            {
                switch (P1direction)
                {
                    case (int)MOVINGDIRECTION.DOWNWARDS:
                        P1headPosition.Y += 5;
                        P1painthead();
                        break;
                    case (int)MOVINGDIRECTION.UPWARDS:
                        P1headPosition.Y -= 5;
                        P1painthead();
                        break;
                    case (int)MOVINGDIRECTION.TOLEFT:
                        P1headPosition.X -= 5;
                        P1painthead();
                        break;
                    case (int)MOVINGDIRECTION.TORIGHT:
                        P1headPosition.X += 5;
                        P1painthead();
                        break;
                }
            }
            else if (active == true)
            {
                gameOver();
                MessageBox.Show("P1 lose! Out of Bounds!");
                P2score += 1;
                p1score.Text = "P1 Score: " + P1score;
                p2score.Text = "P2 Score: " + P2score;
            }

            //P2 Bounds
            if (P2headPosition.X >= 5 && P2headPosition.X <= 745 && P2headPosition.Y >= 5 && P2headPosition.Y <= 745 && active == true)
            {
                switch (P2direction)
                {
                    case (int)MOVINGDIRECTION.Down:
                        P2headPosition.Y += 5;
                        P2painthead();
                        break;
                    case (int)MOVINGDIRECTION.Up:
                        P2headPosition.Y -= 5;
                        P2painthead();
                        break;
                    case (int)MOVINGDIRECTION.Left:
                        P2headPosition.X -= 5;
                        P2painthead();
                        break;
                    case (int)MOVINGDIRECTION.Right:
                        P2headPosition.X += 5;
                        P2painthead();
                        break;
                }
            }
            else if (active == true)
            {
                gameOver();
                MessageBox.Show("P2 lose! Out of Bounds!");
                P1score += 1;
            p1score.Text = "P1 Score: " + P1score;
            p2score.Text = "P2 Score: " + P2score;
                
            }
            
        }

        

        private void collisionCheckforP1P2()
        {
            //p1withself
            for (int i = 0; i < trailPoints1.Count - 10; i++)
            {
                Point point = new Point(trailPoints1[i].X, trailPoints1[i].Y);
                if ((Math.Abs(point.X - P1headPosition.X) < (10)) &&
                     (Math.Abs(point.Y - P1headPosition.Y) < (10)))
                {
                    gameOver();
                    MessageBox.Show("P1 lose! You hit Yourself!");
                    P2score += 1;
                    p1score.Text = "P1 Score: " + P1score;
                    p2score.Text = "P2 Score: " + P2score;
                    break;
                }
            }

            //p2withself
            for (int i = 0; i < trailPoints2.Count - 10; i++)
            {
                Point point = new Point(trailPoints2[i].X, trailPoints2[i].Y);
                if ((Math.Abs(point.X - P2headPosition.X) < (15)) &&
                     (Math.Abs(point.Y - P2headPosition.Y) < (15)))
                {
                    gameOver();
                    MessageBox.Show("P2 lose! You Hit Yourself!");
                    P1score += 1;
                    p1score.Text = "P1 Score: " + P1score;
                    p2score.Text = "P2 Score: " + P2score;
                    break;
                }
            }

            //p1withp2
            for (int i = 0; i < trailPoints1.Count - 10; i++)
            {
                Point point = new Point(trailPoints2[i].X, trailPoints2[i].Y);
                if ((Math.Abs(point.X - P1headPosition.X) < (15)) &&
                    (Math.Abs(point.Y - P1headPosition.Y) < (15)))
                {
                    gameOver();
                    MessageBox.Show("P1 lose! You Hit P2!");
                    P2score += 1;
                    p1score.Text = "P1 Score: " + P1score;
                    p2score.Text = "P2 Score: " + P2score;
                    break;
                }               
            }
            
            //p2withp1
            for (int i = 0; i < trailPoints2.Count - 10; i++)
            {
                Point point = new Point(trailPoints1[i].X, trailPoints1[i].Y);
                if ((Math.Abs(point.X - P2headPosition.X) < (15)) &&
                    (Math.Abs(point.Y - P2headPosition.Y) < (15)))
                {
                    gameOver();
                    MessageBox.Show("P2 lose! You hit P1!");
                    P1score ++;
                    p1score.Text = "P1 Score: " + P1score;
                    p2score.Text = "P2 Score: " + P2score;
                    break;
                }               
            }
        }

        private void gameOver()
        {
            
            paintCanvas.Children.Clear();
            active = false;
            trailPoints1.Clear();
            trailPoints2.Clear();
            
            P1headPosition = new Point(rnd.Next(5, 745), rnd.Next(5, 745));
            P2headPosition = new Point(rnd.Next(5, 745), rnd.Next(5, 745));
            P1painthead();
            P2painthead();
        }

        private void P1painthead()
        {
            Ellipse newEllipse1 = new Ellipse();
            newEllipse1.Fill = Brushes.Red;
            newEllipse1.Width = 15;
            newEllipse1.Height = 15;

            Canvas.SetTop(newEllipse1, P1headPosition.Y);
            Canvas.SetLeft(newEllipse1, P1headPosition.X);


            int count = paintCanvas.Children.Count;
            paintCanvas.Children.Add(newEllipse1);
            trailPoints1.Add(P1headPosition);
        }

        private void P2painthead()
        {

            Ellipse newEllipse2 = new Ellipse();
            newEllipse2.Fill = Brushes.Blue;
            newEllipse2.Width = 15;
            newEllipse2.Height = 15;

            Canvas.SetTop(newEllipse2, P2headPosition.Y);
            Canvas.SetLeft(newEllipse2, P2headPosition.X);

            int count = paintCanvas.Children.Count;
            paintCanvas.Children.Add(newEllipse2);
            trailPoints2.Add(P2headPosition);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {

            active = true;
            switch (e.Key)
            {
                //Player 1 controls Arrow keys
                case Key.Down:
                    if (P1previousDirection != (int)MOVINGDIRECTION.UPWARDS)
                        P1direction = (int)MOVINGDIRECTION.DOWNWARDS;
                    break;
                case Key.Up:
                    if (P1previousDirection != (int)MOVINGDIRECTION.DOWNWARDS)
                        P1direction = (int)MOVINGDIRECTION.UPWARDS;
                    break;
                case Key.Left:
                    if (P1previousDirection != (int)MOVINGDIRECTION.TORIGHT)
                        P1direction = (int)MOVINGDIRECTION.TOLEFT;
                    break;
                case Key.Right:
                    if (P1previousDirection != (int)MOVINGDIRECTION.TOLEFT)
                        P1direction = (int)MOVINGDIRECTION.TORIGHT;
                    break;
                //Player 2 controls WASD
                case Key.S:
                    if (P2previousDirection != (int)MOVINGDIRECTION.Up)
                        P2direction = (int)MOVINGDIRECTION.Down;
                    break;
                case Key.W:
                    if (P2previousDirection != (int)MOVINGDIRECTION.Down)
                        P2direction = (int)MOVINGDIRECTION.Up;
                    break;
                case Key.A:
                    if (P2previousDirection != (int)MOVINGDIRECTION.Right)
                        P2direction = (int)MOVINGDIRECTION.Left;
                    break;
                case Key.D:
                    if (P2previousDirection != (int)MOVINGDIRECTION.Left)
                        P2direction = (int)MOVINGDIRECTION.Right;
                    break;
            }
            P1previousDirection = P1direction;
            P2previousDirection = P2direction;
        }
    }
}
