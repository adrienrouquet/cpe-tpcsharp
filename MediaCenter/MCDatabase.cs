using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;

namespace MediaCenter
{
    class MCDatabase
    {
        //private String _DBPath = @"C:\Users\Loic\Desktop\Csharp\MediaCenterGit\MediaCenter\";
        private String _DBPath = @"D:\Users\Adrien\Documents\Visual Studio 2012\Projects\cpe-tpcsharp\";

        private String _DBFileName = @"DB.txt";
        private static DataSet _MCDB = null;
        private static GridView _MCView = null;

        public MCDatabase()
        {
            if (_MCDB == null)
                LoadDB();
            if (_MCView == null)
                LoadView();
        }

        public void LoadDB()
        {
	        _MCDB = new DataSet();
	        try
	        {
		        // Creates and opens an ODBC connection
                String strConnString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + _DBPath.Trim() + ";Extensions=asc,csv,tab,txt;Persist Security Info=False;HDR=NO;";
                String sql_select;
		        OdbcConnection conn;
		        conn = new OdbcConnection(strConnString.Trim());
		        conn.Open();

		        //Creates the select command text
                sql_select = "select * from [" + this._DBFileName.Trim() + "]";

		        //Creates the data adapter
		        OdbcDataAdapter obj_oledb_da = new OdbcDataAdapter(sql_select, conn);
				
		        //Fills dataset with the records from CSV file
		        obj_oledb_da.Fill(_MCDB, "csv");

                
                //closes the connection
		        conn.Close();
	        }
	        catch (Exception e) //Error
	        {
                Console.WriteLine("Error in: " + e.ToString());
	        }
        }

        public void LoadView()
        {
            _MCView = new GridView();
        }

        public DataSet GetDB()
        {
            return _MCDB;
        }

        public GridView GetView()
        {
            return _MCView;
        }
        
        public void UpdateMedia(Media FinalMedia)
        {

        }

        public void AddMedia(Media FinalMedia)
        {
            DataTable table = _MCDB.Tables["csv"];

            DataRow DT = table.NewRow();
            DT["ID"] = Int32.Parse(((String) table.Compute("Max(ID)", String.Empty)));
            DT["TypeID"] = FinalMedia.GetType().Name;
            DT["Name"] = FinalMedia.GetName();
            DT["Path"] = FinalMedia.GetPath();
            DT["Size"] = FinalMedia.GetSize();
            DT["Rating"] = FinalMedia.GetRating();
            
            if (FinalMedia.GetType().Name.Equals("Video"))
            {
                DT["IsHD"] = ((Video) FinalMedia).IsHD();
            }
            if (FinalMedia.GetType().Name.Equals("Audio"))
            {
                DT["AudioType"] = ((Audio) FinalMedia).GetAudioType();
            }
            
            table.Rows.Add(DT);
        }
    }
}
