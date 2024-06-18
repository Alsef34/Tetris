using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class BlokQueue
    {
        private readonly Bloklar[] bloklar = new Bloklar[]
        {
            new IBlok(),
            new JBlok(),
            new LBlok(),
            new OBlok(),
            new SBlok(),
            new TBlok(),
            new ZBlok()
        };
        private readonly Random random= new Random();

        public Bloklar NextBlock { get; private set; }

        public BlokQueue()
        {

            NextBlock = RandomBlok();

        }
        private Bloklar RandomBlok()
        {

            return bloklar[random.Next(bloklar.Length)];

        }
        public Bloklar GetAndUpdate()
        {
            Bloklar blok = NextBlock;

            do
            {

                NextBlock = RandomBlok();

            }
            while (blok.Id == NextBlock.Id);

            return blok;
        }
    }
}
