using System;
using System.Collections.Generic;
using System.Text;

namespace UnlockMusicDotNet
{
    //用于老版本加密格式
    public class Seed
    {
        public int x { get; set; }
        public int y { get; set; }
        public int dx { get; set; }
        public int index { get; set; }
        public int[,] seedMap { get; set; }


        public Seed()
        {
            this.x = -1;
            this.y = 8;
            this.dx = 1;
            this.index = -1;
            this.seedMap = new int[,]
            {{0x4a, 0xd6, 0xca, 0x90, 0x67, 0xf7, 0x52},
                {0x5e, 0x95, 0x23, 0x9f, 0x13, 0x11, 0x7e},
                {0x47, 0x74, 0x3d, 0x90, 0xaa, 0x3f, 0x51},
                {0xc6, 0x09, 0xd5, 0x9f, 0xfa, 0x66, 0xf9},
                {0xf3, 0xd6, 0xa1, 0x90, 0xa0, 0xf7, 0xf0},
                {0x1d, 0x95, 0xde, 0x9f, 0x84, 0x11, 0xf4},
                {0x0e, 0x74, 0xbb, 0x90, 0xbc, 0x3f, 0x92},
                {0x00, 0x09, 0x5b, 0x9f, 0x62, 0x66, 0xa1}};
        }

        public byte next_mask()
        {
            int ret;
            index++;
            if (x < 0)
            {
                dx = 1;
                y = (8 - y) % 8;
                ret = 0xc3;
            }
            else if (x > 6)
            {
                dx = -1;
                y = 7 - y;
                ret = 0xd8;
            }
            else
            {
                ret = seedMap[y, x];
            }

            x += dx;
            if (index == 0x8000 || (index > 0x8000 && (index + 1) % 0x8000 == 0))
                return next_mask();
            return Convert.ToByte(ret);
        }
    }
}
