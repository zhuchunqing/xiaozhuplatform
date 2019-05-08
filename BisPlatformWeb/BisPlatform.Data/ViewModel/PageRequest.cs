using BisPlatform.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BisPlatform.Data.ViewModel
{
        /// <summary>
        /// 分页请求基础参数
        /// </summary>
        public class PageRequest
        {
            /// <summary>
            /// 页数 从1开始
            /// </summary>
            public int page { get; set; }
            /// <summary>
            /// 每页数据条数
            /// </summary>
            public int limit { get; set; }

            /// <summary>
            /// 搜索条件
            /// </summary>
            public string search { get; set; }

            /// <summary>
            /// 排序字段
            /// </summary>
            public string orderfield { get; set; }

            /// <summary>
            /// 排序方式  desc  asc
            /// </summary>
            public string orderdir { get; set; }
        }

    /// <summary>
    /// 分页响应参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageReponse<T> where T :new()
    {
        public PageReponse(string message, bool isSuccess, T result,int rowcount)
        {
            if (!isSuccess)
            {
                this.Message = message;
                result = new T();
            }
            else
            {
                this.Message = "操作成功";
                this.Result = result;
            }
            this.IsSuccess = isSuccess;
            this.Count = rowcount;
        }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 返回结构
        /// </summary>
        public T Result { get; set; }
        /// <summary>
        /// 一共有多少条数据
        /// </summary>
        public int Count { get; set; }
    }
    
}
