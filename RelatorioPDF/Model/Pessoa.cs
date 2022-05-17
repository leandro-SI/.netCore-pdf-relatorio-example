using System;
using System.Collections.Generic;
using System.Text;

namespace RelatorioPDF.Model
{
    public class Pessoa
    {
        public string Nome { get; set; }
        public int Idade { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public string Ativo { get; set; }
    }
}
