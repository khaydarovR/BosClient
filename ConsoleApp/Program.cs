
using BosApi;

Console.WriteLine("===========INIT APP==============");

var client = new BosClient("qqF2gAiGAcVg/0E5pafuLDjWxpd457P00Ry99m03kUA=");

var response = await client.ReadSecret("a0229f3e-2ac6-46d8-b8f0-7f4819963438");

if (response.IsSuccess)
{
    Console.WriteLine("=========Секрет успешно получен============");
    Console.WriteLine("Название: " + response.Data.Title);
    Console.WriteLine("Логин: " + response.Data.ELogin);
    Console.WriteLine("Пароль: " + response.Data.EPw);
    Console.WriteLine("Секрет: " + response.Data.ESecret);
}
else
{
    Console.WriteLine("==========Ошибка получения секрета==============");
    foreach (var er in response.ErrorList)
    {
        Console.WriteLine(er);
    }
}