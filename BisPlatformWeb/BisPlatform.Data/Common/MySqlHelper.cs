using BisPlatform.Data.ViewModel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace BisPlatform.Data.Common
{
    public static class MySqlHelper
    {
        public static DbParameter CreateParameter(DbCommand cmd, String pName, Object value, System.Data.DbType type)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = pName;
            p.Value = (value == null ? DBNull.Value : value);
            p.DbType = type;
            return p;
        }
        /// <summary>
        /// 返回List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        public static List<T> Select<T>(string sql, Object paramObject = null)
        {
            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                var list = SqlMapper.Query<T>(conn, sql, paramObject);
                return list.ToList<T>();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        public static List<dynamic> Select(string sql, Object paramObject = null)
        {
            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                var list = SqlMapper.Query(conn, sql, paramObject);
                return list.ToList<dynamic>();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        public static dynamic Single(string sql, Object paramObject = null)
        {
            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                var list = SqlMapper.QuerySingleOrDefault<dynamic>(conn, sql, paramObject);
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        public static T Single<T>(string sql, Object paramObject = null)
        {

            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                var list = SqlMapper.QuerySingleOrDefault<T>(conn, sql, paramObject);
                return list;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return default(T);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        /// <summary>
        /// 获取一行一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(string sql, Object paramObject = null)
        {

            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                T t = SqlMapper.ExecuteScalar<T>(conn, sql, paramObject);
                return t;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return default(T);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        public static  int Execute(string sql, Object paramObject = null)
        {
            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                int count = SqlMapper.Execute(conn, sql, paramObject);
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        /// <summary>
        /// ExecuteNonQueryReturnId
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        public static int ExecuteNonQueryReturnId(string sql, Object paramObject = null)
        {
            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                int count = Convert.ToInt32(SqlMapper.ExecuteScalar(conn, sql, paramObject));
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        /// <summary>
        /// 自行维护事务和连接
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="paramObject"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int ExecuteTran(DbConnection conn, string sql, Object paramObject, DbTransaction transaction)
        {
            int count = SqlMapper.Execute(conn, sql, paramObject, transaction);
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        public static List<T> ExecuteStoredProcedure<T>(string sql, Object paramObject)
        {

            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                var list = SqlMapper.Query<T>(conn, sql, paramObject, null, true, null, System.Data.CommandType.StoredProcedure);
                return list.ToList<T>();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        public static DataTable SqlQuery(string sql, Object paramObject = null)
        {
            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                DataTable table = new DataTable("MyTable");
                var reader = SqlMapper.ExecuteReader(conn, sql, paramObject);
                table.Load(reader);
                return table;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
        public static string ConnectionString = "server=localhost;port=3306;user id=root;database=xiaoma2db;password=xiaoma8216;characterset=utf8;Allow User Variables=true;";

        public static DbCommand CreateCommand()
        {
            return new MySql.Data.MySqlClient.MySqlCommand();
        }

        public static DbConnection CreateConnection()
        {
            DbConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = ConnectionString;
            return conn;
        }

        public static List<T> QueryPageBySql<T>(int start, int pagesize, out int rowcount, string sql, DynamicParameters parameters, string orderby)
        {
            using (DbConnection conn = CreateConnection())
            {
                string countSql = string.Format("select count(*) from ({0}) T", sql);
                rowcount = conn.Query<int>(countSql, parameters).SingleOrDefault();

                int startRow = start;

                StringBuilder sb = new StringBuilder();
                sb.Append("select o.* from (");
                sb.Append(sql);
                if (!string.IsNullOrWhiteSpace(orderby))
                {
                    sb.Append(" order by " + orderby);
                }
                sb.AppendFormat(" ) o limit {0},{1}", startRow, pagesize);
                return conn.Query<T>(sb.ToString(), parameters).ToList();
            }
        }

        public static List<T> QueryPageBySqlWithFilter<T>(PageRequest pageModel, out int rowcount, string sql, DynamicParameters parameters, string orderby, List<string> serarchProList = null)
        {
            using (DbConnection conn = CreateConnection())
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append("select d.* from (");
                sqlBuilder.Append(sql + " ) d ");
                string filterStr = " where 1=1 and ( ";
                if (serarchProList != null && !string.IsNullOrWhiteSpace(pageModel.search))
                {
                    pageModel.page = 1;
                    serarchProList.ForEach(item =>
                    {
                        filterStr += $" d.{item.ToLower()} like '%{pageModel.search}%' or ";
                    });
                    sqlBuilder.Append(filterStr.Substring(0, filterStr.LastIndexOf("or")) + " ) ");
                }

                string countSql = string.Format("select count(*) from ({0}) T", sql);
                rowcount = conn.Query<int>(countSql, parameters).SingleOrDefault();

                int startRow = pageModel.page - 1;

                StringBuilder sb = new StringBuilder();
                sb.Append("select o.* from (");
                sb.Append(sqlBuilder);
                if (!string.IsNullOrWhiteSpace(orderby))
                {
                    sb.Append(" order by " + orderby);
                }
                sb.AppendFormat(" ) o limit {0},{1}", startRow, pageModel.limit);
                return conn.Query<T>(sb.ToString(), parameters).ToList();
            }
        }
    }
}
