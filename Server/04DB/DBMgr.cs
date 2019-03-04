/****************************************************
	文件：DBMgr.cs
	作者：WANJ
	邮箱: 1173653942@qq.com
	日期：2018/12/30 0:16   	
	功能：
*****************************************************/

using MySql.Data.MySqlClient;
using PEProtocol;
using System;

public class DBMgr
{
    public static bool isNew;
    private static DBMgr instance;
    public static DBMgr Instance
    {
        get
        {
            if (instance == null)
                instance = new DBMgr();
            return instance;
        }
    }

    private MySqlConnection conn;

    public void Init()
    {
        conn = new MySqlConnection("server=localhost;User Id=root;password=;Database=encrypt;Charset=utf8");
        conn.Open();
        PECommon.Log("DBMgr Init Done");

        //QueryPlayerData("xxx", "oooo");
    }

    public PlayerData QueryPlayerData(string acct, string pass)
    {
        PlayerData playerData = null;
        MySqlDataReader reader = null;
        isNew = true;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select *from account where acct = @acct", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                isNew = false;
                string _pass = reader.GetString("pass");
                if (_pass.Equals(pass))
                {
                    playerData = new PlayerData
                    {
                        id = reader.GetInt32("id"),
                        name = reader.GetString("name"),
                    };
                }
            }
        }
        catch (Exception e)
        {
            PECommon.Log("Query PlayerData By Acct&Pass Error:" + e, LogType.Error);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
            if (isNew)
            {
                //不存在账号
                //playerData = new PlayerData
                //{
                //    id = -1,
                //    name = "",
                //};

                //playerData.id = InsertNewAcctData(acct, pass, playerData);
            }
        }
        return playerData;
    }

    public PlayerData CreateNewAcct(string acct,string pass,string name)
    {
        PlayerData playerData = null;
        playerData = new PlayerData
        {
            id = -1,
            name = name,
        };
        playerData.id = InsertNewAcctData(acct, pass, playerData);
        return playerData;
    }

    private int InsertNewAcctData(string acct, string pass, PlayerData pd)
    {
        int id = -1;
        try
        {
            MySqlCommand cmd = new MySqlCommand(
                "insert into account set acct=@acct,pass =@pass,name=@name", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("pass", pass);
            cmd.Parameters.AddWithValue("name", pd.name);

            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        }
        catch (Exception e)
        {
            PECommon.Log("Insert PlayerData Error:" + e, LogType.Error);
        }
        return id;
    }

    public bool QueryNameData(string name)
    {
        bool exist = false;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where name= @name", conn);
            cmd.Parameters.AddWithValue("name", name);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                exist = true;
            }
        }
        catch (Exception e)
        {
            PECommon.Log("QueryNameData Error:" + e, LogType.Error);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        return exist;
    }

    public bool UpdatePlayerData(int id, PlayerData playerData)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(
            "update account set name=@name where id =@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", playerData.name);

            //TOADD Others
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            PECommon.Log("UpdatePlayerData Error:" + e, LogType.Error);
            return false;
        }
        return true;
    }
}

