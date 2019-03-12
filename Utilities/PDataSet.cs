using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace Utilities
{

    /// <summary>
    /// 在Silverlight中，微软并没有提供DataSet，DataTable对象，但当进行非常灵活的设计时，又是非常必要的
    /// 这里提供一个简易型的PDataSet对象,作为在客户端使用还是非常方便的。
    /// <example>
    /// PDataSet pds = new PDataSet(res);
    /// ObservableCollection<DetailModel> list = new ObservableCollection<DetailModel>();
    /// foreach (PDataRow v in pds.Tables[0].Rows)
    /// {
    ///     DetailModel dm = new DetailModel { FileName = v["FileName"], Path = _siteUrl + "/" + v["FilePath"], Size = Convert.ToInt64(v["FileSize"]) };
    ///     list.Add(dm);
    /// }
    /// this.fileView.ItemSource = list;   
    /// </example>
    /// </summary>
    public class PDataSet
    {
        /// <summary>
        /// 从指定格式的xml字符串中，生成自定义的PDataSet对象
        /// </summary>
        /// <seealso cref="TransferConvert.ToXml(this DataSet ds)"/>
        /// <param name="xml">由TransferConvert.ToXml方法生成的xml字符串</param>
        public PDataSet(string xml)
        {
            var xDoc = XDocument.Parse(xml);
            var root = xDoc.Root.Element("Tables").Elements();
            var index = 0;
            this.Result = xml;
            this.Tables = new List<PDataTable>(root.Count());

            foreach (XElement elem in root)
            {
                var columns = elem.Element("Columns").Nodes();
                var rows = elem.Element("Rows").Nodes();
                this.Tables.Add(new PDataTable(columns.Count()));
                this.Tables[index].Name = elem.Attribute("Name").Value;
                //初始化列
                foreach (XElement firstNode in columns)
                    this.Tables[index].Columns.Add(new PDataColumn { Name = firstNode.Name.LocalName, DataType = firstNode.Attribute("DataType").Value });
                //填充行数据
                foreach (XElement lastNode in rows)
                {
                    var atts = lastNode.Attributes();
                    PDataRow row = this.Tables[index].NewRow();
                    this.Tables[index].Rows.Add(row);
                    int i = 0;
                    foreach (XAttribute att in atts)
                        row[i++] = att.Value;
                }
                index++;
            }
        }

        /// <summary>
        /// 获取数据集中的所有表
        /// </summary>
        public List<PDataTable> Tables { get; private set; }
        /// <summary>
        /// 获取指定名称的数据表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public PDataTable GetTable(string tableName)
        {
            foreach (PDataTable dt in Tables)
            {
                if (dt.Name == tableName.ToLower())
                    return dt;
            }
            return null;
        }
        /// <summary>
        /// 获取原始的构造字符串
        /// </summary>
        public string Result { get; private set; }
    }
    /// <summary>
    /// PDataSet中表的定义
    /// </summary>
    public class PDataTable
    {
        /// <summary>
        /// 创建表对象
        /// </summary>
        public PDataTable() { }
        /// <summary>
        /// 创建指定列数的表对象
        /// </summary>
        /// <param name="columnCount"></param>
        public PDataTable(int columnCount)
        {
            this.Columns = new List<PDataColumn>(columnCount);
            this.Rows = new List<PDataRow>();
        }
        /// <summary>
        /// 获取或设置表名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置列集合
        /// </summary>
        public List<PDataColumn> Columns { get; set; }
        /// <summary>
        /// 获取此表的数据行
        /// </summary>
        public List<PDataRow> Rows { get; private set; }

        /// <summary>
        /// 创建一个新数据行
        /// </summary>
        public PDataRow NewRow()
        {
            return new PDataRow(this, Columns.Count);
        }
        /// <summary>
        /// 向表中添加列
        /// </summary>
        /// <param name="columnName">列名</param>
        public void NewColumn(string columnName)
        {
            NewColumn(columnName, "");
        }
        /// <summary>
        /// 向表中添加列
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="defValue">该列的默认值</param>
        /// <returns>true：添加成功 false:已经存在同名列</returns>
        public void NewColumn(string columnName, string defValue)
        {
            NewColumn(columnName, defValue, "System.String");
        }
        /// <summary>
        /// 向表中添加列
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="defValue">默认值</param>
        /// <param name="columnType">列类型</param>
        public void NewColumn(string columnName, string defValue, string columnType)
        {
            if (this.Columns.Where(p => p.Name.ToLower() == columnName.ToLower()).Count() > 0)
                throw new Exception("不能添加同名的列(注意:列名不区分大小写)");
            else
            {
                this.Columns.Add(new PDataColumn { Name = columnName, DataType = columnType });
                foreach (var r in this.Rows)
                    r._values.Add(defValue);
            }
        }

        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    int colNum = this.Columns.Count;
        //    for (int j = 0; j < colNum; ++j)
        //    {
        //        if (j == 0)
        //            sb.Append(this.Columns[j].Name);
        //        else
        //            sb.Append("#" + this.Columns[j].Name);
        //    }
        //    sb.Append("\r\n");
        //    //加载展示数据
        //    int i = 0;
        //    foreach (var c in this.Rows)
        //    {
        //        for (i = 0; i < colNum; i++)
        //        {
        //            if (i == 0)
        //                sb.Append(c[i]);
        //            else
        //                sb.Append("#" + c[i]);
        //        }
        //        sb.Append("\r\n");
        //    }
        //    return sb.ToString();
        //}
    }

    /// <summary>
    /// PDataSet中行的定义
    /// </summary>
    public class PDataRow
    {
        public PDataRow(PDataTable parentTable, int columnCount)
        {
            this._table = parentTable;
            this._values = new List<string>(columnCount);
            for (int i = 0; i < columnCount; i++)
                this._values.Add("");
        }
        /// <summary>
        /// 为进行Id比较专门留的一个构造方法
        /// </summary>
        /// <example>
        /// pds.Tables[0].Rows.BinarySearch(new PDataRow("3"), new IdComparer())//IdComparer是一个实现了IComparer<PDataRow>接口的类        /// 
        /// </example>
        /// <param name="id"></param>
        public PDataRow(string id)
        {
            this._values = new List<string>(1);
            this._values.Add(id);
        }
        public PDataTable _table;
        internal List<string> _values;
        /// <summary>
        /// 获取隶属表的列名
        /// </summary>
        internal List<string> _columnNames
        {
            get
            {
                return _table.Columns.Select(p => p.Name).ToList<string>();
            }
        }

        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= _values.Capacity)
                    throw new IndexOutOfRangeException("超出了取值范围");
                else
                    return _values[index];
            }
            set
            {
                if (index < 0 || index > _values.Capacity)
                    throw new IndexOutOfRangeException();
                else
                {
                    _values[index] = value;
                }
            }
        }

        public string this[string key]
        {
            get
            {
                int i = 0;
                foreach (string str in _columnNames)
                {
                    if (str.ToLower() == key.ToLower())
                        return _values[i];
                    i++;
                }
                throw new Exception("您提供的列名不存在");
            }
            set
            {
                for (int i = 0; i < _columnNames.Count; i++)
                {
                    if (_columnNames[i] == key)
                    {
                        _values[i] = value;
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// PDataSet中列的定义
    /// </summary>
    public class PDataColumn
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string DataType { get; set; }
    }
}
