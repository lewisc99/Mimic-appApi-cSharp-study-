using _1_mimicApi_study_test.V1.Models;
using _1_mimicApi_study_test.V1.Models.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_mimicApi_study_test.Helpers
{
    public class DTOMapperProfile : Profile
    {



        public DTOMapperProfile()
        {
            CreateMap<Palavra, PalavraDTO>();

            CreateMap<PaginationList<Palavra>, PaginationList<PalavraDTO>>();

        }




    }
}
