using System;
using System.Collections.Generic;
using System.Linq;

namespace LabApp.Server.Utilities
{
    public static class FileSignatureChecker
    {
        private static readonly Dictionary<string, List<byte[]>> FileSignatures = 
            new Dictionary<string, List<byte[]>>
            {
                { ".jpeg", new List<byte[]>
                    {
                        new byte[] {0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] {0xFF, 0xD8, 0xFF, 0xE2},
                        new byte[] {0xFF, 0xD8, 0xFF, 0xE3},
                    }
                },
                { ".jpg", new List<byte[]>
                    {
                        new byte[] {0xFF, 0xD8, 0xFF, 0xE0},
                        new byte[] {0xFF, 0xD8, 0xFF, 0xE1},
                        new byte[] {0xFF, 0xD8, 0xFF, 0xE8},
                    }
                },
                { ".png", new List<byte[]>
                    {
                        new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A},
                    }
                }
            };

        /// <param name="ext"> dot-formatted ext (ex. ".jpeg") </param>
        /// <param name="firstBytes"> 8-byte or more array</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="NotSupportedException"> No support</exception>
        public static bool CheckImage(string ext, byte[] firstBytes)
        {
            if (firstBytes == null) throw new ArgumentNullException(nameof(firstBytes));
            if (firstBytes.Length < 8) throw new IndexOutOfRangeException(nameof(firstBytes));
            if (!FileSignatures.ContainsKey(ext)) throw new NotSupportedException(nameof(ext));
            List<byte[]> signatures = FileSignatures[ext];
            
            return signatures.Any(s => firstBytes.Take(s.Length).SequenceEqual(s));
        }
    }
}