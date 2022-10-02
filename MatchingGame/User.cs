using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;



namespace MatchingGame
{
    public class User
    {
        
        
        public static String  Username = "Adventurer"; //default username="Adventurer" σε περιπτωση που δεν δωσει ονομα ο χρηστης που παιζει.Ειναι static γιατι ανηκει στη κλαση και public για να μπορουν να τη βλεπουν οι αλλες φορμες
       
        public void Insert(String Username, int Time,int Mistakes,String Timestamp)
        {
            //γραφουμε στη βαση δεδομενων 
            String ConnectionString = "Data Source=MatchingGameDB.db; version=3;";
            SQLiteConnection Conn = new SQLiteConnection(ConnectionString);
            Conn.Open();
            String insertQuery = "insert into Users(Username, Time, Mistakes, Timestamp) values('" + Username + "','"
                + Time + "','" 
                + Mistakes + "','" + Timestamp + "')";
            SQLiteCommand cmd = new SQLiteCommand(insertQuery, Conn);

            int count = cmd.ExecuteNonQuery();
            

            Conn.Close();
        }
        public string Read()
        {
            //διαβαζουμε απο τη βαση δεδομενων
            String ConnectionString = "Data Source=MatchingGameDB.db; version=3;";
            SQLiteConnection Conn = new SQLiteConnection(ConnectionString);
            Conn.Open();
            String readQuery = "SELECT * FROM Users ORDER BY Time ASC,Mistakes ASC  LIMIT 10 ";//παιρνει τα δεκα καλυτερα ταξινομημενα κατα διαρκεια παιχνιδιου και λαθων με οριο δεκα
            SQLiteCommand cmd = new SQLiteCommand(readQuery, Conn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            while (reader.Read())
            {
                sb.Append("Username: ").Append(reader.GetString(1)).Append(" Mistakes: ")
                    .Append(reader.GetInt32(2)).Append(" Time: ").Append(reader.GetInt32(3)).Append(" Date: ").Append(reader.GetString(4))
                    .Append(Environment.NewLine).Append(Environment.NewLine);
            }
            
            

            Conn.Close();
            return sb.ToString();
        }

    }
}
