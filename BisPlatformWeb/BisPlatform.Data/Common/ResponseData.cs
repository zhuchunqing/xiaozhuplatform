using System;
using System.Collections.Generic;
using System.Text;

namespace BisPlatform.Data.Common
{
    public class ResponseData<T> where T :new()
    {
        public ResponseData(string message, bool isSuccess, T result)
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
        /// 返回几条数据
        /// </summary>
        public int Count { get; set; }
    }
}
