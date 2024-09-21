using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Application.DTOS.ResultDTOs
{
    public class ResultGetDTO
    {
        public int ResultId { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public double Grade { get; set; }
    }
}
