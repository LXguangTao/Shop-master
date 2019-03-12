//说明：本类是对文件型数据库进行访问的工具类，需要引入System.Data.SQLite.dll

//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Common;
//using System.Linq;
//using System.Text;
//using System.Data.SQLite;

//namespace Quiz
//{
//    public class SQLiteHelper
//    {
//        SQLiteConnection _connection;
//        /// <summary>
//        /// SQLite连接
//        /// </summary>
//        public SQLiteConnection MyConnection
//        {
//            get
//            {                
//                if (_connection == null)
//                {
//                    _connection = new SQLiteConnection(string.Format("Data Source={0}Content/DB;Version=3;", AppDomain.CurrentDomain.SetupInformation.ApplicationBase));
//                }
//                return _connection;
//            }
//        }

//        /// <summary>
//        /// SQLite增删改
//        /// </summary>
//        /// <param name="sql">要执行的sql语句</param>
//        /// <param name="parameters">所需参数</param>
//        /// <returns>所受影响的行数</returns>
//        public int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
//        {
//            SQLiteTransaction transaction = null;
//            try
//            {
//                int affectedRows = 0;
//                MyConnection.Open();
//                transaction = MyConnection.BeginTransaction();
//                SQLiteCommand command = new SQLiteCommand(sql, MyConnection);
//                command.CommandText = sql;
//                if (parameters != null)
//                {
//                    for (int i = 0; i < parameters.Length; i++)
//                    {
//                        command.Parameters.Add(parameters[i]);
//                    }
//                }
//                affectedRows = command.ExecuteNonQuery();
//                transaction.Commit();

//                return affectedRows;
//            }
//            catch (SQLiteException ex) {
//                if(transaction !=null)
//                    transaction.Rollback();
//                return 0;
//            }
//            finally
//            {
//                if (_connection.State != ConnectionState.Closed)
//                    _connection.Close();
//            }
//        }
//        public int ExecuteNonQuery(List<String> sqls)
//        {
//            SQLiteTransaction transaction = null;
//            try
//            {
//                int affectedRows = 0;
//                MyConnection.Open();
//                transaction = MyConnection.BeginTransaction();
//                SQLiteCommand command = new SQLiteCommand(MyConnection);
//                foreach (String sql in sqls)
//                {
//                    command.CommandText = sql;
//                    affectedRows += command.ExecuteNonQuery();
//                }                
//                transaction.Commit();
//                return affectedRows;
//            }
//            catch (SQLiteException ex)
//            {
//                if (transaction != null)
//                    transaction.Rollback();
//                return 0;
//            }
//            finally
//            {
//                if (_connection.State != ConnectionState.Closed)
//                    _connection.Close();
//            }
//        }

//        /// <summary>
//        /// SQLite查询
//        /// </summary>
//        /// <param name="sql">要执行的sql语句</param>
//        /// <param name="parameters">所需参数</param>
//        /// <returns>结果DataTable</returns>
//        public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
//        {
//            DataTable data = new DataTable();
//            SQLiteCommand command = new SQLiteCommand(sql, MyConnection);
//            if (parameters != null)
//            {
//                for (int i = 0; i < parameters.Length; i++)
//                {
//                    command.Parameters.Add(parameters[i]);
//                }                
//            }
//            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
//            adapter.Fill(data);

//            return data;
//        }

//        ///// <summary>
//        ///// 查询数据库表信息
//        ///// </summary>
//        ///// <returns>数据库表信息DataTable</returns>
//        //DataTable GetSchema()
//        //{
//        //    DataTable data = new DataTable();

//        //    data = connection.GetSchema("TABLES");

//        //    return data;
//        //}
//    }
//}
