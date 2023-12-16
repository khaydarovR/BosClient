
using BosApi;

var client = new BosClient("erCPHeVMzLLGSG9He/o8cgNCk2qg1S6BxaF37KaoIAY=");

var responseRead = await client.ReadSecret("562cefa5-0b0c-40e9-a3dd-f8739b641336", true);

Console.WriteLine("Логин: " + responseRead.Data.ELogin);
Console.WriteLine("Пароль: " + responseRead.Data.EPw);
Console.WriteLine("Секрет: " + responseRead.Data.ESecret);