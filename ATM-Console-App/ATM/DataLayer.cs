using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

public class Data
{
    // Read (retreive) content from Json file (user info/transaction info)
    public List<T> ReadFile<T> (string filename)
    {
        List<T> list = new List<T>();
        // filename and its location
        string FilePath = Path.Combine(Environment.CurrentDirectory, filename); 
        StreamReader sr = new StreamReader(FilePath);

        string line = String.Empty; // initializ with empty value
        while ((line=sr.ReadLine()) != null)
        {
            list.Add(JsonSerializer.Deserialize<T>(line)); // parse each line and store in list
        }
        sr.Close(); // close streamReader

        return list;
    }

    // Verify helper that also returns user Role info
    public bool isInFile(User user, out string roleType)
    {
        // store user's Role value
        string role = "";

        // get the stored username and password
        List<User> list = ReadFile<User>("user.txt");

        // verify the entered username and password
        foreach (User individual in list)
        {
            if (individual.UserName == user.UserName && individual.Pin == user.Pin)
            {
                role = individual.Role;
                roleType = role;
                return true;
            }
        }

        roleType = null; // return user role
        return false;
    }

    // Check if record is in file
    public bool isInFile(int AccNo, out User customer)
    {
        List<User> list = ReadFile<User>("user.txt");

        foreach (User individual in list)
        {
            if (individual.AccountNo == AccNo)
            {
                customer = individual;
                return true; 
            }
        }

        customer = null;
        return false;
    }

    // check if user name already exist
    public bool isInFile(string username)
    {
        // get the stored username and password
        List<User> list = ReadFile<User>("user.txt");

        // verify the entered username and password
        foreach (User individual in list)
        {
            if (individual.UserName == username)
            {
                return true;
            }
        }
        return false;
    }

    // Get customer info using username
    public User getCustomer(string username)
    {
        List<User> list = ReadFile<User>("user.txt");

        foreach (User individual in list)
        {
            if (individual.UserName == username)
            {
                return individual;
            }
        }
        return null;
    }

    // ADD new line (Update file)
    // useful when only adding
    public void addToFile<T>(T obj)
    {
        string jsonOutput = JsonSerializer.Serialize(obj);

        if (obj is User)
        {
            File.AppendAllText("user.txt", jsonOutput + Environment.NewLine);
        }
        else if (obj is Transaction)
        {
            File.AppendAllText("transaction.txt", jsonOutput + Environment.NewLine);
        }
    }

    // RECREATE the file and save it
    // useful when changing existing property
    public void saveToFile<T>(List<T> list)
    {
        // delete previous content and write fresh content (Overwite)
        string jsonOutput = JsonSerializer.Serialize(list[0]); // why not serialize the whole list?
        if (list[0] is User)
        {
            File.WriteAllText("user.txt", jsonOutput + Environment.NewLine);
        }
        else if (list[0] is Transaction)
        {
            File.WriteAllText("transaction.txt", jsonOutput + Environment.NewLine);
        }

        // once fresh file is made with first line, add the rest
        for (int i=1; i<list.Count; i++)
        {
            addToFile(list[i]);
        }
    }

    // Update object in file
    public void updateInFile(User customer)
    {
        // load content from JSON file
        List<User> list = ReadFile<User>("user.txt");
        // look for customer in the content
        for (int i=0; i<list.Count; i++)
        {
            if (list[i].AccountNo == customer.AccountNo)
            {
                list[i] = customer; // upadate customer info in the file
            }
        }
        // upload the changes to JSON file
        saveToFile(list);
    }

    // Get Account Count
    public int getAccCount()
    {
        List<User> list = ReadFile<User>("user.txt");
        return list.Count;
    }

    // delete account from file
    // save the changes
    public void deleteFromFile(int accountNo)
    {
        List<User> list = ReadFile<User>("user.txt");

        foreach (User record in list)
        {
            if (record.AccountNo == accountNo)
            {
                list.Remove(record);
                break;
            }
        }
        // save changes
        saveToFile(list);
    }
}