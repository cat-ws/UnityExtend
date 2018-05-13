using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

namespace WsFrame
{   
    /// <summary>
    /// 解析csv ，数据保存在csv类中，（所有数据为string，索引从0开始）
    /// 
    /// 解析格式要求：（1）第一行作为列索引，第一列作为行索引，不准重复
    ///              （2）整个矩阵中间没有缺项
    /// 其他：
    ///     继承此类后，扩展自己 的解析和数据处理（类型转换，自己的数据结构等）
    ///     查找失败后会返回null
    /// </summary>
    public class CSV
    {
        /// <summary>
        /// 指定文件所在位置
        /// </summary>
        public enum LoadType { 
            Resource,
            StreamingAssets,
            OrderPath
        }

		private static char[] LineSeparatorChar = new char[]{'\n','\r'}; 
		private static char[] CommaSeparatorChar = new char[]{','}; 
        /// <summary>
        /// 第一行的值-索引号
        /// </summary>
        protected Dictionary<string, int> rowKeys;
        /// <summary>
        /// 第一列的值-索引号
        /// </summary>
        protected Dictionary<string, int> colKeys;
        protected List<List<string>> dataList;
        private bool _isAvailable=false ;
        private int _rowNum=0;
        private int _colNum=0;

        //外部接口
        public bool IsAvailable {
            get {
                return _isAvailable;
            }
            protected set {
                _isAvailable = value;
            }
        }
        public int RowNum {
            get {
                return _rowNum;
            }
        }
        public int ColNum {
            get {
                return _colNum;
            }
        }

        protected string GetDataByIndex(int row,int col) {
            if (row > _rowNum - 1 || col > _colNum - 1 ||row<0 ||col<0)
            {
                return null;
            }
            else {
                return dataList[row][col];
            }
        }

        protected string GetDataByKey(string rowK,string colK) {

            int row = rowKeys.ContainsKey(rowK) ? rowKeys[rowK]:-1;
            int col = colKeys.ContainsKey(colK) ? colKeys[colK] : -1;
            return GetDataByIndex(row, col);
        }
        protected List<string> GetRowsByRowIdx(int index) {

            if (index > _rowNum - 1 || index < 0)
            {
                return null;
            }
            else
            {
                return dataList[index];
            }
        }
        protected List<string> GetRowsByRowKey(string key) {
            int row = rowKeys.ContainsKey(key) ? rowKeys[key] : -1;
            return GetRowsByRowIdx(row);
        }
		protected int GetColIndexByKey(string key) {
            return colKeys.ContainsKey(key) ? colKeys[key] : -1;
        }

        /// <summary>
        /// 加载文件并解析
        /// IsAvailable 可知道解析是否成功
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
         /// Resource文件夹
        /// StreamingAssets文件夹
        /// 或者指定的路径（c#获得）
        public CSV(string path, LoadType type) {
            string oriText = null;
            switch (type){
                case LoadType.Resource:
                    oriText = FileHelper.GetTextFromResource(path);
                    break;
                case LoadType.StreamingAssets:
                    //暂时没写，以后完善
                    break;

                case LoadType.OrderPath:
                    oriText = FileHelper.GetTextFromOrderPath(path);
                    break;            
            }


            if (ParseCSV(oriText, path))
            {
                _isAvailable = true;
            }
            else {
                
                _isAvailable = false;
            }

        }
        private bool ParseCSV(string oriText,string path) {

    
            if (oriText == null || oriText.Length==0)
            {
                Debug.Log("<color=red> CSV 解析失败，文件不存在或没有数据 </color>"+ " 文件："+path );
                return false;
            }
            //解析出来
            dataList = new List<List<string>>();

            string[] rows = oriText.Split(LineSeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            //重置数据
            _rowNum = 0;
            _colNum = 0;
            rowKeys = new Dictionary<string, int>();
            colKeys = new Dictionary<string, int>();

            bool isFirstNoNullLine = false;
            for (int i = 0 ;i<rows.Length;++i){
				string rowstr = rows[i].TrimEnd(CommaSeparatorChar);

                string[] items = rowstr.Split(CommaSeparatorChar);
                if (!isFirstNoNullLine )
                {
                  
                    //检测初始列keys
           
                    for (int j = 0; j < items.Length;++j ) {
                        if(colKeys.ContainsKey(items[j])){
                            Debug.Log("<color=red> CSV 解析失败：重复的key  (" + items[j] + ")</color>" + " 文件：" + path);
                            return false;
                        }
                        else{
                            if (items[j].Length == 0)
                            {
                                Debug.Log("<color=red> CSV 解析失败： 第" + (items[j]+1) + " 列 存在空列key</color>" + " 文件：" + path);
                                return false;
                            }
                            colKeys.Add(items[j], _colNum);
                            ++_colNum;
                            
                        }
                    }
                    isFirstNoNullLine = true;
                }




                //检测 初始行keys
                if (rowKeys.ContainsKey(items[0])){
                    Debug.Log("<color=red> CSV 解析失败：重复的行key （" + items[0] + "）</color>" + " 文件：" + path);
                    return false;
                }else
                {
                    rowKeys.Add(items[0],_rowNum);
                    ++_rowNum; 
                }
                if (_colNum != items.Length)
                {
                    Debug.Log("<color=red> Csv ： 第" + (i + 1) + "行 部分数据缺失</color>" + " 文件：" + path);
                    string[] temp = new string[_colNum];


                    int num = Math.Min(_colNum,items.Length);
                    Array.Copy(items,0,temp,0,num);
                    items = temp;
                }

                dataList.Add(new List<string>(items));

            }
            return true;
        }



    }

}
