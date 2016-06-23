using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBrowsing.Models
{
    public class FileCountModels
    {
        private static long _fileLenghtLess;
        private static long _fileLenghtMiddle;
        private static long _fileLenghtMore;
        public long FileLenghtLess { get; set; }
        public long FileLenghtMiddle { get; set; }
        public long FileLenghtMore { get; set; }

        public static void SetCount(long less, long middle, long more)
        {
            _fileLenghtLess = less;
            _fileLenghtMiddle = middle;
            _fileLenghtMore = more;
        }
        public static FileCountModels GetCount()
        {
            return new FileCountModels {
                FileLenghtLess = _fileLenghtLess,
                FileLenghtMiddle = _fileLenghtMiddle,
                FileLenghtMore = _fileLenghtMore
            };
        }
    }
}