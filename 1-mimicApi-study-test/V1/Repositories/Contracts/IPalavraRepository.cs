using _1_mimicApi_study_test.Helpers;
using _1_mimicApi_study_test.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_mimicApi_study_test.V1.Repositories.Contracts
{
    public interface IPalavraRepository
    {


        PaginationList<Palavra> ObterPalavras(PalavraUrlQuery query);

        Palavra Obter(int id);

        void Cadastrar(Palavra palavra);

        void Atualizar(Palavra palavra);

        void Deletar(int id);


    }
}
