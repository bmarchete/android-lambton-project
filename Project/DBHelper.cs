using System;
using Android.Content;  // Step: 1 - 0
using Android.Database.Sqlite; // Step: 1 - 1
using Android.Database;

namespace Project
{
    public class DBHelper : SQLiteOpenHelper  // Step: 1 - 2 // Class that you need extend 
    {

        //Step: 1 - 3:
        private static string _DatabaseName = "mydatabase.db";
        private const string TableName = "USERS";

        public const string CreateUserTableQuery = "CREATE TABLE " +
        TableName + " (" +
            "EMAIL TEXT," +
            "PASSWORD TEXT," +
            "NAME TEXT," +
            "AGE TEXT," +
            "PHONE TEXT)";  //Step: 1 - 4

        SQLiteDatabase myDBObj; // Step: 1 - 5
        Context myContext; // Step: 1 - 6

        public DBHelper(Context context) : base(context, name: _DatabaseName, factory: null, version: 1) //Step 2;
        {
            myContext = context;
            myDBObj = WritableDatabase; // Step:3 create a DB objects
            //myDBObj.ExecSQL("DROP TABLE USER");
            //myDBObj.ExecSQL(CreateUserTableQuery);
        }

        public override void OnCreate(SQLiteDatabase db)  // Step: 1 - 2:1
        {
            db.ExecSQL(CreateUserTableQuery);  // Step: 4
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) // Step: 1 - 2:2
        {
            throw new NotImplementedException();
        }

        public void insertUser(string email, string password, string name, string age, string phone)
        {
            string insertSQL = "insert into " + TableName + " values ("
                + "'" + email + "'" + ","
                + "'" + password + "'" + ","
                + "'" + name + "'" + ","
                + "'" + age + "'" + ","
                + "'" + phone + "'" + ");";

            System.Console.WriteLine("Insert SQL " + insertSQL);
            myDBObj.ExecSQL(insertSQL);
        }

        public void updateUser(string email, string password, string name, string age, string phone)
        {
            string updateSQL = "update " + TableName + " set "
                + "PASSWORD = '" + password + "'" + ", "
                + "NAME = '" + name + "'" + ", "
                + "AGE = '" + age + "'" + ", "
                + "PHONE = '" + password + "' "
                + "where EMAIL = '" + email + "';";

            System.Console.WriteLine("UPDATE SQL " + updateSQL);
            myDBObj.ExecSQL(updateSQL);
        }

        public void deleteRecordById(string email)
        {
            string myDelete = "delete from " + TableName + "where email=" + email;
            myDBObj.ExecSQL(myDelete);
        }


        public bool checkEmailIDExisit(string email)
        {
            string selectStm = "Select EMAIL from " + TableName + " where EMAIL=" + "'" + email + "'";
            Console.WriteLine(selectStm);
            ICursor result = myDBObj.RawQuery(selectStm, null);

            if (result.Count > 0)
                return true;
            else
                return false;
        }

        public bool checkLogin(string username, string password)
        {
            string selectStm = "Select EMAIL from " + TableName + " where EMAIL=" + "'" + username + "' and PASSWORD = '" + password + "'";
            ICursor result = myDBObj.RawQuery(selectStm, null);

            if (result.Count > 0)
                return true;
            else
                return false;
        }

        public ICursor selectUser(string email)
        {
            string selectStm = "Select PASSWORD, NAME, AGE, PHONE from " + TableName + " where EMAIL=" + "'" + email + "'";
            ICursor result = myDBObj.RawQuery(selectStm, null);
            result.MoveToFirst();

            if (result.Count > 0)
                return result;
            else
                return null;
        }

        public ICursor selectAllUsers()
        {
            string selectStm = "Select EMAIL, PASSWORD, NAME, AGE, PHONE from " + TableName;
            ICursor result = myDBObj.RawQuery(selectStm, null);
            result.MoveToFirst();

            if (result.Count > 0)
                return result;
            else
                return null;
        }
    }
}