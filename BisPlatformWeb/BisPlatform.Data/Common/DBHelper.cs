using BisPlatform.Data.ViewModel;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace BisPlatform.Data.Common
{ 

    public abstract class DBHelper
    {       


        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public abstract string ConnectionString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="pName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>

        public DbParameter CreateParameter(DbCommand cmd, String pName, Object value, System.Data.DbType type)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = pName;
            p.Value = (value == null ? DBNull.Value : value);
            p.DbType = type;
            return p;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract DbCommand CreateCommand();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract DbConnection CreateConnection();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="pagesize"></param>
        /// <param name="rowcount"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public abstract List<T> QueryPageBySql<T>(int start, int pagesize, out int rowcount, string sql, DynamicParameters parameters, string orderby);

        /// <summary>
        /// 分页查询（带检索）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageModel"></param>
        /// <param name="rowcount"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="orderby"></param>
        /// <param name="serarchProList">搜索字段</param>
        /// <returns></returns>
        public abstract List<T> QueryPageBySqlWithFilter<T>(PageRequest pageModel, out int rowcount, string sql, DynamicParameters parameters, string orderby, List<string> serarchProList = null);
        /// <summary>
        /// 返回List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        public List<T> Select<T>(string sql, Object paramObject = null)
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
        public List<dynamic> Select(string sql, Object paramObject = null)
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
        public dynamic Single(string sql, Object paramObject = null)
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
        public T Single<T>(string sql, Object paramObject = null)
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
        public T ExecuteScalar<T>(string sql, Object paramObject = null)
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
        public int Execute(string sql, Object paramObject = null)
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
        public int ExecuteNonQueryReturnId(string sql, Object paramObject = null)
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
        public int ExecuteTran(DbConnection conn, string sql, Object paramObject, DbTransaction transaction)
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
        public List<T> ExecuteStoredProcedure<T>(string sql, Object paramObject)
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
        public DataTable SqlQuery(string sql, Object paramObject = null)
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
    }
}
