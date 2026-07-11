namespace Domain.Models
{
    public class Icon
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Name (Guid) + extension of the icon file (e.g., "bcde-1234-5678-90ab-cdef12345678.png")
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
