using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace CsvT
{
    class CsvConv
    {
        public static DataTable ReadFile()
        {
            DataTable dtCsv = new DataTable(); //create a datatable to store the CSV file info- to make it easier to covert to json
            string Fulltext;
            using (StreamReader sr = new StreamReader("Deal.csv")) //steamreader will read through documents
            {
                while (!sr.EndOfStream)
                {
                    Fulltext = sr.ReadToEnd().ToString(); //read full file text and conver to string
                    string[] rows = Fulltext.Split('\n'); //split full file text into rows

                    for (int i = 0; i < rows.Count(); i++)
                    {
                        string[] rowValues = rows[i].Split("||"); //split each row with || to get individual values
                        {
                            DataRow dh = dtCsv.NewRow(); //create 2 row types for datatable, for headers and content
                            DataRow dr = dtCsv.NewRow();
                            if (i == 0)
                            {
                                for (int j = 0; j < rowValues.Count(); j++)
                                {
                                    dtCsv.Columns.Add(rowValues[j]); //look through the first line and see how many values are present = no of columns
                                    dh[j] = rowValues[j].ToString(); //create the headers and add to table
                                }
                                dtCsv.Rows.Add(dh); //add to table
                            }
                            if (i > 0)
                            {
                                for (int k = 0; k < rowValues.Count(); k++)
                                {
                                    dr[k] = rowValues[k].ToString(); //look through remaining lines of data
                                }
                                dtCsv.Rows.Add(dr);//add remaining lines of data under correct header
                            }
                        }
                    }
                }
            }
            foreach (DataRow row in dtCsv.Rows) //method to display table on console to check accuracy
            {
                Console.WriteLine();
                for (int x = 0; x < dtCsv.Columns.Count; x++)
                {
                    Console.Write(row[x].ToString() + " ");
                }
            }
            DataTableToJSON(dtCsv); //calling method to covert to JSON
            return dtCsv;
        }

        public static string DataTableToJSON(DataTable dtCsv)//convert to JSON method
        {
            var JSONString = new StringBuilder();//Stringbuilder enables changes to data so if CSV data is altered it changes in JSON too
            if (dtCsv.Rows.Count > 0)
            {
                JSONString.Append("[");//start JSON file
                for (int i = 0; i < dtCsv.Rows.Count; i++)
                {
                    JSONString.Append("{");//start first row of data
                    for (int j = 0; j < dtCsv.Columns.Count; j++)
                    {
                        if (j < dtCsv.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + dtCsv.Columns[j].ColumnName.ToString() + "\":" + "\"" + dtCsv.Rows[i][j].ToString() + "\",");
                        }   //selecting the headers and matching them to values
                        else if (j == dtCsv.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + dtCsv.Columns[j].ColumnName.ToString() + "\":" + "\"" + dtCsv.Rows[i][j].ToString() + "\"");
                        }   //filling in values for all rows
                    }
                    if (i == dtCsv.Rows.Count - 1)
                    {
                        JSONString.Append("}");//end JSON file when all rows have been read
                    }
                    else
                    {
                        JSONString.Append("},");//start a new row
                    }
                }
                JSONString.Append("]");//end file
            }
            //save output into a .json file 
            FileStream Sjson = new FileStream("Deal.json", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter addJ = new StreamWriter(Sjson);
            Console.SetOut(addJ);
            Console.Write(JSONString.ToString());
            Console.SetOut(Console.Out);
            addJ.Close();
            Sjson.Close();
            return JSONString.ToString();
        }
    }
}