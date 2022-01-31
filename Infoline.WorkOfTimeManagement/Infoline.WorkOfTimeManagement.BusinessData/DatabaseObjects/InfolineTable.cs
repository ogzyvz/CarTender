using System;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public class InfolineTable
    {
        public InfolineTable()
        {
            id = Guid.NewGuid();
        }
        /// <summary>
        /// Kayıt ID
        /// </summary>
        public Guid id { get; set; }
        /// <summary>
        /// Oluşturma Tarihi
        /// </summary>
        public DateTime? created { get; set; }
        /// <summary>
        /// Değiştirme Tarihi
        /// </summary>
        public DateTime? changed { get; set; }
        /// <summary>
        /// Değiştiren User
        /// </summary>        
        public Guid? changedby { get; set; }        
        /// <summary>
        /// Oluşturan User
        /// </summary>
        public Guid? createdby { get; set; }
    }
}
