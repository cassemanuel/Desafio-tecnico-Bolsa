// See https://aka.ms/new-console-template for more information
// lembrar de dar build para passar os args direito
using System; //tem as libs gerais
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Desafio_INOA
{
    public class Program
    {
        public static async Task Main(string[] args) //já inicia com uma string de argumentos (3 argumentos devem ser passados)
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

            //configuracoes do serviço de email
            Configuracao? config = LerConfiguracao("../../../config.json");
            //é necessário voltar 3 pastas pra ficar onde o json tá agora. escolhi assim por motivos de github
            if (config == null)
            {
                Console.WriteLine("Erro ao carregar o arquivo de configuracao.");
                return;
            }

            //instanciando emailservice
            EmailService emailService = new EmailService();

            Console.WriteLine("Tentando enviar o e-mail...\n"); //checagem de seguranca se chegou ate aqui

            // email de teste
            await emailService.SendEmail(
                config.SmtpServidor,
                config.SmtpPorta,
                config.SmtpSSL,
                config.EmailRemetente,
                config.SenhaRemetente,
                config.EmailDestino,
                "ponto de teste se recebeu 3 parametros",
                $"O programa recebeu 3 parametros: Ativo={ativo}, Venda={precoVenda}, Compra={precoCompra}. email teste"
            );

            Console.WriteLine("\nPressione qualquer tecla para sair.");
            Console.ReadKey();
        }

        static Configuracao? LerConfiguracao(string caminhoArquivo)
        {
            try
            {
                string jsonString = File.ReadAllText(caminhoArquivo);
                return JsonSerializer.Deserialize<Configuracao>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler o arquivo de configuração: {ex.Message}");
                return null;
            }
        }
    }

    public class Configuracao //caso o json esteja vazio, sem argmentos, ele pode gerar um erro e complicar o tempo de execucao por nao ter sido inicializado. Required faz dar erro logo se vier nulo
    {
        public required string EmailDestino { get; set; }
        public required string SmtpServidor { get; set; }
        public int SmtpPorta { get; set; }
        public bool SmtpSSL { get; set; }
        public required string EmailRemetente { get; set; }
        public required string SenhaRemetente { get; set; }
    }
}
