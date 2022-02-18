using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Data.DataModels.Abstract
{
    //DataModeler'in id'sini içeren ortak bir base class.
    public class BaseProperty : IDataModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
