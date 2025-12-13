using System.ComponentModel.DataAnnotations;
namespace EvimCebim.Models
{
    public class Expense
    {
        [Key] //Bu, veritabanında bu alanın birincil anahtar (Primary Key) olduğunu belirtir. 
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen harcama başlığını giriniz.")]
        [Display(Name = "Harcama Başlığı")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage ="Lütfen tutarı giriniz.")]
        [Display(Name ="Tutar (TL)")]
        public decimal Amount { get; set; } //150.50 TL gibi
        [Required]
        [Display(Name="Tarih")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } //12.12.2025

        [Required(ErrorMessage = "Katagori boş bırakılamaz")]
        [Display(Name = "Kategori")]
        public string Category { get; set; } = string.Empty; //Fatura, Mutfak
    }
}
