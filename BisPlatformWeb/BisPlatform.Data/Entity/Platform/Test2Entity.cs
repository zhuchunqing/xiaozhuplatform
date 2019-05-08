using System;
using System.Collections.Generic;
using System.Text;

namespace BisPlatform.Data.Entity.Platform
{
    public class Test2Entity : BaseEntity
    {
        public float BIM { get; set; }
        public float Height{get;set;}

        public int TestId { get; set; } 
        public TestEntity testEntity { get; set; }
    }
}
