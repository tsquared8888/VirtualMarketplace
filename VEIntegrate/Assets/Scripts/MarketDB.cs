using Mono.Data.Sqlite;
using System.Data;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MarketDB : MonoBehaviour
{
    public string databaseName;
    //private string ListingsAsJson;
	void Start(){
		string conn = "URI=file:" + Application.dataPath + "/" + databaseName; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
		//Create Table
		
		IDbCommand dbcmd = dbconn.CreateCommand();
		
		string q_drop = "DROP TABLE Listing";
		
		dbcmd.CommandText = q_drop;
		dbcmd.ExecuteNonQuery();
		
		q_drop = "DROP TABLE Player";
		
		dbcmd.CommandText = q_drop;
		dbcmd.ExecuteNonQuery();
		
		
		string q_createTable = 
		  "CREATE TABLE IF NOT EXISTS Listing (" +
			" ID	INTEGER PRIMARY KEY AUTOINCREMENT," +
			" ItemID	INTEGER NOT NULL," +
			" Price	REAL NOT NULL," +
			" Quantity	INTEGER NOT NULL," +
			" UserID	INTEGER NOT NULL," +
			" Category TEXT NOT NULL)";
		  
		dbcmd.CommandText = q_createTable;
		dbcmd.ExecuteNonQuery();
		Debug.Log("Listing Table was successfully created!");
		
		q_createTable = 
		"CREATE TABLE IF NOT EXISTS Player (" +
			" PlayerID	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
			" Money	INTEGER NOT NULL" +
		")";
		
		dbcmd.CommandText = q_createTable;
		dbcmd.ExecuteNonQuery();
		Debug.Log("Player Table was successfully created!");
		
		
		//need to create whatever item subclass from the reader return values
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
       
	
	}
	
	public int addListing(int itemID, double price, int quantity, int userID, string category){
		string conn = "URI=file:" + Application.dataPath + "/" + databaseName; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
		
		IDbCommand dbcmd = dbconn.CreateCommand();
																										
		dbcmd.CommandText = "INSERT INTO Listing (ItemID, Price, Quantity, UserID, Category) VALUES (?,?,?,?,?)";
        var parameter = dbcmd.CreateParameter();
        parameter.ParameterName = "ItemID";
        parameter.Value = itemID;
        dbcmd.Parameters.Add(parameter);

        parameter = dbcmd.CreateParameter();
        parameter.ParameterName = "Price";
        parameter.Value = price;
        dbcmd.Parameters.Add(parameter);

        parameter = dbcmd.CreateParameter();
        parameter.ParameterName = "Quantity";
        parameter.Value = quantity;
        dbcmd.Parameters.Add(parameter);
		
		parameter = dbcmd.CreateParameter();
        parameter.ParameterName = "UserID";
        parameter.Value = userID;
        dbcmd.Parameters.Add(parameter);
		
		parameter = dbcmd.CreateParameter();
        parameter.ParameterName = "Category";
        parameter.Value = category;
        dbcmd.Parameters.Add(parameter);
		
        dbcmd.Prepare();
        dbcmd.ExecuteNonQuery();
		Debug.Log("Inserted Values into Listing DB");
		
		dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
		return 1;
	}
	
	public int removeListing(int listingID){
		string conn = "URI=file:" + Application.dataPath + "/" + databaseName; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();
		string sqlQuery = "DELETE FROM LISTING WHERE ID = @id";
        dbcmd.CommandText = sqlQuery;

        var parameter = dbcmd.CreateParameter();
        parameter.ParameterName = "@id";
        parameter.Value = listingID;
        dbcmd.Parameters.Add(parameter);
        dbcmd.Prepare();
        dbcmd.ExecuteNonQuery();
				
		dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
		return 1;
	}
	
	public List<T> getAllListings<T>(){
		try{
			List<T> allListings = new List<T>();
			string conn = "URI=file:" + Application.dataPath + "/" + databaseName; //Path to database.
	        IDbConnection dbconn;
	        dbconn = (IDbConnection)new SqliteConnection(conn);
	        dbconn.Open(); //Open connection to the database.
			IDbCommand dbcmd = dbconn.CreateCommand();
			
			string sqlQuery = "SELECT * FROM Listing";
	        dbcmd.CommandText = sqlQuery;
	        IDataReader reader = dbcmd.ExecuteReader();
			
	        while (reader.Read()){
                string serializedListing = reader[2].ToString();
                T listing = JsonUtility.FromJson<T>(serializedListing);
                if (listing != null)
                {
                    allListings.Add(listing);
                }
             }
	        //need to create whatever item subclass from the reader return values
	        dbcmd.Dispose();
	        dbcmd = null;
	        dbconn.Close();
	        dbconn = null;
	        reader.Close();
	        reader = null;
	
	        return allListings;
		}catch (Exception e){
            Debug.Log(e.ToString());
            return default(List<T>);
        }
	}
	
	public List<T> getCatListings<T>(string category){
		try{
			List<T> allListings = new List<T>();
			string conn = "URI=file:" + Application.dataPath + "/" + databaseName; //Path to database.
	        IDbConnection dbconn;
	        dbconn = (IDbConnection)new SqliteConnection(conn);
	        dbconn.Open(); //Open connection to the database.
			IDbCommand dbcmd = dbconn.CreateCommand();
			
			string sqlQuery = "SELECT * FROM Listing WHERE category = " + category;
	        dbcmd.CommandText = sqlQuery;
	        IDataReader reader = dbcmd.ExecuteReader();
			
	        while (reader.Read()){
                string serializedListing = reader[2].ToString();
                T listing = JsonUtility.FromJson<T>(serializedListing);
                if (listing != null)
                {
                    allListings.Add(listing);
                }
             }
	        //need to create whatever item subclass from the reader return values
	        dbcmd.Dispose();
	        dbcmd = null;
	        dbconn.Close();
	        dbconn = null;
	        reader.Close();
	        reader = null;
	
	        return allListings;
		}catch (Exception e){
            Debug.Log(e.ToString());
            return default(List<T>);
        }
	}
	/*
	public Listing getSingleListing(int listingID){
		string conn = "URI=file:" + Application.dataPath + "/" + databaseName; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
		string sqlQuery = "SELECT * FROM Listing WHERE ID = @id";
	    
		var parameter = dbcmd.CreateParameter();
        parameter.ParameterName = "@id";
        parameter.Value = listingID;
        dbcmd.Parameters.Add(parameter);
        dbcmd.Prepare();
        dbcmd.CommandText = sqlQuery;
	    IDataReader reader = dbcmd.ExecuteReader();
		
		dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
		return default(List<T>);
	}*/
	 public T GetListing<T>(int id, String joinTable = "") 
    {
        if (databaseName != "")
        {
            try
            {
                string conn = "URI=file:" + Application.dataPath + "/" + databaseName; //Path to database.
                IDbConnection dbconn;
                dbconn = (IDbConnection)new SqliteConnection(conn);
                dbconn.Open(); //Open connection to the database.
                IDbCommand dbcmd = dbconn.CreateCommand();
                string sqlQuery;
                if (joinTable != "")
                {
                    sqlQuery = "SELECT * FROM Listing INNER JOIN " + joinTable + " on " + joinTable + ".ID = Listing.ID  WHERE ID = " + id.ToString();
                }
                else
                {
                    sqlQuery = "SELECT * FROM Listing WHERE ID = " + id.ToString();
                }
                dbcmd.CommandText = sqlQuery;
                IDataReader reader = dbcmd.ExecuteReader();

                reader.Read();
                string serializedListing = reader[2].ToString();

                T listing = JsonUtility.FromJson<T>(serializedListing);

                //need to create whatever item subclass from the reader return values
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;
                reader.Close();
                reader = null;
                return listing;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return default(T);

            }
        }
        else
        {
            print("Database name not specified");
            return default(T);
        }
    }

}
