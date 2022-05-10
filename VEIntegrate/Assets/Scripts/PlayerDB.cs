using Mono.Data.Sqlite;
using System.Data;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDB : MonoBehaviour
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
		
		string q_drop = "DROP TABLE Player";
		
		dbcmd.CommandText = q_drop;
		dbcmd.ExecuteNonQuery();
		
		string q_createTable =  
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
	
	public int addPlayer(int moneyAmount){
		string conn = "URI=file:" + Application.dataPath + "/" + databaseName; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();
																										
		dbcmd.CommandText = "INSERT INTO Player (Money) VALUES (?)";
        var parameter = dbcmd.CreateParameter();
        parameter.ParameterName = "Money";
        parameter.Value = moneyAmount;
        dbcmd.Parameters.Add(parameter);	
        dbcmd.Prepare();
        dbcmd.ExecuteNonQuery();
		Debug.Log("Inserted Values into Player DB");	
		dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
		return 1;
	}
	
	public int removeMoney(int playerID, int amount){
		int retVal = getMoneyAmount(playerID);
		if(retVal == -1){
			Debug.Log("Error: retrieving the amount of money that the player has.");
			return -1;
		}else if(retVal < amount){
			Debug.Log("Error: player doesn't have enough money");
			return -2;
		}
		
		string conn = "URI=file:" + Application.dataPath + "/" + databaseName; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();
		
		string sqlQuery = "UPDATE Player "+
						  "SET money=" + (retVal - amount) +
						  "WHERE playerID= " + playerID;
						  
		dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
		
		dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        reader.Close();
        reader = null;
		return 1;
	}
	
	public int addMoney(int playerID, int amount){
		int retVal = getMoneyAmount(playerID);
		if(retVal == -1){
			Debug.Log("Error: retrieving the amount of money that the player has.");
			return -1;
		}
		string conn = "URI=file:" + Application.dataPath + "/" + databaseName; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();
		
		string sqlQuery = "UPDATE Player "+
						  "SET money=" + (amount + retVal) +
						  "WHERE playerID= " + playerID;
						  
		dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
		
		dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        reader.Close();
        reader = null;
		return 1;
	}
	
	public int getMoneyAmount(int playerID){
		string conn = "URI=file:" + Application.dataPath + "/" + databaseName; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();
		
		string sqlQuery = "SELECT * FROM Player WHERE playerID=" + playerID;
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
		int retVal = -1;
		if(reader.Read()){
			retVal = reader.GetInt32(1);
		}else{
			dbcmd.Dispose();
	        dbcmd = null;
	        dbconn.Close();
	        dbconn = null;
	        reader.Close();
	        reader = null;
			return retVal;
		}
        //need to create whatever item subclass from the reader return values
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        reader.Close();
        reader = null;

        return 1;
	}
}
