using BisPlatform.Data.Entity.Platform;
using System;
using System.Collections.Generic;
using System.Text;

namespace BisPlatform.Data.Entity
{
    public class TestEntity: BaseEntity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public string Score { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        public string testtw { get; set; }

        public Test2Entity test2Entity { get; set; }
    }
}
