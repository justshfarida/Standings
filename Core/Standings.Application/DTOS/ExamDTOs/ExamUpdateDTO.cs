using Standings.Domain.Entities.AppDbContextEntity;
using Standings.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Application.DTOS.ExamDTOs
{
    public class ExamUpdateDTO
    { 
        public string Name { get; set; }
        public DateTime ExamDate { get; set; }
       public double Coefficient { get; set; }
}
}
