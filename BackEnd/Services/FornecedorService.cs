using System;
using BackEnd.Models;
using System.Text;

namespace BackEnd.Services
{
    public class FornecedorService
    {
        private const string PARANA = "PR";
        private const long IDADE_MINIMA = 18;
        private const long DIGITOS_CPF = 11;
        private const long DIGITOS_CNPJ = 14;
        private StringBuilder sbErros = new StringBuilder();

        public void ValidarFornecedor(Fornecedor fornecedor, Empresa empresa)
        {
            if (fornecedor.TipoPessoa == TipoPessoa.Fisica)
            {
                ValidarPessoaFisica(fornecedor, empresa);
            }
            else if (fornecedor.TipoPessoa == TipoPessoa.Juridica)
            {
                ValidarPessoaJuridica(fornecedor);
            }

            if (string.IsNullOrEmpty(fornecedor.Nome))
            {
                sbErros.AppendLine("É necessário informar o Nome.");
            }

            if (fornecedor.DataEHoraDoCadastro == DateTime.MinValue || fornecedor.DataEHoraDoCadastro == null)
            {
                sbErros.AppendLine("É necessário informar a Data de cadastro.");
            }

            if (sbErros.Length > 0)
            {
                throw new Exception(sbErros.ToString());
            }
        }

        public void ValidarPessoaFisica(Fornecedor fornecedor, Empresa empresa)
        {
            if (string.IsNullOrEmpty(fornecedor.Rg))
            {
                sbErros.AppendLine("É necessário informar o RG.");
            }

            if (fornecedor.Nascimento == DateTime.MinValue || fornecedor.Nascimento == null)
            {
                sbErros.AppendLine("É necessário informar a Data de Nascimento.");
            }
            else
            {
                ValidarIdade(fornecedor, empresa);
            }

            if (string.IsNullOrEmpty(fornecedor.CpfOuCnpj) || !CpfValido(fornecedor.CpfOuCnpj))
            {
                sbErros.AppendLine("É necessário informar um CPF válido.");
            }
        }

        private void ValidarPessoaJuridica(Fornecedor fornecedor)
        {
            if (string.IsNullOrEmpty(fornecedor.CpfOuCnpj) || !CnpjValido(fornecedor.CpfOuCnpj))
            {
                sbErros.AppendLine("É necessário informar um CNPJ válido.");
            }

            if (fornecedor.Nascimento == null)
            {
                fornecedor.Nascimento = DateTime.MinValue;
            }
        }

        public void ValidarIdade(Fornecedor fornecedor, Empresa empresa)
        {
            if (empresa.Uf == PARANA)
            {
                long idade = CalcularIdade(fornecedor.Nascimento.Value);

                if (idade < IDADE_MINIMA)
                {
                    sbErros.AppendLine("É necessário que o Fornecedor tenha 18 anos ou mais.");
                }
            }
        }

        public long CalcularIdade(DateTime dataNascimento)
        {
            long idade = DateTime.Now.Year - dataNascimento.Year;

            if (DateTime.Now.DayOfYear < dataNascimento.DayOfYear)
            {
                idade -= 1;
            }

            return idade;
        }

        private bool CpfValido(string cpf)
        {
            long[] multiplicador1 = new long[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            long[] multiplicador2 = new long[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = RetornarSomenteNumeros(cpf);

            if (cpf.Length != DIGITOS_CPF)
            {
                return false;
            }

            for (int j = 0; j < 10; j++)
            {
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                {
                    return false;
                }
            }

            string tempCpf = cpf.Substring(0, 9);
            long soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += long.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            }

            long resto = soma % DIGITOS_CPF;

            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = DIGITOS_CPF - resto;
            }

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }

            resto = soma % 11;

            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = DIGITOS_CPF - resto;
            }

            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public bool CnpjValido(string cnpj)
        {
            long[] multiplicador1 = new long[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            long[] multiplicador2 = new long[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            cnpj = RetornarSomenteNumeros(cnpj);

            if (cnpj.Length != DIGITOS_CNPJ)
            {
                return false;
            }

            string tempCnpj = cnpj.Substring(0, 12);
            long soma = 0;

            for (int i = 0; i < 12; i++)
            {
                soma += long.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            }

            long resto = (soma % 11);

            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
            {
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            }

            resto = (soma % 11);

            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        private string RetornarSomenteNumeros(string texto)
        {
            return String.Join("", System.Text.RegularExpressions.Regex.Split(texto, @"[^\d]"));
        }
    }
}
