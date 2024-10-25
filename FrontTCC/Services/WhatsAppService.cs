using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using WindowsInput.Native;
using WindowsInput;
using System.Threading.Tasks;

namespace FrontTCC.Services
{
    public class WhatsAppService
    {
        // Método para abrir o WhatsApp Web com a conversa pré-preenchida
        public async Task OpenWhatsAppChat(string phoneNumber, string message)
        {
            // Formatar a mensagem para URL (substitui espaços por %20, etc.)
            string encodedMessage = System.Net.WebUtility.UrlEncode(message);

            // Criar a URL do WhatsApp
            string url = $"https://wa.me/{phoneNumber}?text={encodedMessage}";

            // Abrir a URL no navegador padrão
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // Usa o shell do sistema para abrir a URL
            });

            await Task.Delay(5000); // Aguarda 5 segundos (ajuste conforme necessário)

            // Simula as teclas usando InputSimulator
            var simulator = new InputSimulator();

            // Simular o pressionamento de teclas (Tab + Enter)
            simulator.Keyboard.KeyPress(VirtualKeyCode.TAB);//1
            await Task.Delay(500);
            simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);//1
            await Task.Delay(500);
            simulator.Keyboard.KeyPress(VirtualKeyCode.TAB);//2
            await Task.Delay(500);
            simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            await Task.Delay(3000);
            simulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
            await Task.Delay(500);
            simulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
            await Task.Delay(500);
            simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            await Task.Delay(10000);

            //envia
            simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            await Task.Delay(3000);

            simulator.Keyboard.KeyDown(VirtualKeyCode.CONTROL); // Pressiona Ctrl
            simulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);   // Pressiona W
            simulator.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
        }
    }
}
