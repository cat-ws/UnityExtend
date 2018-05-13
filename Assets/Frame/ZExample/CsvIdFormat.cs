using UnityEngine;
using System.Collections.Generic;
using WsFrame;
using System;
namespace WsFrame
{
    public class CsvIdFormat :CSV
    {   
        private  Dictionary<int,int>  idToRowIndex = new Dictionary<int,int>();
        private int curIndx;
        public CsvIdFormat(string tableName)
            : base(tableName, LoadType.Resource)
        { 
            foreach( KeyValuePair<string,int> entry in rowKeys){
                int id;
                if (int.TryParse(entry.Key,out id) ){
                    idToRowIndex.Add(id,entry.Value);
                }
            }
        
        }
        public List<int> GetAllId(){
      
            List<int> ret = new List<int>(idToRowIndex.Keys);
            return ret;
        }
        
        //设定当前行的方式寻找
        public void SetCurId(int id){
            int ret;
            if (idToRowIndex.TryGetValue(id, out ret))
            {
                curIndx = idToRowIndex[id];
            }
            else {
                curIndx = -1;
            }
        }

  
        public string GetStringInCurId(string colKey) {
             int colIndex = GetColIndexByKey(colKey);
             string ret = GetDataByIndex(curIndx, colIndex);
             return ret ?? "";
        }
        public int GetIntInCurId(string colKey)
        {
            string s = GetStringInCurId(colKey);
            int ret ;
            if (int.TryParse(s, out ret))
            {

                return ret;
            }
            else {
                return -1;
            }
        }
        public float GetFloatInCurId(string colKey)
        {
            string s = GetStringInCurId(colKey);
             float ret ;
             if (float.TryParse(s, out ret))
             {

                 return ret;
             }
             else
             {
                 return 0.0f;
             }
        }
        //ID+colKey获得
        public string GetString(int id, string colKey) {

     
            int rowIdx;
            if (!idToRowIndex.TryGetValue(id, out rowIdx))
            { 
                rowIdx = -1;
            }
            int colIdx = GetColIndexByKey(colKey);
            string ret;
            ret = GetDataByIndex(rowIdx,colIdx);
            return ret ?? "";
        }

        public int GetInt(int id, string colKey)
        {
            string s = GetString(id, colKey);
            int ret;
            if (int.TryParse(s, out ret))
            {

                return ret;
            }
            else
            {
                return -1;
            }
        }

        public float GetFloat(int id, string colKey)
        {
            string s = GetString(id, colKey);
            float ret;
            if (float.TryParse(s, out ret))
            {

                return ret;
            }
            else
            {
                return 0.0f;
            }
        }

    }
}