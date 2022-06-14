using _1_mimicApi_study_test.V1.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_mimicApi_study_test.Helpers
{
    public class PaginationList<T> 
    {

        public List<T> Results { get; set; } = new List<T>();

        public Paginacao Paginacao { get; set; }

        public List<LinkDTO> links { get; set; } = new List<LinkDTO>();

    }
}
