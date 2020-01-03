using System;
using System.Collections.Generic;
using System.Text;

namespace UnlockMusicDotNet
{
    //用于mflac加密格式
    public class Mask
    {
        public int index { get; set; }
        public int mask_index { get; set; }
        public byte[] mask { get; set; }
        public int[] flac_Header { get; set; }

        public Mask()
        {
            this.index = -1;
            this.mask_index = -1;
            this.mask = new byte[128];
            this.flac_Header = new int[] { 0x66, 0x4C, 0x61, 0x43, 0x00 };
        }

        public bool detectMask(byte[] data)
        {
            int search_len = data.Length - 256;
            byte[] mask = new byte[128];

            for (int block_idx = 0; block_idx < search_len; block_idx += 128)
            {
                bool flag = true;
                //mask = data.slice(block_idx, block_idx + 128);
                //let next_mask = data.slice(block_idx + 128, block_idx + 256);
                Array.Copy(data, block_idx, mask, 0, 128);

                byte[] next_mask = new byte[128];
                Array.Copy(data, block_idx + 128, next_mask, 0, 128);
                for (int idx = 0; idx < 128; idx++)
                {
                    if (mask[idx] != next_mask[idx])
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag) continue;


                for (int test_idx = 0; test_idx < this.flac_Header.Length; test_idx++)
                {
                    int p = data[test_idx] ^ mask[test_idx];
                    if (p != this.flac_Header[test_idx])
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag) continue;
                this.mask = mask;
                return true;
            }
            return false;
        }

        public byte nextMask()
        {
            this.index++;
            this.mask_index++;
            if (this.index == 0x8000 || (this.index > 0x8000 && (this.index + 1) % 0x8000 == 0))
            {
                this.index++;
                this.mask_index++;
            }
            if (this.mask_index >= 128)
            {
                this.mask_index -= 128;
            }
            return Convert.ToByte(this.mask[this.mask_index]);
        }
    }
}
