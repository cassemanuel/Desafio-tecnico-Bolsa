// See https://aka.ms/new-console-template for more information
// lembrar de dar build para passar os args direito
using System; //tem as libs gerais

namespace Desafio_INOA
{
    public class Program
    {
        public static void Main(string[] args) //já inicia com uma string de argumentos (3 argumentos devem ser passados)
        {
            Console.WriteLine("Bem-vindos ao Monitoramento de Cotação de Ativo da B3\n");
            Console.WriteLine("--------------------------------------------------\n");

            if (args.Length != 3) //caso a passem os parametro errados no console
            {
                Console.WriteLine("Uso: MonitorB3 <ativo> <preco_venda> <preco_compra>");
                Console.WriteLine("Exemplo: MonitorB3 PETR4 22.67 22.59\n"); //exemplo passado pelo Desafio
                return;
            }

            string ativo = args[0]; //pega o primeiro argumento
            if (!decimal.TryParse(args[1], out decimal precoVenda)) //funçao de conversão do string para decimal
            {
                Console.WriteLine("Erro: O preço de venda deve ser um número decimal válido.");
                return;
            }
            if (!decimal.TryParse(args[2], out decimal precoCompra))
            {
                Console.WriteLine("Erro: O preço de compra deve ser um número decimal válido.");
                return;
            }

            Console.WriteLine($"Monitorando o ativo: {ativo}");
            Console.WriteLine($"Preço de venda de referência: {precoVenda:N2}");
            Console.WriteLine($"Preço de compra de referência: {precoCompra:N2}\n");

            //instanciando emailservice
            EmailService emailService = new EmailService();

            Console.WriteLine("Tentando enviar o e-mail..."); //checagem de seguranca se chegou ate aqui

            // email de teste
            emailService.SendEmail(
                "silvacassio6@outlook.com", // Mesmo destinatário para teste
                "Teste de Alerta - 3 Parâmetros",
                $"O programa recebeu 3 parâmetros: Ativo={ativo}, Venda={precoVenda}, Compra={precoCompra}. Este é um e-mail de teste."
            );

            Console.WriteLine("\nPressione qualquer tecla para sair.");
            Console.ReadKey();
        }
    }
}
