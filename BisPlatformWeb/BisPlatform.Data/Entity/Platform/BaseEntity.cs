using System;
using System.Collections.Generic;
using System.Text;

namespace BisPlatform.Data.Entity
{
    public abstract partial class BaseEntity
    {
        /// <summary>
        ///主键Id
        /// </summary>
        public int Id { get; set; }
    }
}
