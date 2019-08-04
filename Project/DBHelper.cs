using System;
using Android.Content;  // Step: 1 - 0
using Android.Database.Sqlite; // Step: 1 - 1
using Android.Database;
using System.Collections.Generic;
using System.Collections;

namespace Project
{
    public class DBHelper : SQLiteOpenHelper  // Step: 1 - 2 // Class that you need extend 
    {

        private static string _DatabaseName = "mydatabase.db";
        
        public const string CreateUserTableQuery = "CREATE TABLE USERS (" +
            "EMAIL TEXT," +
            "PASSWORD TEXT," +
            "NAME TEXT," +
            "AGE TEXT," +
            "PHONE TEXT)";

        public const string CreateNewsTableQuery = "CREATE TABLE NEWS (" +
            "EMAIL TEXT," +
            "TITLE TEXT," +
            "IMAGE TEXT," +
            "CONTENT TEXT)";

        SQLiteDatabase myDBObj;
        Context myContext;

        public DBHelper(Context context) : base(context, name: _DatabaseName, factory: null, version: 1) 
        {
            myContext = context;
            myDBObj = WritableDatabase;
            //myDBObj.ExecSQL("DROP TABLE USER");
            //myDBObj.ExecSQL(CreateUserTableQuery);
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(CreateUserTableQuery);
            db.ExecSQL(CreateNewsTableQuery);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            throw new NotImplementedException();
        }

        public void insertSQL(Dictionary<string, string> insertArray, string table)
        {
            string insertStr = "INSERT INTO " + table + " VALUES (";
            int count = 0;
            foreach (var i in insertArray)
            {
                if (count < insertArray.Count - 1)
                {
                    insertStr += "'" + i.Value + "'" + ", ";
                    count++;
                }
                else
                {
                    insertStr += "'" + i.Value + "'" + ");";
                }
            }
            System.Console.WriteLine(insertStr);
            myDBObj.ExecSQL(insertStr);
        }

        public void updateSQL(Dictionary<string, string> updateArray, string table, Dictionary<string, string> whereClause)
        {
            string updateStr = "UPDATE " + table + " SET ";
            int count = 0;
            foreach (var i in updateArray)
            {
                if (count < updateArray.Count - 1)
                {
                    updateStr += i.Key +" = '" + i.Value + "'" + ", ";
                    count++;
                }
                else
                {
                    updateStr += i.Key + " = '" + i.Value + "'";
                }
            }
            count = 0;
            updateStr += "WHERE ";
            foreach (var i in whereClause)
            {
                if (count < whereClause.Count - 1)
                {
                    updateStr += i.Key + " = '" + i.Value + "'" + ", ";
                    count++;
                }
                else
                {
                    updateStr += i.Key + " = '" + i.Value + "'";
                }
            }
            System.Console.WriteLine(updateStr);
            myDBObj.ExecSQL(updateStr);
        }

        public void deleteSQL(string table, Dictionary<string, string> whereClause)
        {
            string updateStr = "DELETE FROM " + table + " WHERE ";
            int count = 0;
            foreach (var i in whereClause)
            {
                if (count < whereClause.Count - 1)
                {
                    updateStr += i.Key + " = '" + i.Value + "'" + ", ";
                    count++;
                }
                else
                {
                    updateStr += i.Key + " = '" + i.Value + "'";
                }
            }
            System.Console.WriteLine(updateStr);
            myDBObj.ExecSQL(updateStr);
        }

        public bool checkEmailIDExisit(string email)
        {
            string selectStm = "Select EMAIL from USERS where EMAIL=" + "'" + email + "'";
            return checkStatement(selectStm);
        }

        public bool checkLogin(string username, string password)
        {
            string selectStm = "Select EMAIL from USERS where EMAIL=" + "'" + username + "' and PASSWORD = '" + password + "'";
            return checkStatement(selectStm);
        }

        public bool checkStatement(string selectStm)
        {
            ICursor result = myDBObj.RawQuery(selectStm, null);
            if (result.Count > 0)
                return true;
            else
                return false;
        }

        public ICursor selectUser(string email)
        {
            string selectStm = "Select NAME, AGE, PHONE, PASSWORD from USERS where EMAIL=" + "'" + email + "'";
            return returnStatement(selectStm);
        }

        public ICursor selectAllUsers()
        {
            string selectStm = "Select EMAIL, PASSWORD, NAME, AGE, PHONE from USERS";
            return returnStatement(selectStm);
        }

        public ICursor returnStatement(string selectStm)
        {
            ICursor result = myDBObj.RawQuery(selectStm, null);
            result.MoveToFirst();
            if (result.Count > 0)
                return result;
            else
                return null;
        }
    }
}