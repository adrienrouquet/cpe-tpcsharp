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
        private String _DBPath = @"C:\Users\Loic\Desktop\Csharp\MediaCenterGit\MediaCenter\";
        //private String _DBPath = @"D:\Users\Adrien\Documents\Visual Studio 2012\Projects\cpe-tpcsharp\MediaCenter\";

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
            DataTable table = _MCDB.Tables["csv"];

            DataRow[] foundRows;
            foundRows = table.Select("ID = " + FinalMedia.GetID().ToString());
            DataRow DR = foundRows[0];
            
            DR["Type"] = FinalMedia.GetType().Name;
            DR["Name"] = FinalMedia.GetName();
            DR["Path"] = FinalMedia.GetPath();
            DR["Size"] = FinalMedia.GetSize();
            DR["Rating"] = FinalMedia.GetRating();
            DR["AudioType"] = "";
            DR["IsHD"] = "";

            if (FinalMedia.GetType().Name.Equals("Video"))
            {
                DR["IsHD"] = ((Video)FinalMedia).IsHD();
            }
            if (FinalMedia.GetType().Name.Equals("Audio"))
            {
                DR["AudioType"] = ((Audio)FinalMedia).GetAudioType();
            }
        }

        public void AddMedia(Media FinalMedia)
        {
            DataTable table = _MCDB.Tables["csv"];

            DataRow DR = table.NewRow();

            DR["ID"] = Int32.Parse(((String) table.Compute("Max(ID)", String.Empty))) + 1;
            DR["Type"] = FinalMedia.GetType().Name;
            DR["Name"] = FinalMedia.GetName();
            DR["Path"] = FinalMedia.GetPath();
            DR["Size"] = FinalMedia.GetSize();
            DR["Rating"] = FinalMedia.GetRating();
            
            if (FinalMedia.GetType().Name.Equals("Video"))
            {
                DR["IsHD"] = ((Video) FinalMedia).IsHD();
            }
            if (FinalMedia.GetType().Name.Equals("Audio"))
            {
                DR["AudioType"] = ((Audio) FinalMedia).GetAudioType();
            }
            
            table.Rows.Add(DR);
        }

        public void DeleteMedia(int ID)
        {
            DataTable table = _MCDB.Tables["csv"];

            DataRow[] foundRows;
            foundRows = table.Select("ID = " + ID.ToString());
            DataRow DR = foundRows[0];

            table.Rows.Remove(DR);
        }

        public void UnloadDB()
        {
            CreateCSVFile(_MCDB.Tables["csv"], _DBPath);
        }

        public void CreateCSVFile(DataTable dt, string strFilePath)
        {
            // Create the CSV file to which grid data will be exported.
            StreamWriter sw = new StreamWriter(strFilePath, false);

            // First we will write the headers.
            int iColCount = dt.Columns.Count;

            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);

            // Now write all the rows.
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }

                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
    }
}
