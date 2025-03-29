# Projeto monitoramento B3
Desafio onde tenho que construir um sistema que monitore ativos da B3 e envie avisos sobre quando a cotação cai ou cresce além dos níveis determinados.

Esse projeto foi importante para aprendizado e primeiro contato com C#!


## Informações relevantes:

1. O arquivo "config.json" possui as configurações necessárias para enviar e-mail, incluindo o endereço de e-mail do remetente, servidor SMTP, porta e a senha. Para configurar o Outlook tem que usar o OAuth 2.0, o que se demonstrou complexo para esse projeto simples, então preferi seguir com o Gmail mesmo. No Gmail você deve gerar uma senha de aplicativo. CUIDADO para não fazer upload disso efetivamente com sua senha e chave privada (senha)!

2. Lembrando que o SmtpServidor e SmtpPorta varia de acordo com o seu provedor de e-mail. Fácil de achar.

3. E o arquivo "AlphaVantageService.cs", na linha "private readonly string_apiKey" tem que colocar uma key que você gera para testes no site da AlphaVantageService.cs. Sobre essa API:
3.1 - Foi uma boa sugestão que achei com bastante material de implementação logo após eu desistir de umas outras 2 e descobrir que a docs.fintz é paga. Ela tem algumas limitações a longo prazo, então use apenas para teste.

4. No arquivo MonitorB3.cs eu coloquei o intervalo de 3 minutos entre as requisições. Implementei uma lógica para adicionar automaticamente a extensão .SA aos códigos de ativos da Bovespa fornecidos como parâmetro. Isso permite que o usuário insira apenas o ticker do ativo (ex: PETR4) sem a necessidade de especificar a extensão completa.

### Como executar
5. Antes de você executar você tem que fazer a build e ai sim ir na pasta bin -> debugs -> net8.0 -> executar o arquivo ".exe", que no meu caso ficou "Desafio Inoa.exe".

comando:

```bash
dotnet build
```

Este projeto utiliza a biblioteca MailKit para o envio de e-mails. Caso ainda não tenha adicionado ao seu projeto, execute o seguinte comando no terminal, na raiz do projeto:

```bash
dotnet add package MailKit
```

## Considerações finais
Projeto divertido e que eu consegui entender um pouco de Csharp. Lembra bastante a quando eu estava implementando o jogo da forca em C++.

## Fontes utilizadas até então para registro (e consulta futura em caso de dúvida):
https://www.youtube.com/watch?v=rrra_oAVgdA&list=PL_yq9hmeKAk-3rsgUcaaTBjWjEh5SeaU5&index=8

https://www.reddit.com/r/investimentos/comments/1cvutfh/corretora_da_b3_com_api_para_enviar_dados_e/

https://www.reddit.com/r/investimentos/comments/i68cg1/onde_consigo_os_dados_das_a%C3%A7%C3%B5es_da_ibovespa_via/

https://www.alphavantage.co/documentation/
(toda implementação da API veio daqui! agradeço ao Reddit e Youtube)

https://www.youtube.com/watch?v=0pTty4bYZC0 (implementação AlphaVantage C#)

https://www.youtube.com/watch?v=wUfQ1bXboCA (e mais vídeos dessa série)

https://ironpdf.com/blog/net-help/mailkit-csharp-guide/

https://propreports.atlassian.net/wiki/spaces/PR/pages/589983/Listing+Exchange+Codes


## Contato
Em caso de dúvidas entre em contato comigo!