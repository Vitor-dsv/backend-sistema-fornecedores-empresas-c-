using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class Fornecedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public long EmpresaFK { get; set; }

        public Empresa Empresa { get; set; }

        [Required]
        public string CpfOuCnpj { get; set; }

        [Required]
        public DateTime DataEHoraDoCadastro { get; set; }

        public string Telefone { get; set; }

        public string Rg { get; set; }

        public DateTime? Nascimento { get; set; }

        public TipoPessoa TipoPessoa { get; set; }
    }
}
