using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AI_Models.Models;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var claudeOpus = new Claude3Opus(
            configuration["ClaudeAPI:ApiKey"],
            configuration["ClaudeAPI:ApiEndpoint"],
            configuration["ClaudeAPI:OpusModel"]
        );

        var claudeSonnet = new Claude3Sonnet(
            configuration["ClaudeAPI:ApiKey"],
            configuration["ClaudeAPI:ApiEndpoint"],
            configuration["ClaudeAPI:SonnetModel"]
        );

        var claude3_5Sonnet = new Claude3_5Sonnet(
            configuration["ClaudeAPI:ApiKey"],
            configuration["ClaudeAPI:ApiEndpoint"],
            configuration["ClaudeAPI:Sonnet3_5Model"]
        );

        string mensaje = "Hola, ¿cómo estás?";

        try
        {
            string respuestaOpus = await claudeOpus.EnviarMensaje(mensaje);
            Console.WriteLine("Respuesta de Claude 3 Opus: " + respuestaOpus);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al enviar mensaje a Claude 3 Opus: " + ex.Message);
        }

        try
        {
            string respuestaSonnet = await claudeSonnet.EnviarMensaje(mensaje);
            Console.WriteLine("Respuesta de Claude 3 Sonnet: " + respuestaSonnet);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al enviar mensaje a Claude 3 Sonnet: " + ex.Message);
        }

        try
        {
            string respuesta3_5Sonnet = await claude3_5Sonnet.EnviarMensaje(mensaje);
            Console.WriteLine("Respuesta de Claude 3.5 Sonnet: " + respuesta3_5Sonnet);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al enviar mensaje a Claude 3.5 Sonnet: " + ex.Message);
        }
    }
}
