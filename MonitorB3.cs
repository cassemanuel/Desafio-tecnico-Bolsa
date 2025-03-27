// See https://aka.ms/new-console-template for more information
// lembrar de dar build para passar os args direito
using System; //tem as libs gerais
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Desafio_INOA.Services;

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

            // carregar configurações do email do json
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

            Console.WriteLine(
                "\nIniciando o monitoramento contínuo (pressione Ctrl+C para sair)...\n"
            );

            // PARTE DO MONITORAMENTO E INTEGRAÇÃO COM A API
            AlphaVantageService alphaVantageService = new AlphaVantageService();

            while (true)
            {
                decimal? cotacaoAtual = await alphaVantageService.ObterCotacaoAsync($"{ativo}.SA"); //adicionamos .SA para as ações da B3

                if (cotacaoAtual.HasValue) //verifica se deu certo a obtenção da cotação
                {
                    Console.WriteLine(
                        $"[{DateTime.Now}] Cotação atual de {ativo}: {cotacaoAtual.Value:N2}" //data e hora da cotação atual
                    );

                    //lógica de comparação e envio de e-mail
                    if (cotacaoAtual > precoVenda)
                    {
                        string assunto = $"ALERTA DE VENDA: {ativo} atingiu {cotacaoAtual:N2}";
                        string corpo =
                            $"A cotação de {ativo} subiu para {cotacaoAtual:N2}, acima do preço de venda de referência ({precoVenda:N2}).\nConsidere vender.";
                        await emailService.SendEmail(
                            config.SmtpServidor,
                            config.SmtpPorta,
                            config.SmtpSSL,
                            config.EmailRemetente,
                            config.SenhaRemetente,
                            config.EmailDestino,
                            assunto,
                            corpo
                        );
                        Console.WriteLine(
                            $"[{DateTime.Now}] E-mail de alerta de VENDA enviado para {config.EmailDestino}"
                        );
                    }
                    else if (cotacaoAtual < precoCompra)
                    {
                        string assunto = $"ALERTA DE COMPRA: {ativo} atingiu {cotacaoAtual:N2}";
                        string corpo =
                            $"A cotação de {ativo} caiu para {cotacaoAtual:N2}, abaixo do preço de compra de referência ({precoCompra:N2}).\nConsidere comprar.";
                        await emailService.SendEmail(
                            config.SmtpServidor,
                            config.SmtpPorta,
                            config.SmtpSSL,
                            config.EmailRemetente,
                            config.SenhaRemetente,
                            config.EmailDestino,
                            assunto,
                            corpo
                        );
                        Console.WriteLine(
                            $"[{DateTime.Now}] E-mail de alerta de COMPRA enviado para {config.EmailDestino}"
                        );
                    }
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now}] Falha ao obter a cotação de {ativo}.");
                }

                await Task.Delay(TimeSpan.FromMinutes(3)); // gerencia o tempo de intervalo entre as verificações
            }
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
