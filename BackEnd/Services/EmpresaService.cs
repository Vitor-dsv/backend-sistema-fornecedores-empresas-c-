using BackEnd.Models;
using System;
using System.Text;

namespace BackEnd.Services
{
    public class EmpresaService
    {
        private const long DIGITOS_UF = 2;
        private StringBuilder sbErros = new StringBuilder();

        public void ValidarEmpresa(Empresa empresa)
        {
            if (string.IsNullOrEmpty(empresa.Uf) || empresa.Uf.Length != DIGITOS_UF)
            {
                sbErros.AppendLine("É necessário informar um UF válido");
            }

            if (string.IsNullOrEmpty(empresa.Nome))
            {
                sbErros.AppendLine("É necessário informar um Nome.");
            }

            if (string.IsNullOrEmpty(empresa.Cnpj) || !new FornecedorService().CnpjValido(empresa.Cnpj))
            {
                sbErros.AppendLine("É necessário informar um CNPJ válido.");
            }

            if (sbErros.Length > 0)
            {
                throw new Exception(sbErros.ToString());
            }
        }
    }
}
