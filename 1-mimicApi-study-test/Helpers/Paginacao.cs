using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_mimicApi_study_test.Helpers
{
    public class Paginacao
    {
        public int NumeroPagina { get; set; }

        public int RegistroPorPagina { get; set; }

        public int TotalRegistros { get; set; }

        public int TotalPaginas { get; set; }

        public Paginacao()
        {

        }
    }
}
