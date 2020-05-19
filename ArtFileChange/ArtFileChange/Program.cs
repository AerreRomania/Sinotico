using System;
using System.Data.SqlClient;

namespace ArtFileChange
    {
    class Program
        {
        [STAThread]
        static void Main(string[] args)
            {
            Console.WriteLine("Sinotico file combiner - Sinotico extension 2019.1 ...");
            
            var q = "select * from combarchive";
            using (var con = new SqlConnection(Config.ConString))
                {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();

                if (dr.HasRows)
                    {
                    Console.ForegroundColor = ConsoleColor.Green;
                    while (dr.Read())
                        {

                        //TODO
                        //save on start 
                        Console.WriteLine("{0};{1};{2};{3}", dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
                        }
                    }
                else
                    {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Storage is empty.");
                    }
                
                con.Close();
                dr.Close();

                var f = new FrmRename();
                f.ShowDialog();
                }

            }
        }
    }
