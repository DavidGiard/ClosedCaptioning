using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractCaptions.Models
{
    class TranscriptionSet
    {
        public int Id { get; set; }

        public Line[] Lines { get; set; }

    }
}
