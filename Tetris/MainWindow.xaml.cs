using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {

            new BitmapImage(new Uri("assets/TileEmpty.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileCyan.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileBlue.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileOrange.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileYellow.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileGreen.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/TilePURPLE.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileRed.png",UriKind.Relative))
        };

        private readonly ImageSource[] blokImages = new ImageSource[]
        {
            new BitmapImage(new Uri("assets/Block-Empty.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-I.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-J.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-L.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-O.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-S.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-T.png",UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-Z.png",UriKind.Relative)),
        };

        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;

        private GameState gameState = new GameState();
        public MainWindow()
        {
            InitializeComponent();

            imageControls = SetupGameCanvas(gameState.GameGrid);
        }
        private Image[,] SetupGameCanvas(GameGrid grid)
        {

            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {

                for (int c = 0; c < grid.Columns; c++)
                {

                    Image imageControl = new Image
                    {

                        Width = cellSize,
                        Height = cellSize

                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
                
            }
            return imageControls;
        }
        private void DrawGrid(GameGrid grid)
        {

            for (int r = 0; r < grid.Rows; r++)
            {

                for (int c = 0; c < grid.Columns; c++)
                {

                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];
                }

            }

        }

        private void DrawBlok(Bloklar blok)
        { 
        
          foreach (Position p in blok.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[blok.Id];

            }
        
        }

        private void DrawNextBlok(BlokQueue blokQueue)
        {

            Bloklar next = blokQueue.NextBlock;
            NextImage.Source = blokImages[next.Id];

        }

        private void DrawHeldBlok(Bloklar heldBlok)
        {

            if(heldBlok == null)
            {

                HoldImage.Source = blokImages[0];

            }
            else
            {

                HoldImage.Source = blokImages[heldBlok.Id];

            }
        }
        
        private void DrawGhostBlok(Bloklar blok)
        {

            int dropDistance = gameState.BlokDropDistance();

            foreach (Position p in blok.TilePositions())
            {

                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[blok.Id];
            }
        }
        private void Draw(GameState gameState)
        {

            DrawGrid(gameState.GameGrid);
            DrawGhostBlok(gameState.CurrentBlok);
            DrawBlok(gameState.CurrentBlok);
            DrawNextBlok(gameState.BlokQueue);
            DrawHeldBlok(gameState.HeldBlok);
            ScoreText.Text = $"Skor: {gameState.Skor}";
        }

        private async Task GameLoop()
        {

            Draw(gameState);

            while(!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Skor * delayDecrease));
                await Task.Delay(delay);
                gameState.MoveBlokDown();
                Draw(gameState);
            }
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Skor: {gameState.Skor}"; 
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {

                return;

            }
            switch(e.Key)
            {

                case Key.Left:
                    gameState.MoveBlokLeft(); break;

                case Key.Right:
                    gameState.MoveBlokRight(); break;

                case Key.Down:
                    gameState.MoveBlokDown(); break;

                case Key.Up:
                    gameState.RotateBlokCW(); break;

                case Key.Z:
                    gameState.RotateBlokCCW(); break;

                case Key.C:
                    gameState.HoldBlok();break;

                case Key.Space:
                    gameState.DropBlok();break;

                default:
                    return;
            }

            Draw(gameState);

        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {

            await GameLoop();

        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {

            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}