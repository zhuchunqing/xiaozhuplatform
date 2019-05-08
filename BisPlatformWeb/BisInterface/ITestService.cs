using BisPlatform.Data.Common;
using System;

namespace BisInterface
{
    public interface ITestService
    {
        ResponseData<object> add(int a, int b);
    }
}
