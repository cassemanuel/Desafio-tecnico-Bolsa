// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Console.WriteLine("Digite seu nome:");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
string nome = Console.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

Console.WriteLine("Bem vindo {0} {1}", nome, "Bla blaa bla");