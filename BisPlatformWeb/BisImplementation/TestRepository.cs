using BisInterface;
using BisPlatform.Data.Common;
using BisPlatform.Data.Entity;
using System;
using System.Linq;

namespace BisImplementation
{
    public class TestRepository : ITestService
    {
        private readonly IRepository<TestEntity> _testrepository;
        private readonly IUserContext _dbContext;
        public TestRepository(IRepository<TestEntity> testrepository, IUserContext dbContext)
        {
            _testrepository = testrepository;
            _dbContext = dbContext;
        }
        public ResponseData<object> add(int a, int b)
        {
            var domain3 = MySqlHelper.Select<TestEntity>("select * from xiaoma where Id=22");
            //linq to entity
            var domain = _testrepository.Table.Where(aa => aa.Id == 22).ToList();
            var domain2 = (from ab in _testrepository.Table where ab.Id == 22 select a).ToList(); 
            //var domain1 = _dbContext.QueryFromSql<object>("select * from xiaoma where Id=22");
            //多表关联查询demo
                            //var query = from pm in _productManufacturerRepository.Table
                            //join p in _productRepository.Table on pm.ProductId equals p.Id
                            //where pm.ManufacturerId == manufacturerId &&
                            //      !p.Deleted &&
                            //      (showHidden || p.Published)
                            //orderby pm.DisplayOrder, pm.Id
                            //select pm;
            return new ResponseData<object>("模板操作成功",true,"返回对象");
        }

    }
}
