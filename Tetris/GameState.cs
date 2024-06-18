using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameState
    {
        private Bloklar currentBlok;

        public Bloklar CurrentBlok
        {

            get => currentBlok;
            private set
            {

                currentBlok = value;
                currentBlok.Reset();

                for(int i = 0; i < 2; i++)
                {

                    currentBlok.Move(1, 0);

                    if(!BlokFits())
                    {

                        currentBlok.Move(-1, 0);

                    }
                }
            }
 
        }
        public GameGrid GameGrid { get; }

        public BlokQueue BlokQueue { get; }

        public bool GameOver { get; private set; }

        public int Skor { get; private set; }

        public Bloklar HeldBlok { get; private set; }

        public bool CanHold { get; private set; }

        public GameState()
        {

            GameGrid = new GameGrid(22, 10);
            BlokQueue = new BlokQueue();
            CurrentBlok = BlokQueue.GetAndUpdate();
            CanHold = true;
        }
        private bool BlokFits()
        {

            foreach (Position P in CurrentBlok.TilePositions())
            {

                if (!GameGrid.IsEmpty(P.Row, P.Column))
                {

                    return false;

                }

            }
            return true;
        }

        public void HoldBlok()
        {

            if(!CanHold)
            {

                return;

            }

            if(HeldBlok == null)
            {

                HeldBlok = CurrentBlok;
                CurrentBlok = BlokQueue.GetAndUpdate();
            }
            else
            {

                Bloklar tmp = CurrentBlok;
                CurrentBlok = HeldBlok;
                HeldBlok = tmp;
            }
            CanHold = false;
        }
        public void RotateBlokCW()
        {

            CurrentBlok.RotateCW();

            if (!BlokFits())
            {

                CurrentBlok.RotateCCW();

            }
        }

        public void RotateBlokCCW()
        {

            CurrentBlok.RotateCCW();

            if (!BlokFits())
            {

                CurrentBlok.RotateCW();

            }
        }
        public void MoveBlokLeft()
        {

            CurrentBlok.Move(0, -1);

            if (!BlokFits())
            {

                CurrentBlok.Move(0, 1);

            }
        }
        public void MoveBlokRight()
        {

            CurrentBlok.Move(0, 1);

            if (!BlokFits())
            {

                CurrentBlok.Move(0, -1);

            }
        }
        private bool IsGameOver()
        {

            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));

        }
        private void PlaceBlok()
        {

            foreach (Position p in CurrentBlok.TilePositions())
            {
               GameGrid[p.Row, p.Column] = CurrentBlok.Id;

            }
            Skor += GameGrid.ClearFullRows();

            if (IsGameOver())
            {

                GameOver = true;

            }
            else
            {

                CurrentBlok = BlokQueue.GetAndUpdate();
                CanHold = true;
            }
        }
        public void MoveBlokDown()
        {

            CurrentBlok.Move(1, 0);
            {

                if (!BlokFits())
                {

                    CurrentBlok.Move(-1, 0);
                    PlaceBlok();
                }

            }

        }
        private int TileDropDistance(Position p)
        {

            int drop = 0;

            while(GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {

                drop++;

            }
            return drop;
        }
        public int BlokDropDistance()
        {

            int drop = GameGrid.Rows;

            foreach (Position p in currentBlok.TilePositions())
            {

                drop = System.Math.Min(drop, TileDropDistance(p));

            }
            return drop;
        }

        public void DropBlok()
        {

            CurrentBlok.Move(BlokDropDistance(), 0);
            PlaceBlok();
        }
    }
}
