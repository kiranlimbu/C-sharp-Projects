using System;

public class User
{
    public string UserName { get; set; }
    public string Pin { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public string AccountType { get; set; }
    public double Balance { get; set; }
    public string Status { get; set; }
    public int AccountNo { get; set; }
}

public class Transaction
{
    public int AccountNo { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string TransactionType { get; set; }
    public double TransactionAmount { get; set; }
    public string Date { get; set; }
    public double Balance { get; set; }
}