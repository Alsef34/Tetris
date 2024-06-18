using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public abstract class Bloklar
    {


        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffset { get; }
        public abstract int Id { get; }
        private int RotationState;
        private Position offset;

        public Bloklar()
        {

            offset = new Position(StartOffset.Row, StartOffset.Column);

        }

        public IEnumerable<Position> TilePositions()
        {

            foreach (Position p in Tiles[RotationState])
            {

                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);

            }


        }
        public void RotateCW()
        {
            RotationState = (RotationState + 1) % Tiles.Length;


        }
        public void RotateCCW()
        {

            if (RotationState == 0)
            {

                RotationState = Tiles.Length - 1;

            }
            else
            {

                RotationState--;

            }
        }
        public void Move(int rows, int columns)
        {

            offset.Row += rows;
            offset.Column += columns;
        }
        public void Reset()
        {

            RotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
